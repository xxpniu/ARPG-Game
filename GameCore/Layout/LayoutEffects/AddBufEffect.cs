using System;
using Layout.EditorAttributes;
using Proto;

namespace Layout.LayoutEffects
{
	public enum DurationTimeValueOf
	{
		MagicConfig =0 ,
		DurationTime
	}

	[EditorEffect("添加持续效果(释放技能buf)")]
	public class AddBufEffect:EffectBase
	{
		public AddBufEffect ()
		{
		}

		[Label("buff表ID")]
		public int buffID;

		[Label("持续时间取值来源")]
        public GetValueFrom timeVauleOf;

		public override string ToString()
		{
            return string.Format("Buff:{0} 持续时间:{1}", buffID, timeVauleOf);
		}
	}
}

