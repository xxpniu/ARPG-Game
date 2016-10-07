using System;
using Layout.EditorAttributes;
using System.Collections.Generic;

namespace Layout
{
	public class MagicData
	{
		public MagicData ()
		{
			Containers = new List<EventContainer> ();
		}

		[Label("查询Key")]
		public string key;

		[Label("名称")]
		public string name;

		//[Label("事件")]
		public List<EventContainer> Containers;

	}
}

