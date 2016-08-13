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
            this.Runner = new Task((obj) => 
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
            }, this);
        }

        public Task Runner { get; private set; }
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
                foreach (var i in clients)
                {
                    var client = Appliaction.Current.GetClientByID(i);
                    if (client == null)
                    {
                        var userID = 0L;
                        if (Clients.TryGetValue(i, out userID))
                        {
                            var request = Appliaction.Current.Client.CreateRequest<B2L_EndBattle, L2B_EndBattle>();
                            request.RequestMessage.UserID =  userID;
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
                
            }
        }

        public bool IsCompleted { private set; get; }
    }
}

