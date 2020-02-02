using System;

namespace UMath
{
    [Serializable]
    public struct UVector4
    {
        #region fields
        public float x,y,z,w;
        #endregion

        public UVector4(float x,float y,float z,float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public UVector4(UVector3 v,float w):this(v.x,v.y,v.z,w)
        {
            
        }

        #region public 
        /// <summary>
        /// Gets or sets the value at the index of the Vector.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The component of the vector at index.</returns>
        public float this[int index]
        {
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
                    case 4:
                        return w;
                    default:
                        throw new ArgumentOutOfRangeException("You tried to access this vector at index: " + index);
                }
            }

            set
            {
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
                    case 4:
                        w = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("You tried to set this vector at index: " + index);
                }
            }
        }
        /// <summary>
        /// Gets the sqr magnitude.
        /// </summary>
        /// <value>The sqr magnitude.</value>
        public float sqrMagnitude
        {
            get{ return x* x + y * y + z * z + w * w; }
        }
        /// <summary>
        /// the magnitude/length of vector4
        /// </summary>
        /// <value>The magnitude.</value>
        public float magnitude
        {
            get
            {
                return (float)Math.Sqrt(sqrMagnitude);
            }
        }

        /// <summary>
        /// Gets or sets the xyz.
        /// </summary>
        /// <value>The xyz.</value>
        public UVector3 Xyz
        {

            get
            {
                return new UVector3(x, y, z);
            }
            set
            {
                x = value.x;
                y = value.y;
                z = value.z;
            }
        }

        #endregion

        #region Static 

        public static UVector4 zero = new UVector4(0,0,0,0);

        public static UVector4 Normalize(UVector4 vec)
        {
            if (vec.sqrMagnitude < MathHelper.Epsilon)
                return UVector4.zero;
            float scale = 1.0f / vec.magnitude;
            vec.x *= scale;
            vec.y *= scale;
            vec.z *= scale;
            vec.w *= scale;
            return vec;
        }

        #endregion
    }
}

