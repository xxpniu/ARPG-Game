using System;

namespace EngineCore
{
	public struct GVector3
	{
		public float x;
		public float y;
		public float z;

        private const float EPSILON =0.00001f;

		public GVector3(float x,float y,float z)
		{
			this.x = x;this.y = y;this.z = z;
		}

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

        public static bool operator ==(GVector3 l, GVector3 r)
        {
            return Math.Abs(l.x - r.x) < EPSILON && Math.Abs(l.z - r.z) < EPSILON && Math.Abs(l.y - r.y) < EPSILON;
        }

        public static bool operator !=(GVector3 l, GVector3 r)
        {
            return Math.Abs(l.x - r.x) > EPSILON || Math.Abs(l.z - r.z) > EPSILON || Math.Abs(l.y - r.y) > EPSILON;
        }
	}
}

