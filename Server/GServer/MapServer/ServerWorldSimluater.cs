using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using EngineCore.Simulater;
using ExcelConfig;
using GameLogic.Game.Elements;
using GameLogic.Game.Perceptions;
using GameLogic.Game.States;
using XNet.Libs.Net;
using org.vxwo.csharp.json;
using System.Text;
using Proto;
using MapServer.Managers;
using ServerUtility;
using EngineCore;
using XNet.Libs.Utility;
using GameLogic.Game;

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
            BattlePlayers = battlePlayers;
            this.Runner = new Thread(RunProcess);
        }

        public Thread Runner { get; private set; }
        public List<BattlePlayer> BattlePlayers { private set; get; }
        public int MapID { private set; get; }
        public int Index { private set; get; }
        public BattleState State { private set; get; }
        //private Dictionary<long, int> _monster = new Dictionary<long, int>();
        private List<BattleCharacter> Players = new List<BattleCharacter>();
        private SyncDictionary<int, long> Clients = new SyncDictionary<int, long>();
        private DateTime StartTime = DateTime.UtcNow;
        private DateTime LastTime = DateTime.UtcNow;
        private DateTime _Now = DateTime.UtcNow;
        private MapData MapConfig;

        public GTime Now
        {
            get
            {
                //var now = DateTime.UtcNow;
                return new GTime((float)(_Now - StartTime).TotalSeconds,  (float)Math.Max(0.01f,(_Now - LastTime).TotalSeconds));
            }
        }

        private float lastHpCure = 0;

        private void Tick()
        {
            LastTime = _Now;
            _Now = DateTime.UtcNow;

            GState.Tick(State, Now);
            //处理生命恢复
            var per = State.Perception as BattlePerception;
            if (lastHpCure + 1 < Now.Time)
            {
                lastHpCure = Now.Time;
                per.State.Each<BattleCharacter>((el) =>
                {
                    int hp = (int)(el[HeroPropertyType.Force].FinalValue * BattleAlgorithm.FORCE_CURE_HP);
                    if (hp > 0)
                        el.AddHP(hp);
                    return false;
                });
            }
            var view = per.View as GameViews.BattlePerceptionView;
            view.Update();
            var notify = per.GetNotifyMessageAndClear();
            SendNotify(notify);

            if (Clients.Count == 0)
            {
                IsCompleted = true;
            }
            else
            {
                if ((DateTime.UtcNow - lastTime).TotalSeconds > 3)
                {
                    CheckPlayers();
                    lastTime = DateTime.UtcNow;
                }
            }
        }

        private DateTime lastTime = DateTime.UtcNow;

        private void CheckPlayers()
        {
            var clients = this.Clients.Keys;
            if (clients != null)
            {
                foreach (var i in clients)
                {
                    var client = Appliaction.Current.GetClientByID(i);
                    if (client == null)
                    {
                        var userID = 0L;
                        if (Clients.TryToGetValue(i, out userID))
                        {
                            var request = Appliaction.Current.Client.CreateRequest<B2L_EndBattle, L2B_EndBattle>();
                            request.RequestMessage.UserID = userID;
                            request.SendRequestSync();
                        }
                        this.Clients.Remove(i);
                    }
                }
            }
        }

        private void SendNotify(Proto.ISerializerable[] notify)
        {
            if (notify.Length > 0)
            {
                var clients = this.Clients.Keys;
                if (clients != null)
                {
                    foreach (var i in clients)
                    {
                        var client = Appliaction.Current.GetClientByID(i);
                        if (client == null)
                        {
                            continue;
                        }
                        else
                        {
                            foreach (var n in notify)
                            {
                                int index = 0;
                                Proto.MessageHandleTypes.GetTypeIndex(n.GetType(), out index);
                                using (var mem = new MemoryStream())
                                {
                                    using (var bw = new BinaryWriter(mem))
                                    {
#if DEBUG
                                        var json = JsonTool.Serialize(n);
                                        var bytes = Encoding.UTF8.GetBytes(json);
                                        bw.Write(bytes);
#else
                                        n.ToBinary(bw);
#endif
                                    }
                                    client.SendMessage(new Message(MessageClass.Notify, index, mem.ToArray()));
                                }
                            }
                        }
                    }
                }
                else {
                    IsCompleted = true;
                }
            }
        }

        private void RunProcess()
        {
            IsCompleted = false;
            Start();
            try
            {
                while (!IsCompleted)
                {
                    Thread.Sleep(100);
                    Tick();
                }
                this.Tick();
            }
            catch (Exception ex)
            {
                Debuger.Log(ex);
            }
            Stop();
        }


        private void Start()
        {
            try
            {
                State = new BattleState(new GameViews.ViewBase(), this, this);
                StartTime = _Now = LastTime = DateTime.UtcNow;
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
                var clients = this.Clients.Keys;
                foreach (var i in clients)
                {
                    var client = Appliaction.Current.GetClientByID(i);
                    if (client != null)
                    {
                        var m = new Task_B2C_ExitBattle();
                        var message = NetProtoTool.ToNetMessage(MessageClass.Task, m);
                        client.SendMessage(message);
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
            Clients.Add(client.ID, (long)client.UserState);
        }

        public void Load(GState state)
        {
            //创建玩家
            var per = state.Perception as BattlePerception;
            foreach (var i in BattlePlayers)
            {
                var data = ExcelToJSONConfigManager.Current.GetConfigByID<CharacterData>(i.Hero.HeroID);
                var battleCharacte = per.CreateCharacter(i.Hero.Level, data, 1, GetBornPos(),new GVector3(0, 0, 0), i.User.UserID);
                Players.Add(battleCharacte);

                per.ChangeCharacterAI(data.AIResourcePath, battleCharacte);
            }

            { 
                var data = ExcelToJSONConfigManager.Current.GetConfigByID<CharacterData>(2);
                var battleCharacte = per.CreateCharacter(50, data, 1, GetBornPos(), new GVector3(2, 0, 0),-1);
                Players.Add(battleCharacte);

                per.ChangeCharacterAI(data.AIResourcePath, battleCharacte);
            }

            CreateMonster(per);
           
        }

        private void CreateMonster(BattlePerception per)
        {
            {
                var data = ExcelToJSONConfigManager.Current.GetConfigByID<CharacterData>(
                    GRandomer.RandomArray(new[] { 1, 3, 4 }));
                Monster = per.CreateCharacter(30, data, 2,
                                              new GVector3(GRandomer.RandomMinAndMax(0,20), 0, GRandomer.RandomMinAndMax(0, 20)),
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

