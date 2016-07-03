using System;
using Layout.EditorAttributes;

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
		EVENT_END,
		EVENT_COMPLETED
	}

	public class EventContainer
	{
		public EventContainer ()
		{
			type = EventType.EVENT_START;
		}

		public EventType type;

		[Label("事件相应Layout")]
		[EditorResourcePath]
		public string layoutPath;
	}
}

