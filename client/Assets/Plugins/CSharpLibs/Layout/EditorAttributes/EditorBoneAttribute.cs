using System;

namespace Layout.EditorAttributes
{
	[AttributeUsage(AttributeTargets.Field)]
	//用来给编辑器显示骨骼属性
	public class EditorBoneAttribute:Attribute
	{
		public EditorBoneAttribute ()
		{
			
		}
	}
}

