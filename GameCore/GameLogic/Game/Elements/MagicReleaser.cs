using System;
using Layout;
using GameLogic.Game.LayoutLogics;
using EngineCore.Simulater;
using System.Collections.Generic;
using GameLogic.Game.Perceptions;
using Layout.LayoutEffects;
using Proto;

namespace GameLogic.Game.Elements
{
    public enum ReleaserStates
    {
        NOStart,
        Releasing,
        ToComplete,
        Completing,
        Ended
    }

    public enum ReleaserType
    {
        Magic,
        Buff
    }

    public class MagicReleaser : BattleElement<IMagicReleaser>
    {
        public MagicReleaser(
            MagicData magic,
            IReleaserTarget target,
            GControllor controllor,
            IMagicReleaser view,
            ReleaserType type)
            : base(controllor, view)
        {
            ReleaserTarget = target;
            Magic = magic;
            RType = type;
            OnExitedState = ReleaseAll;
        }

        public void SetParam(params string[] parms)
        {
            Params = parms;
        }

        private string[] Params;

        public string this[int paramIndex]
        { 
            get {
                if (Params == null) return string.Empty;
                if (paramIndex < 0 || paramIndex >= Params.Length) return string.Empty;
                return Params[paramIndex];
            }
        }

        public ReleaserType RType { private set; get; }

        public MagicData Magic { private set; get; }

        public IReleaserTarget ReleaserTarget { private set; get; }

        public ReleaserStates State { private set; get; }

        public int UnitCount { get { return this._objs.Count; }}

        public void SetState(ReleaserStates state)
        {
            State = state;
        }

        public void OnEvent(EventType eventType)
        {
            var per = this.Controllor.Perception as BattlePerception;

            LastEvent = eventType;
            for (var index = 0; index < Magic.Containers.Count; index++)
            {
                var i = Magic.Containers[index];
                if (i.type == eventType)
                {
                    var timeLine = i.line == null ? per.View.GetTimeLineByPath(i.layoutPath) : i.line;
                    var player = new TimeLinePlayer(timeLine, this, i);
                    _players.AddLast(player);
                    if (i.type == EventType.EVENT_START)
                    {
                        if (startLayout != null)
                        {
                            throw new Exception("Start layout must have one!");
                        }
                        startLayout = player;
                    }
                }
            }
        }

        private TimeLinePlayer startLayout;

        public class AttachedElement
        {
            public GObject Element;
            public float time;
            public bool HaveLeftTime;
        }

        private Dictionary<int, AttachedElement> _objs = new Dictionary<int, AttachedElement>();

        public void AttachElement(GObject el, float time = -1f)
        {
            if (_objs.ContainsKey(el.Index))
            {
                return;
            }
            _objs.Add(el.Index,
                      new AttachedElement()
                      {
                          time = time,
                          Element = el,
                          HaveLeftTime = time >= 0f
                      });
        }

        private LinkedList<TimeLinePlayer> _players = new LinkedList<TimeLinePlayer>();
        private Queue<long> _removeTemp = new Queue<long>();

        public void TickTimeLines(GTime time)
        {
            var current = _players.First;
            while (current != null)
            {
                if (current.Value.Tick(time))
                {
                    _players.Remove(current);
                }
                current = current.Next;
            }

            foreach (var i in _objs)
            {
                if (i.Value.Element.IsAliveAble)
                {
                    if (i.Value.HaveLeftTime)
                    {
                        i.Value.time -= time.DeltaTime;
                        var character = i.Value.Element as BattleCharacter;
                        if (character != null)
                        {
                            if (i.Value.time <= 0)
                            {
                                character.SubHP(character.MaxHP);
                            }
                        }
                    }
                    continue;
                }
                else
                {
                    _removeTemp.Enqueue(i.Key);
                    OnEvent(EventType.EVENT_UNIT_DEAD);
                }
            }

        }

        public bool IsCompleted
        {
            get
            {

                if (State == ReleaserStates.NOStart)
                    return false;

                var current = _players.First;
                while (current != null)
                {
                    if (!current.Value.IsFinshed) return false;
                    current = current.Next;
                }

                if (_objs.Count > 0)
                {
                    foreach (var i in _objs)
                    {
                        if (i.Value.Element.Enable)
                            return false;
                    }

                }
                return true;
            }
        }

        public float GetLayoutTimeByPath(string path)
        {
            foreach (var i in _players)
            {
                if (i.TypeEvent.layoutPath == path) return i.PlayTime;
            }
            return -1f;
        }

        public EventType? LastEvent { get; private set; }

        public bool IsLayoutStartFinish
        {
            get
            {
                if (State == ReleaserStates.NOStart) return false;
                if (State == ReleaserStates.Releasing && startLayout != null)
                    return startLayout.IsFinshed;
                return true;
            }
        }

        public void StopAllPlayer()
        {
            foreach (var i in _players) 
            {
                i.Destory();
            }
            _players.Clear();
        }

        private void ReleaseAll(GObject el)
        {

            foreach (var i in reverts)
            {
                if (i.target.Enable)
                {
                    i.target.ModifyValue(i.property, i.addtype, -i.addValue);
                }
            }

            foreach (var i in actionReverts)
            {
                if (i.target.Enable)
                {
                    i.target.Lock.Unlock(i.type);
                }
            }

            actionReverts.Clear();
            reverts.Clear();

            foreach (var i in _objs)
            {
                Destory(i.Value.Element);
            }

            _objs.Clear();
            foreach (var i in _players)
            {
                i.Destory();
            }

            _players.Clear();

        }

        public bool IsRuning(EventType type)
        {
            foreach (var i in _players)
            {
                if (i.IsFinshed) continue;
                if (i.TypeEvent.type == type) return true;
            }

            return false;
        }

        public float LastTickTime = -1;
        public float tickStartTime = -1;

        private class RevertData
        {
            public BattleCharacter target;
            public HeroPropertyType property;
            public AddType addtype;
            public float addValue;
        }

        private List<RevertData> reverts = new List<RevertData>();
        internal void RevertProperty(BattleCharacter effectTarget, HeroPropertyType property, AddType addType, float addValue)
        {
            reverts.Add(new RevertData { addtype = addType, addValue = addValue, property = property, target = effectTarget });

        }

        private class RevertActionLock
        {
            public BattleCharacter target;
            public ActionLockType type;
        }

        private List<RevertActionLock> actionReverts = new List<RevertActionLock>();
        internal void RevertLock(BattleCharacter effectTarget, ActionLockType lockType)
        {
            actionReverts.Add(new RevertActionLock { target = effectTarget, type = lockType });
        }
    }
}

