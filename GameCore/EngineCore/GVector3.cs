using System;

namespace EngineCore
{
	public struct GVector3
	{
		public float x;
		public float y;
		public float z;

		public GVector3(float x,float y,float z)
		{
			this.x = x;this.y = y;this.z = z;
		}

		public GVector3():this(0,0,0){}

		public override string ToString ()
		{
			return string.Format ("({0},{1},{2})",x,y,z);
		}

		public static GVector3 operator +(GVector3 l, GVector3 r)
		{
			return new GVector3{ x = l.x +r.x , y = l.y +r.y, z = l.z +r.z};
		}

		public static GVector3 operator -(GVector3 l,GVector3 r)
		{
			return new GVector3{ x = l.x -r.x , y = l.y -r.y, z = l.z -r.z};
		}

		public static GVector3 operator *(GVector3 v, float f)
		{
			return new GVector3{ x = v.x*f , y = v.y*f, z = v.z*f};
		}
	}
}

