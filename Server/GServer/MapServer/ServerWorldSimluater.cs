using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EngineCore.Simulater;
using ExcelConfig;
using GameLogic.Game.Elements;
using GameLogic.Game.Perceptions;
using GameLogic.Game.States;
using XNet.Libs.Net;
using System.Linq;
using org.vxwo.csharp.json;
using System.Text;
using Proto;
using MapServer.Managers;
using ServerUtility;
using EngineCore;
using XNet.Libs.Utility;

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
        private Dictionary<long, int> _monster = new Dictionary<long, int>();
        private List<BattleCharacter> Players = new List<BattleCharacter>();
        private Dictionary<int, long> Clients = new Dictionary<int, long>();
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

        private void Tick()
        {
            LastTime = _Now;
            _Now = DateTime.UtcNow;
            GState.Tick(State, Now);
            var per = State.Perception as BattlePerception;
            var view = per.View as GameViews.BattlePerceptionView;
            view.Update();
            var notify = per.GetNotifyMessageAndClear();
            SendNotify(notify);

            if (_monster.Count == 0)
            {
                AddMonster();
            }

            if (Clients.Count == 0)
            {
                IsCompleted = true;
            }
        }

        private void AddMonster()
        { 
             
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
                            var userID = 0L;
                            if (Clients.TryGetValue(i, out userID))
                            {
                                var request = Appliaction.Current.Client.CreateRequest<B2L_EndBattle, L2B_EndBattle>();
                                request.RequestMessage.UserID = userID;
                                request.SendRequestSync();
                            }
                            this.Clients.Remove(i);
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
            try
            {
                IsCompleted = false;
                Start();
                while (!IsCompleted)
                {
                    Thread.Sleep(100);
                    Tick();
                }

                this.Tick();
                Stop();
            }
            catch (Exception ex)
            {
                Debuger.Log(ex);
            }
        }
                                

        private void Start()
        {
            State = new BattleState(new GameViews.ViewBase(), this, this);
            StartTime= _Now =  LastTime = DateTime.UtcNow;
            State.Start(this.Now);
        }

        private void Stop()
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

        public void AddClient(Client client)
        {
            if (Clients.ContainsKey(client.ID)) 
                return;
            Clients.Add(client.ID, (long)client.UserState);
        }

        public void Load(GState state)
        {
            //创建玩家
            var per = state.Perception as BattlePerception;
            foreach (var i in BattlePlayers)
            {
                var data = ExcelToJSONConfigManager.Current.GetConfigByID<CharacterData>(i.Hero.HeroID);
                var battleCharacte = per.CreateCharacter(100, data, 1, GetBornPos(),new GVector3(0, 0, 0), i.User.UserID);
                Players.Add(battleCharacte);

                per.ChangeCharacterAI(data.AIResourcePath, battleCharacte);
            }

            CreateMonster(per);
           
        }

        private void CreateMonster(BattlePerception per)
        {
            {
                var data = ExcelToJSONConfigManager.Current.GetConfigByID<CharacterData>(GRandomer.RandomMinAndMax(1,4));
                Monster = per.CreateCharacter(10, data, 2,
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
    }
}

