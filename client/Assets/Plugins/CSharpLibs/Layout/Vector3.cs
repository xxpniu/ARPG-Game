using System;

namespace Layout
{
	public class Vector3
	{
		public Vector3 (float x, float y, float z)
		{
			this.x = x;this.y = y;this.z = z;
		}

		public Vector3() :this(0,0,0) { }

		public float x;
		public float y;
		public float z;
	}
}

