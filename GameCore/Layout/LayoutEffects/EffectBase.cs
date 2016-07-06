using System;
using Layout.EditorAttributes;
using System.Xml.Serialization;

namespace Layout.LayoutEffects
{
	[
		XmlInclude(typeof(NormalDamageEffect))
	]
	public class EffectBase
	{
		public EffectBase ()
		{
		}
			
	}
}

