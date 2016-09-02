using System;
using System.Collections.Generic;
using System.Threading;
using EngineCore.Simulater;
using ExcelConfig;
using GameLogic.Game.Elements;
using GameLogic.Game.Perceptions;
using GameLogic.Game.States;
using XNet.Libs.Net;
using Proto;
using MapServer.Managers;
using ServerUtility;
using EngineCore;
using XNet.Libs.Utility;
using GameLogic.Game;
using System.Linq;
using GameLogic.Game.AIBehaviorTree;
using Layout.LayoutEffects;
using Astar;

namespace MapServer
{
    public class ServerWorldSimluater:  ITimeSimulater,GameLogic.IStateLoader
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
            Grid.offsetX = data.Offset.x;
            Grid.offsetY = data.Offset.y;
            Grid.offsetZ = data.Offset.z;
            Grid.sizeX = data.Size.x;
            Grid.sizeY = data.Size.y;
            Grid.sizeZ = data.Size.z;
            Grid.grid = new Astar.Node[Grid.maxX,Grid.maxY,Grid.maxZ];
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

            BattlePlayers = new SyncDictionary<long, BattlePlayer>();
            foreach (var i in battlePlayers)
            {
                BattlePlayers.Add(i.User.UserID, i);
            }
            this.Runner = new Thread(RunProcess);
        }

        private MapGridData Data;
        private GridBase Grid;

        public Thread Runner { get; private set; }
        public SyncDictionary<long,BattlePlayer> BattlePlayers { private set; get; }
        public int LevelID { private set; get; }
        public int Index { private set; get; }
        public BattleState State { private set; get; }

        private Dictionary<long, BattleCharacter> UserCharacters = new Dictionary<long, BattleCharacter>();
        private SyncDictionary<long,Client> Clients = new SyncDictionary<long, Client>();
        //private SyncList<long> _dels = new SyncList<long>();
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

        private void ExitUser(long userid)
        {
            MapSimulaterManager.Singleton.DeleteUser(userid,true);
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
                    Clients.Add((long)client.UserState, client);

                    BattlePlayer battlePlayer;
                    //package
                    if (BattlePlayers.TryToGetValue((long)client.UserState, out battlePlayer))
                    {
                        var package = battlePlayer.GetNotifyPackage();
                        package.TimeNow = Now.Time;
                        client.SendMessage(NetProtoTool.ToNetMessage(MessageClass.Notify, package));
                    }

                    var createNotify = view.GetInitNotify();
                    var messages = ToMessages(createNotify);
                    //Notify package
                    foreach (var i in messages)
                    {
                        client.SendMessage(i);
                    }

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
                ISerializerable action;
                Message msg;
                if (i.Value.TryGetActionMessage(out msg))
                {
                    action = NetProtoTool.GetProtoMessage(msg);
                    BattleCharacter userCharacter;
                    if (UserCharacters.TryGetValue(i.Key, out userCharacter))
                    {
                        if (action is Action_AutoFindTarget)
                        {
                            var auto = action as Action_AutoFindTarget;
                            userCharacter.ModifyValue(HeroPropertyType.ViewDistance,
                                                      AddType.Append, !auto.Auto ? 0 : 1000 * 100); //修改玩家AI视野
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
            if (Clients.Count == 0)
            {
                IsCompleted = true;
            }
        }

        private List<Message> ToMessages(Proto.ISerializerable[] notify)
        {
            return notify.Select(t => NetProtoTool.ToNetMessage(MessageClass.Notify, t)).ToList();
        }

        private void SendNotify(Proto.ISerializerable[] notify)
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

        private void RunProcess()
        {
            IsCompleted = false;
            Start();
            try
            {
                int maxTime = Appliaction.SERVER_TICK;
                while (!IsCompleted)
                {
                    DateTime begin = DateTime.Now;
                    delteTime = (float)maxTime / 1000f;
                    time += delteTime;
                    Tick();
                   
                    var cost = (int)(DateTime.Now -begin).TotalMilliseconds;
                    if (maxTime <= cost)
                    {
                        Debuger.LogWaring(
                            string.Format("WorldSimulater {2} Timeout, Want {0}ms real cost {1}ms",
                                          maxTime,cost,Index));
                    }
                    Thread.Sleep(Math.Max(0,maxTime -cost));
                }
                Tick();
            }
            catch (Exception ex)
            {
                Debuger.LogError(ex);
            }
            Stop();
        }


        private void Start()
        {
            try
            {

                State = new BattleState(new GameViews.ViewBase(new Astar.Pathfinder(this.Grid)), this, this);
                State.Start(this.Now);
            }
            catch (Exception ex)
            {
                Debuger.LogError(ex);
            }
        }

        private void Stop()
        {
            try
            {
                State.Stop(this.Now);
                foreach (var i in Clients)
                {
                    if (i.Value.Enable)
                    {
                        var m = new Task_B2C_ExitBattle();
                        var message = NetProtoTool.ToNetMessage(MessageClass.Task, m);
                        i.Value.SendMessage(message);
                    }
                }
            }
            catch (Exception ex)
            {
                Debuger.LogError(ex);
            }
        }

        private Queue<Client> _addTemp = new Queue<Client>();
        private object syncRoot = new object();

        public void AddClient(Client client)
        {
            lock (syncRoot)
            {
                _addTemp.Enqueue(client);
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
            var data = ExcelToJSONConfigManager.Current.GetConfigByID<CharacterData>(user.Hero.HeroID);
            var magic = ExcelToJSONConfigManager.Current.GetConfigs<CharacterMagicData>(t => { 
                return t.CharacterID == data.ID; });
            var battleCharacte = per.CreateCharacter(
                user.Hero.Level, data, magic.ToList(), 1,
                GetBornPos(), new GVector3(0, 0, 0), user.User.UserID);
            battleCharacte.ModifyValue(HeroPropertyType.ViewDistance,AddType.Append, 1000 * 100); //修改玩家AI视野
            battleCharacte.Speed += 2;
            UserCharacters.Add(user.User.UserID, battleCharacte);
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
            var items = drop.DropItem.SplitToInt('|');
            var pors = drop.Pro.SplitToInt('|');
            foreach (var i in this.BattlePlayers)
            {
                var notify = new Notify_Drop();
                notify.UserID = i.Value.User.UserID;
                var gold = GRandomer.RandomMinAndMax(drop.GoldMin, drop.GoldMax);
                notify.Gold = gold;
                i.Value.ModifyGold(gold);
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
                Client client;
                if (this.Clients.TryToGetValue(i.Value.User.UserID, out client))
                {
                    var message = NetProtoTool.ToNetMessage(MessageClass.Notify, notify);
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

                var groups = LevelData.MonsterGroupID.SplitToInt('|');

                var monsterGroups = ExcelToJSONConfigManager.Current.GetConfigs<MonsterGroupData>(t =>
                {
                    return groups.Contains(t.ID);
                });

               
                var monsterGroup = GRandomer.RandomArray(monsterGroups);
                drop = ExcelToJSONConfigManager.Current.GetConfigByID<DropGroupData>(monsterGroup.DropID);

                int maxCount = GRandomer.RandomMinAndMax(monsterGroup.MonsterNumberMin, monsterGroup.MonsterNumberMax);
                for (var i = 0; i < maxCount; i++)
                {
                    var m = monsterGroup.MonsterID.SplitToInt('|');
                    var p = monsterGroup.Pro.SplitToInt('|').ToArray();
                    var monsterID = m[GRandomer.RandPro(p)];
                    var monsterData = ExcelToJSONConfigManager.Current.GetConfigByID<MonsterData>(monsterID);
                    var data = ExcelToJSONConfigManager.Current.GetConfigByID<CharacterData>(monsterData.CharacterID);
                    var magic = ExcelToJSONConfigManager.Current.GetConfigs<CharacterMagicData>(t => { return t.CharacterID == data.ID; });
                    var Monster = per.CreateCharacter(monsterData.Level,
                                                      data,
                                                      magic.ToList(),
                                                      2,
                                                      group.Pos.ToGVector3() 
                                                      + new GVector3(GRandomer.RandomMinAndMax(-1,1), 0,GRandomer.RandomMinAndMax(-1, 1)) * i,
                                                   new GVector3(0, 0, 0), -1);
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
                    Monster[HeroPropertyType.MaxMP]
                        .SetBaseValue(Monster[HeroPropertyType.MaxHP].BaseValue + monsterData.HPMax);
                    Monster[HeroPropertyType.MaxMP]
                        .SetBaseValue(Monster[HeroPropertyType.MaxMP].BaseValue + monsterData.HPMax);
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


        public GVector3 GetBornPos()
        {
            return new GVector3(Data.Born.X, Data.Born.Y, Data.Born.Z);
        }

        public bool IsCompleted { private set; get; }

        public void Exit()
        {
            IsCompleted = true;
        }
    }
}

