using System;
using Layout.EditorAttributes;
using System.Xml.Serialization;

namespace Layout.LayoutElements
{
	[
		XmlInclude(typeof(MissileLayout)),
		XmlInclude(typeof(MotionLayout)),
		XmlInclude(typeof(DamageLayout)),
		XmlInclude(typeof(ParticleLayout)),
		XmlInclude(typeof(LookAtTarget)),
        XmlInclude(typeof(CallUnitLayout))
	]
	public class LayoutBase
	{
		public LayoutBase ()
		{
			
		}

		[HideInEditor]
		public string GUID;

		public static T CreateInstance<T> ()where T: LayoutBase, new()
		{
			var t= new T ();
			t.GUID = Guid.NewGuid ().ToString ();
			return t;
		}

		public static LayoutBase CreateInstance(Type t)
		{
			var instance = Activator.CreateInstance (t) as LayoutBase;
			instance.GUID = Guid.NewGuid ().ToString ();
			return instance;
		}
	}
}

