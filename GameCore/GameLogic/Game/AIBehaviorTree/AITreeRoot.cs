using System;
using System.Collections.Generic;
using BehaviorTree;
using EngineCore.Simulater;
using GameLogic.Game.Elements;
using GameLogic.Game.Perceptions;
using Layout.AITree;

namespace GameLogic.Game.AIBehaviorTree
{
	public class AITreeRoot:ITreeRoot
	{

		public const string SELECT_MAGIC_ID = "MagicID";
		public const string TRAGET_INDEX = "TargetIndex";
			

		public AITreeRoot(ITimeSimulater timeSimulater, BattleCharacter userstate, Composite root,
		                  TreeNode nodeRoot)
		{
			TimeSimulater = timeSimulater;
			UserState = userstate;
			_char = userstate;
			Root = root;
			NodeRoot = nodeRoot;
		}

		public bool GetDistanceByValueType(DistanceValueOf type, float value, out float outValue)
		{
			outValue = value;
			switch (type)
			{
				case DistanceValueOf.BlackboardMaigicRangeMax:
					{
						var data = this[SELECT_MAGIC_ID];
						if (data == null)
						{
							return false;
						}
						var magic = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.CharacterMagicData>((int)data);
						if (magic == null)
						{
							return false;
						}
						outValue = magic.ReleaseRangeMax;
					}
					break;
				case DistanceValueOf.BlackboardMaigicRangeMin:
					{
						var data = this[SELECT_MAGIC_ID];
						if (data == null)
						{
							return false;
						}
						var magic = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.CharacterMagicData>((int)data);
						if (magic == null)
						{
							return false;
						}
						outValue = magic.ReleaseRangeMin;
					}
					break;
				case DistanceValueOf.Value:
					break;
			}
			return true;
		}

		public TreeNode NodeRoot { private set; get; }

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

