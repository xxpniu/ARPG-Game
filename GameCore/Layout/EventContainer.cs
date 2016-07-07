using System;
using Layout.EditorAttributes;
using System.Collections.Generic;
using Layout.LayoutEffects;
using Layout.LayoutElements;
using System.Xml.Serialization;

namespace Layout
{

	/// <summary>
	/// 技能和buff使用一个处理，
	/// 相对来buff的trigger是多次的，如果这个buff设置了间隔时间的话
	/// 
	/// </summary>
	public enum EventType
	{
		EVENT_START,
		EVENT_TRIGGER,
		EVENT_ANIMATOR_TRIGGER ,
		EVENT_MISSILE_CREATE,
		EVENT_MISSILE_HIT,
		EVENT_MISSILE_DEAD,
		EVENT_UNIT_CREATE,
		EVENT_UNIT_HIT,
		EVENT_UNIT_DEAD,
		EVENT_END
		//EVENT_COMPLETED
	}

	public class EffectGroup
	{
		public EffectGroup()
		{
			effects = new List<EffectBase> ();
		}

		[EditorEffectsAttribute]
		public List<EffectBase> effects;
		[Label("描述")]
		public string Des;
		[Label("标记")]
		public string key;
	}

	public class EventContainer
	{
		public EventContainer ()
		{
			type = EventType.EVENT_START;
			effectGroup = new List<EffectGroup> ();
		}
		[Label("类型")]	
		public EventType type;
		[Label("事件相应Layout")]
		[LayoutPath]
		public string layoutPath;

		public List<EffectGroup> effectGroup;

		[XmlIgnore]
		public TimeLine line;

		public EffectGroup FindGroupByKey(string key)
		{
			foreach (var i in effectGroup) {
				if (i.key == key)
					return i;
			}

			return null;
		}

	}
}

