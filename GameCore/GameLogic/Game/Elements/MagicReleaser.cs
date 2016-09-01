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

        public ReleaserType RType { private set; get; }
        public MagicData Magic { private set; get; }

        public IReleaserTarget ReleaserTarget { private set; get; }

        public ReleaserStates State { private set; get; }

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
                    _add.Enqueue(player);
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

        public HashSet<GObject> _objs = new HashSet<GObject>();

        public void AttachElement(GObject el)
        {
            if (_objs.Contains(el))
            {
                return;
            }
            _objs.Add(el);
        }

        private Queue<TimeLinePlayer> _add = new Queue<TimeLinePlayer>();
        private Queue<TimeLinePlayer> _del = new Queue<TimeLinePlayer>();
        private List<TimeLinePlayer> _players = new List<TimeLinePlayer>();

        public void TickTimeLines(GTime time)
        {

            while (_del.Count > 0)
            {
                var p = _del.Dequeue();
                p.Destory();
                _players.Remove(p);
            }

            while (_add.Count > 0)
            {
                _players.Add(_add.Dequeue());
            }

            for (var i = 0; i < _players.Count; i++)
            {
                if (_players[i].Tick(time))
                {
                    _del.Enqueue(_players[i]);
                }
            }

        }

        public bool IsCompleted
        {
            get
            {

                if (State == ReleaserStates.NOStart)
                    return false;

                if (_add.Count > 0)
                    return false;
                for (var i = 0; i < _players.Count; i++)
                {
                    if (!_players[i].IsFinshed)
                        return false;
                }

                foreach (var i in _objs)
                {
                    if (i.Enable)
                        return false;
                }

                return true;
            }
        }

        public float GetLayoutTimeByPath(string path)
        {
            for (var i = 0; i < _players.Count; i++)
            {
                var p = _players[i];
                if (p.TypeEvent.layoutPath == path) return p.PlayTime;
            }
            return -1;
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
                _del.Enqueue(i);
            }


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

            foreach (var i in _objs)
            {
                Destory(i);
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
            reverts.Add(new RevertData { addtype = addType, addValue =addValue, property =property, target= effectTarget });

        }
    }
}

