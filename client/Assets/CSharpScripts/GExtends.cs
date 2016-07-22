using System;
using UnityEngine;

namespace UGameTools
{
	public static class GExtends
	{
		public static T FindChild<T> (this Transform trans, string name) where T :Component
		{
			var t = FindInAllChild (trans, name);
			if (t == null)
				return null;
			else
				return t.GetComponent<T> ();
		}

		private static Transform FindInAllChild (Transform trans, string name)
		{
			if (trans.name == name) {
				return trans;
			}

			for (var i = 0; i < trans.childCount; i++) {
				var t = FindInAllChild (trans.GetChild (i), name);
				if (t != null)
					return t;
			}

			return null;
		}

		public static void ActiveSelfObject (this Component com, bool active)
		{
			com.gameObject.SetActive (active);
		}



	}

}
