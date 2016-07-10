using System;
using System.Collections.Generic;
using BehaviorTree;
using EngineCore.Simulater;
using GameLogic.Game.Elements;
using GameLogic.Game.Perceptions;

namespace GameLogic.Game.AIBehaviorTree
{
	public class AITreeRoot:ITreeRoot
	{
		public AITreeRoot(ITimeSimulater timeSimulater, BattleCharacter userstate, Composite root)
		{
			TimeSimulater = timeSimulater;
			UserState = userstate;
			_char = userstate;
			Root = root;
		}

		public ITimeSimulater TimeSimulater { private set; get; }

		public float Time
		{
			get
			{
				return TimeSimulater.Now.Time;
			}
		}

		public BattlePerception Perception { get { return _char.Controllor.Perception as BattlePerception; }}

		public object UserState
		{
			get;
			private set;
		}

		private BattleCharacter _char;

		public BattleCharacter Character { get { return _char; }}

		public Composite Root { private set; get; }

		public void Tick()
		{
			if (Current == null)
			{
				Current = Root;
			}

			if (next != null)
			{
				Current.Stop(this);
				Current = next;
				next = null;
				Current.Start(this);
			}

			if (Current.LastStatus == null)
			{
				Current.Start(this);
			}

			Current.Tick(this);
			if (Current.LastStatus.HasValue
				&& Current.LastStatus.Value != BehaviorTree.RunStatus.Running)
			{
				Current.Stop(this);
				//重新从根执行
				Current = Root;
				Current.Start(this);
			}
		}

		private Composite next;

		public void Chanage(Composite cur)
		{
			next = cur;
		}

		private Composite Current;

		private Dictionary<string, object> _blackbroad = new Dictionary<string, object>();

		public object this[string key] { 
			set {
				if (_blackbroad.ContainsKey(key)) _blackbroad[key] = value;
				else
					_blackbroad.Add(key, value);
			}
			get {
				object v;
				if (_blackbroad.TryGetValue(key, out v)) return v;
				return null;
			}
		}

	}
}

