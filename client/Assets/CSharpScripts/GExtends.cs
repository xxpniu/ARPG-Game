using System;
using UnityEngine;
using UnityEngine.UI;
using EngineCore;

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


        public static void SetText(this Button bt, string text)
        {
            var t =bt.transform.FindChild<Text>("Text");
            if (t == null)
                return;
            t.text = text;
        }


        public static GVector3 ToGVer3(this Proto.Vector3 v3)
        {
            return new GVector3(v3.x, v3.y, v3.z);
        }

        public static Layout.Vector3 ToLVer3(this Proto.Vector3 v3)
        {
            return new Layout.Vector3(v3.x, v3.y, v3.z);
        }

    }

}
