using System;
using Layout.EditorAttributes;

namespace Layout.LayoutElements
{
	public class LayoutBase
	{
		public LayoutBase ()
		{
			
		}

		[Label("ID")]
		public string GUID;

		public static T CreateInstance<T> ()where T: LayoutBase, new()
		{
			var t= new T ();
			t.GUID = Guid.NewGuid ().ToString ();
			return t;
		}
	}
}

