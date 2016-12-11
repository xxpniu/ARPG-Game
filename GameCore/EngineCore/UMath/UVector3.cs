using System;

namespace UMath
{
    /// <summary>
    /// vector3 of 3d
    /// </summary>
    [Serializable]
    public struct UVector3
    {
        #region fields
        /// <summary>
        /// The x,y,z
        /// </summary>
        public float x,y,z;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="UMath.UVector3"/> struct.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">The z coordinate.</param>
        public UVector3(float x,float y,float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        #region public

        /// <summary>
        /// Gets or sets the <see cref="UMath.UVector3"/> at the specified index.
        /// </summary>
        /// <param name="index">Index.</param>
        public float this[int index]{
            get
            { 
                switch (index)
                {
                    case 1:
                        return x;
                    case 2:
                        return y;
                    case 3:
                        return z;
                    default:
                        throw new Exception("Out of index must be 1-3");
                }
            }
            set{
                switch (index)
                {
                    case 1:
                        x = value;
                        break;
                    case 2:
                        y = value;
                        break;
                    case 3:
                        z = value;
                        break;
                    default:
                        throw new Exception("Out of index must be 1-3");
                }
            }
        }

        /// <summary>
        ///  the sqr magnitude.
        /// </summary>
        /// <value>The sqr magnitude.</value>
        public float sqrMagnitude
        {
            get
            {
                return  x * x + y * y + z * z;
            }
        }

        /// <summary>
        /// length of vector3
        /// </summary>
        /// <value>The magnitude.</value>
        public float magnitude
        {
            get{
                return (float)Math.Sqrt(sqrMagnitude);
            }
        }

        /// <summary>
        /// Gets the normalized.
        /// </summary>
        /// <value>The normalized.</value>
        public UVector3 normalized
        {
            get
            {
                return Normalize(this);
            }
        }

        /// <summary>
        /// Normalized this instance.
        /// </summary>
        public void Normalized()
        {
            var v = Normalize(this);
            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
        }

        #endregion

        #region Override

        public override bool Equals(object other)
        {
            if(other is UVector3)
                return this == (UVector3)other;
            return false;
        }
    
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("({0:0.0},{1:0.0},{2:0.0})", x, y, z);
        }
        #endregion

        #region static

        /// <summary>
        /// Up.(0,1,0)
        /// </summary>
        public static UVector3 up = new UVector3(0,1,0);
        /// <summary>
        /// Down.(0,-1,0)
        /// </summary>
        public static UVector3 down = new UVector3(0,-1,0);
        /// <summary>
        /// forward. (0,0,1)
        /// </summary>
        public static UVector3 forward = new UVector3(0,0,1);
        /// <summary>
        /// back (0,0,-1)
        /// </summary>
        public static UVector3 back = new UVector3(0,0,-1);
        /// <summary>
        /// left. (-1,0,0)
        /// </summary>
        public static UVector3 left = new UVector3(-1,0,0);
        /// <summary>
        /// right.(1,0,0)
        /// </summary>
        public static UVector3 right = new UVector3(1,0,0);
        /// <summary>
        ///  zero.(0,0,0)
        /// </summary>
        public static UVector3 zero = new UVector3(0,0,0);
        /// <summary>
        ///  one.(1,1,1)
        /// </summary>
        public static UVector3 one = new UVector3(1,1,1);

        /// <summary>
        /// Cross
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        public static UVector3 Cross(UVector3 left, UVector3 right)
        {
            var result = new UVector3(left.y * right.z - left.z * right.y,
                left.z * right.x - left.x * right.z,
                left.x * right.y - left.y * right.x);
            return result;
        }

        /// <summary>
        /// angle of two vector
        /// </summary>
        /// <param name="first">First.</param>
        /// <param name="second">Second.</param>
        public static float Angle(UVector3 first, UVector3 second)
        {
            return (float)System.Math.Acos((UVector3.Dot(first, second)) / (first.magnitude * second.magnitude))
                * MathHelper.Rad2Deg;
        }

        /// <summary>
        /// Normalize the specified vector.
        /// </summary>
        /// <param name="vec">Vec.</param>
        public static UVector3 Normalize(UVector3 vec)
        {
            if (vec.sqrMagnitude < MathHelper.Epsilon)
                return zero;
            
            float scale = 1.0f / vec.magnitude;
            vec.x *= scale;
            vec.y *= scale;
            vec.z *= scale;
            return vec;
        }

        /// <summary>
        /// Dot
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        public static float Dot(UVector3 left, UVector3 right)
        {
            return left.x * right.x + left.y * right.y + left.z * right.z;
        }

        /// <summary>
        /// Lerp.
        /// </summary>
        /// <param name="a">The alpha component.</param>
        /// <param name="b">The blue component.</param>
        /// <param name="blend">Blend.</param>
        public static UVector3 Lerp(UVector3 a, UVector3 b, float blend)
        {
            a.x = blend * (b.x - a.x) + a.x;
            a.y = blend * (b.y - a.y) + a.y;
            a.z = blend * (b.z - a.z) + a.z;
            return a;
        }

        /// <summary>
        /// Distance the specified l and r.
        /// </summary>
        /// <param name="l">L.</param>
        /// <param name="r">The red component.</param>
        public static float Distance(UVector3 l, UVector3 r)
        {
            return (l - r).magnitude;
        }

        #endregion

        #region Operators
        public static UVector3 operator +(UVector3 l, UVector3 r)
        {
            return new UVector3 { x = l.x + r.x, y = l.y + r.y, z = l.z + r.z };
        }

        public static UVector3 operator -(UVector3 l, UVector3 r)
        {
            return new UVector3 { x = l.x - r.x, y = l.y - r.y, z = l.z - r.z };
        }
        public static UVector3 operator -(UVector3 vec)
        {
            vec.x = -vec.x;
            vec.y = -vec.y;
            vec.z = -vec.z;
            return vec;
        }

        public static UVector3 operator /(UVector3 v, float f)
        {
            return new UVector3 { x = v.x / f, y = v.y / f, z = v.z / f };
        }

        public static UVector3 operator *(UVector3 v, float f)
        {
            return new UVector3 { x = v.x * f, y = v.y * f, z = v.z * f };
        }

        public static UVector3 operator *(float f, UVector3 v)
        {
            return new UVector3 { x = v.x * f, y = v.y * f, z = v.z * f };
        }

        public static float operator *(UVector3 l, UVector3 r)
        {
            return Dot(l, r);
        }

        public static bool operator ==(UVector3 l, UVector3 r)
        {
            return Math.Abs(l.x - r.x) < MathHelper.Epsilon 
                && Math.Abs(l.z - r.z) < MathHelper.Epsilon 
                && Math.Abs(l.y - r.y) < MathHelper.Epsilon;
        }

        public static bool operator !=(UVector3 l, UVector3 r)
        {
            return Math.Abs(l.x - r.x) > MathHelper.Epsilon 
                || Math.Abs(l.z - r.z) > MathHelper.Epsilon
                || Math.Abs(l.y - r.y) > MathHelper.Epsilon;
        }
        #endregion
    }
}

