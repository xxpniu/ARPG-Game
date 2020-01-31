using System;
using System.Collections.Generic;
using EngineCore.Simulater;
using ExcelConfig;
using GameLogic.Game.Elements;
using GameLogic.Game.Perceptions;
using GameLogic.Game.States;
using XNet.Libs.Net;
using Proto;
using MapServer.Managers;
using ServerUtility;
using XNet.Libs.Utility;
using GameLogic.Game;
using System.Linq;
using GameLogic.Game.AIBehaviorTree;
using Layout.LayoutEffects;
using Astar;
using org.vxwo.csharp.json;
using UMath;
using GameLogic;
using EConfig;
using Google.Protobuf;

namespace MapServer
{
    public class ServerWorldSimluater: ITimeSimulater,IStateLoader,IUpdateThread
    {
        public ServerWorldSimluater(int levelId, int index ,List<BattlePlayer> battlePlayers)
        {
            LevelID = levelId;
            Index = index;
            IsCompleted = false;
            LevelData = ExcelToJSONConfigManager.Current.GetConfigByID<BattleLevelData>(levelId);
            MapConfig = ExcelToJSONConfigManager.Current.GetConfigByID<MapData>(LevelData.MapID);
            var mapGridName = MapConfig.LevelName + ".bin";
            var data = ResourcesLoader.Singleton.GetMapGridByLevel(mapGridName);
            Grid = new Astar.GridBase();
            Grid.maxX = data.MaxX;
            Grid.maxY = data.MaxY;
            Grid.maxZ = data.MaxZ;
            Grid.offsetX = data.Offset.X;
            Grid.offsetY = data.Offset.Y;
            Grid.offsetZ = data.Offset.Z;
            Grid.sizeX = data.Size.X;
            Grid.sizeY = data.Size.Y;
            Grid.sizeZ = data.Size.Z;
            Grid.grid = new Node[Grid.maxX,Grid.maxY,Grid.maxZ];
            Data = data;
            foreach (var i in data.Nodes)
            {
                Grid.grid[i.X, i.Y, i.Z] = new Astar.Node()
                {
                    x = i.X,
                    isWalkable = i.IsWalkable,
                    y = i.Y,
                    z = i.Z
                };
            }

            BattlePlayers = new SyncDictionary<string, BattlePlayer>();
            foreach (var i in battlePlayers)
            {
                BattlePlayers.Add(i.User.AccountUuid, i);
            }
            //this.Runner = new Thread(RunProcess);
        }

        private MapGridData Data;
        private GridBase Grid;
        public SyncDictionary<string,BattlePlayer> BattlePlayers { private set; get; }
        public int LevelID { private set; get; }
        public int Index { private set; get; }
        public BattleState State { private set; get; }

        private Dictionary<string, BattleCharacter> UserCharacters = new Dictionary<string, BattleCharacter>();
        private SyncDictionary<string,Client> Clients = new SyncDictionary<string, Client>();

        private MapData MapConfig;
        private BattleLevelData LevelData;

        private float time = 0;
        private float delteTime = 0.1f;

        public GTime Now
        {
            get
            {
                return new GTime(time,delteTime);
            }
        }

        private void ExitUser(string account_uuid)
        {
            MonitorPool.Singleton
                       .GetMointor<MapSimulaterManager>().DeleteUser(account_uuid, true);
        }

        private float lastHpCure = 0;

        private void ProcessJoinClient()
        {
            var per = State.Perception as BattlePerception;
            var view = per.View as GameViews.BattlePerceptionView;
            lock (syncRoot)
            {
                //send Init message.
                while (_addTemp.Count > 0)
                {
                    var client = _addTemp.Dequeue();
                    Clients.Add((string)client.UserState, client);
                    //package
                    if (BattlePlayers.TryToGetValue((string)client.UserState, out BattlePlayer battlePlayer))
                    {
                        var package = battlePlayer.GetNotifyPackage();
                        package.TimeNow = (Now.Time);
                        client.SendMessage(package.ToNotityMessage());
                    }

                    var createNotify = view.GetInitNotify();
                    var messages = ToMessages(createNotify);
                    //Notify package
                    foreach (var i in messages)
                    {
                        client.SendMessage(i);
                    }

                }
                while (_kickUsers.Count > 0)
                {
                    //kick
                    var u = _kickUsers.Dequeue();
                    BattlePlayers.Remove(u);
                    Clients.Remove(u);
                    per.State.Each<BattleCharacter>((el) =>
                    {
                        if (el.AcccountUuid == u)
                        {
                            GObject.Destory(el);
                        }
                        return false;
                    });
                }
            }

        }

