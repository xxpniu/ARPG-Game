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

namespace MapServer
{
    public class ServerWorldSimluater:  ITimeSimulater,GameLogic.IStateLoader
    {
        public ServerWorldSimluater(int mapID, int index ,List<BattlePlayer> battlePlayers)
        {
            MapID = mapID;
            Index = index;
            IsCompleted = false;
            MapConfig = ExcelToJSONConfigManager.Current.GetConfigByID<MapData>(mapID);
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

        private Astar.GridBase Grid;

        public Thread Runner { get; private set; }
        public SyncDictionary<long,BattlePlayer> BattlePlayers { private set; get; }
        public int MapID { private set; get; }
        public int Index { private set; get; }
        public BattleState State { private set; get; }

        private Dictionary<long, BattleCharacter> UserCharacters = new Dictionary<long, BattleCharacter>();
        private SyncDictionary<long,Client> Clients = new SyncDictionary<long, Client>();
        //private SyncList<long> _dels = new SyncList<long>();
        private MapData MapConfig;

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

        private void Tick()
        {
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
                        if (userCharacter.AIRoot != null)
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
            var per = State.Perception as BattlePerception;
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
            var view = per.View as GameViews.BattlePerceptionView;
            view.Update(Now);
            var viewNotify = view.GetAndClearNotify();
            SendNotify(viewNotify);
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

        public void AddClient(Client client)
        {
            Clients.Add((long)client.UserState, client);
        }

        public void Load(GState state)
        {
            //创建玩家
            var per = state.Perception as BattlePerception;
            foreach (var i in BattlePlayers.Values)
            {
                var data = ExcelToJSONConfigManager.Current.GetConfigByID<CharacterData>(i.Hero.HeroID);
                var magic = ExcelToJSONConfigManager.Current.GetConfigs<CharacterMagicData>(t => { return t.CharacterID == data.ID; });
                var battleCharacte = per.CreateCharacter(
                    i.Hero.Level, data,magic.ToList(), 1,
                    GetBornPos(),new GVector3(0, 0, 0), i.User.UserID);
                battleCharacte.ModifyValue(HeroPropertyType.ViewDistance, 
                                           AddType.Append, 1000*100); //修改玩家AI视野
                UserCharacters.Add(i.User.UserID, battleCharacte);
                per.ChangeCharacterAI(data.AIResourcePath, battleCharacte);
            }

            { 
                var data = ExcelToJSONConfigManager.Current.GetConfigByID<CharacterData>(2);
                var magic = ExcelToJSONConfigManager.Current.GetConfigs<CharacterMagicData>(t => { return t.CharacterID == data.ID; });

                var battleCharacte = per.CreateCharacter(50, data,magic.ToList(), 1,  GetBornPos(), new GVector3(2, 0, 0),-1);
                per.ChangeCharacterAI(data.AIResourcePath, battleCharacte);
            }

            CreateMonster(per);
           
        }

        private void CreateMonster(BattlePerception per)
        {
            {
                var pos = new GVector3[] {
                    new GVector3(1,0,10),
                    new GVector3(120,0,120)
                };
                var data = ExcelToJSONConfigManager.Current.GetConfigByID<CharacterData>(
                    GRandomer.RandomArray(new[] { 1, 3, 4 }));
                var magic = ExcelToJSONConfigManager.Current.GetConfigs<CharacterMagicData>(t => { return t.CharacterID ==  data.ID; });

                Monster = per.CreateCharacter(30, data, magic.ToList(), 2,
                                              GRandomer.RandomArray(pos),
                                              new GVector3(0, 0, 0), -1);
                per.ChangeCharacterAI(data.AIResourcePath, Monster);
                Monster.OnDead = (el) => {
                    CreateMonster(per);
                };
            }
        }

        private BattleCharacter Monster;

        public GVector3 GetBornPos()
        {
            return new GVector3(0, 0, 0);
        }

        public bool IsCompleted { private set; get; }

        public void Exit()
        {
            IsCompleted = true;
        }
    }
}