        private void Tick()
        {
            var per = State.Perception as BattlePerception;
            var view = per.View as GameViews.BattlePerceptionView;
            //sync Root

            if (AliveCount == 0)
            {
                CreateMonster(per);
            }



            //处理用户输入
            foreach (var i in Clients)
            {
                if (!i.Value.Enable)
                {
                    Clients.Remove(i.Key);
                    ExitUser(i.Key);//send msg
                    continue;
                }
                IMessage action;
                if (i.Value.TryGetActionMessage(out Message msg))
                {
                    action = msg.AsAction();

                    if (NetProtoTool.EnableLog)
                    {
                        Debuger.Log(action.GetType().ToString() + "-->" + JsonTool.Serialize(action));
                    }

                    if (UserCharacters.TryGetValue(i.Key, out BattleCharacter userCharacter))
                    {
                        if (action is Action_AutoFindTarget)
                        {
                            var auto = action as Action_AutoFindTarget;
                            userCharacter.ModifyValue(HeroPropertyType.ViewDistance,
                                                      AddType.Append, !auto.Auto ? 0 : 1000 * 100); //修改玩家AI视野
                            userCharacter.AIRoot.BreakTree();
                        }
                        else if (userCharacter.AIRoot != null)
                        {
                            //保存到AI
                            userCharacter.AIRoot[AITreeRoot.ACTION_MESSAGE] = action;
                            userCharacter.AIRoot.BreakTree();//处理输入 重新启动行为树
                        }
                    }
                }
            }

            GState.Tick(State, Now);
            //处理生命,魔法恢复

            if (lastHpCure + 1 < Now.Time)
            {
                lastHpCure = Now.Time;
                per.State.Each<BattleCharacter>((el) =>
                {
                    var hp = (int)(el[HeroPropertyType.Force].FinalValue * BattleAlgorithm.FORCE_CURE_HP);
                    if (hp > 0)
                        el.AddHP(hp);
                    var mp =(int)(el[HeroPropertyType.Knowledge].FinalValue * BattleAlgorithm.KNOWLEDGE_CURE_MP);
                    if (mp > 0)
                        el.AddMP(mp);
                    return false;
                });
            }

            view.Update(Now);
            var viewNotify = view.GetAndClearNotify();
            SendNotify(viewNotify);

            ProcessJoinClient();
            IsCompleted |= Clients.Count == 0;
        }

       
        private List<Message> ToMessages(IMessage[] notify)
        {
            return notify.Select(t => t.ToNotityMessage()).ToList();
        }

        private void SendNotify(IMessage[] notify)
        {
            if (notify.Length > 0)
            {
                var messages = ToMessages(notify);
                foreach (var i in Clients)
                {
                    if (!i.Value.Enable)
                    {
                        continue;
                    }
                    foreach (var m in messages)
                        i.Value.SendMessage(m);
                }
            }
        }

        private int lastTick = 0;

        bool IUpdateThread.Update()
        {
            try
            {
                int now = Environment.TickCount;
                if (now < lastTick) return false;
                var maxTime = Appliaction.SERVER_TICK;
                delteTime = (float)maxTime/ 1000f;
                time += delteTime;
                Tick();
                lastTick = maxTime + now;
                return IsCompleted;
            }
            catch (Exception ex)
            {
                Debuger.LogError(ex);
            }
            return true;
        }

        void IUpdateThread.Exit()
        { 
            Stop();
        }

        void IUpdateThread.Begin()
        { 
            Start();
        }

        private void Start()
        {
            try
            {
                lastTick = -1;
                State = new BattleState(new GameViews.ViewBase(new Pathfinder(this.Grid)), this, this);
                State.Start(this.Now);
            }
            catch (Exception ex)
            {
                Debuger.LogError(ex);
            }
        }

        private void Stop()
        {
            MonitorPool.Singleton.GetMointor<SimulaterManager>().EndSumlater(this.Index);
            try
            {
                State.Stop(this.Now);
                foreach (var i in Clients)
                {
                    if (i.Value.Enable)
                    {
                        i.Value.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Debuger.LogError(ex);
            }
        }

        private readonly Queue<Client> _addTemp = new Queue<Client>();
        private readonly Queue<string> _kickUsers = new Queue<string>();
        private readonly object syncRoot = new object();

        public void AddClient(Client client)
        {
            lock (syncRoot)
            {
                _addTemp.Enqueue(client);
            }
        }

        public void KickUser(string account_uuid)
        { 
            lock(syncRoot)
            {
                _kickUsers.Enqueue(account_uuid);
            }
        }

        public void Load(GState state)
        {
            foreach (var i in BattlePlayers.Values)
            {
                CreateUser(i, state);
            }
        }

        //创建角色
        private void CreateUser(BattlePlayer user,GState state)
        {
            var per = state.Perception as BattlePerception;
            var data = ExcelToJSONConfigManager.Current.GetConfigByID<CharacterData>(user.GetHero().HeroID);
            var magic = ExcelToJSONConfigManager.Current.GetConfigs<CharacterMagicData>(t => 
            { 
                return t.CharacterID == data.ID; 
            });

            //处理装备加成
            var battleCharacte = per.CreateCharacter(
                user.GetHero().Level, data, magic.ToList(), 1,
                GetBornPos(), new UVector3(0, 0, 0), user.User.AccountUuid);

            foreach (var i in user.GetHero().Equips)
            {
                var itemsConfig = ExcelToJSONConfigManager.Current.GetConfigByID<ItemData>(i.ItemID);
                var equipId = int.Parse(itemsConfig.Params1);
                var equipconfig = ExcelToJSONConfigManager.Current.GetConfigByID<EquipmentData>(equipId);
                if (equipconfig == null) continue;
                var equip = user.GetEquipByGuid(i.GUID);
                float addRate = 0f;
                if (equip != null)
                {
                    var equipLevelUp = ExcelToJSONConfigManager
                        .Current
                        .FirstConfig<EquipmentLevelUpData>(t => t.Level == equip.Level && t.Quility == equipconfig.Quility);
                    if (equipLevelUp != null)
                    {
                        addRate = (float)equipLevelUp.AppendRate / 10000f;
                    }
                }
                //基础加成
                var properties = equipconfig.Properties.SplitToInt();
                var values = equipconfig.PropertyValues.SplitToInt();
                for (var index = 0; index < properties.Count; index++)
                {
                    var p = (HeroPropertyType)properties[index];
                    var v = battleCharacte[p].BaseValue + (float)values[index] *( 1+addRate);
                    battleCharacte[p].SetBaseValue((int)v);
                }
            }
            battleCharacte.ModifyValue(HeroPropertyType.ViewDistance,AddType.Append, 1000 * 100); //修改玩家AI视野
            UserCharacters.Add(user.User.AccountUuid, battleCharacte);
            per.ChangeCharacterAI(data.AIResourcePath, battleCharacte);
        }

        private int CountKillCount = 0;
        private int AliveCount = 0;
        private MapMonsterGroup group;
        private DropGroupData drop;

        //处理掉落
        private void DoDrop()
        {
            if (drop == null) return;
            var items = drop.DropItem.SplitToInt();
            var pors = drop.Pro.SplitToInt();
            foreach (var i in this.BattlePlayers)
            {
                var notify = new Notify_Drop();
                notify.AccountUuid = i.Value.User.AccountUuid;
                var gold = GRandomer.RandomMinAndMax(drop.GoldMin, drop.GoldMax);
                notify.Gold = gold;
                i.Value.AddGold(gold);
                if (items.Count > 0)
                {
                    for (var index = 0; index < items.Count; index++)
                    {
                        if (GRandomer.Probability10000(pors[index]))
                        {
                            i.Value.AddDrop(items[index], 1);
                            notify.Items.Add(new PlayerItem { ItemID = items[index], Num =1 });
                        }
                    }
                }
                if (Clients.TryToGetValue(i.Value.User.AccountUuid, out Client client))
                {
                    var message = notify.ToNotityMessage();
                    client.SendMessage(message);
                }
            }
        }

        //处理怪物生成
        private void CreateMonster(BattlePerception per)
        {
            //process Drop;
            if (drop != null)
            {
                DoDrop();
            }

            {
                var groupPos = Data.Monsters.Where(t => t!=group)
                              .ToArray();
                 group = GRandomer.RandomArray(groupPos);

                var groups = LevelData.MonsterGroupID.SplitToInt();

                var monsterGroups = ExcelToJSONConfigManager.Current.GetConfigs<MonsterGroupData>(t =>
                {
                    return groups.Contains(t.ID);
                });

               
                var monsterGroup = GRandomer.RandomArray(monsterGroups);
                drop = ExcelToJSONConfigManager.Current.GetConfigByID<DropGroupData>(monsterGroup.DropID);

                int maxCount = GRandomer.RandomMinAndMax(monsterGroup.MonsterNumberMin, monsterGroup.MonsterNumberMax);
                for (var i = 0; i < maxCount; i++)
                {
                    var m = monsterGroup.MonsterID.SplitToInt();
                    var p = monsterGroup.Pro.SplitToInt().ToArray();
                    var monsterID = m[GRandomer.RandPro(p)];
                    var monsterData = ExcelToJSONConfigManager.Current.GetConfigByID<MonsterData>(monsterID);
                    var data = ExcelToJSONConfigManager.Current.GetConfigByID<CharacterData>(monsterData.CharacterID);
                    var magic = ExcelToJSONConfigManager.Current.GetConfigs<CharacterMagicData>(t => { return t.CharacterID == data.ID; });
                    var Monster = per.CreateCharacter(monsterData.Level,
                                                      data,
                                                      magic.ToList(),
                                                      2,
                                                      group.Pos.ToGVector3() 
                                                      + new UVector3(GRandomer.RandomMinAndMax(-1,1), 0,
                                                      GRandomer.RandomMinAndMax(-1, 1)) * i,
                                                   new UVector3(0, 0, 0), string.Empty);
                    //data
                    Monster[HeroPropertyType.DamageMax]
                        .SetBaseValue(Monster[HeroPropertyType.DamageMax].BaseValue + monsterData.DamageMax);
                    Monster[HeroPropertyType.DamageMin]
                        .SetBaseValue(Monster[HeroPropertyType.DamageMin].BaseValue + monsterData.DamageMax);
                    Monster[HeroPropertyType.Force]
                        .SetBaseValue(Monster[HeroPropertyType.Force].BaseValue + monsterData.Force);
                    Monster[HeroPropertyType.Agility]
                        .SetBaseValue(Monster[HeroPropertyType.Agility].BaseValue + monsterData.Agility);
                    Monster[HeroPropertyType.Knowledge]
                        .SetBaseValue(Monster[HeroPropertyType.Knowledge].BaseValue + monsterData.Knowledeg);
                    Monster[HeroPropertyType.MaxHp]
                        .SetBaseValue(Monster[HeroPropertyType.MaxHp].BaseValue + monsterData.HPMax);
                    Monster[HeroPropertyType.MaxMp]
                        .SetBaseValue(Monster[HeroPropertyType.MaxMp].BaseValue);
                    Monster.Name = string.Format("{0}.{1}", monsterData.NamePrefix, data.Name);

                    Monster.Reset();
                    per.ChangeCharacterAI(data.AIResourcePath, Monster);

                    AliveCount ++;
                    Monster.OnDead = (el) =>
                    {
                        CountKillCount++;
                        AliveCount--;
                    };


                }
            }
        }

        public UVector3 GetBornPos()
        {
            return new UVector3(Data.Born.X, Data.Born.Y, Data.Born.Z);
        }

        public bool IsCompleted { private set; get; }

        public void Exit()
        {
            IsCompleted = true;
        }
    }
}

