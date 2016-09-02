using System;

namespace EngineCore
{
    public struct GVector3 :IEquatable<GVector3>
    {
        public float x;
        public float y;
        public float z;

        public float X { get { return x; } }
        public float Y { get { return y; } }
        public float Z { get { return z; } }

       

        public GVector3(float x, float y, float z)
        {
            this.x = x; this.y = y; this.z = z;
        }


        public GVector3(GVector3 src)
        {
            this.x = src.x;
            this.y = src.y;
            this.z = src.z;
        }
        public GVector3(GVector4 src)
        {
            this.x = src.X;
            this.y = src.Y;
            this.z = src.Z;
        }

        public float LengthFast
        {
            get
            {
                return Length;
            }
        }

        public float Length
        {
            get
            {
                return (float)System.Math.Sqrt(X * X + Y * Y + Z * Z);
            }
        }

        public float LengthSquared
        {
            get
            {
                return x * x + y * y + z * z;
            }
        }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return X;
                    case 1:
                        return Y;
                    case 2:
                        return Z;
                    default:
                        throw new ArgumentOutOfRangeException("You tried to access this vector at index: " + index);
                }
            }

            set
            {
                switch (index)
                {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    case 2:
                        z = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("You tried to set this vector at index: " + index);
                }
            }
        }

        public GVector3 Normalized()
        {
            GVector3 v = this;
            v.Normalize();
            return v;
        }


        /// <summary>
        /// Scales the Vector3 to unit length.
        /// </summary>
        public void Normalize()
        {
            float scale = 1.0f / this.Length;
            x *= scale;
            y *= scale;
            z *= scale;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is GVector3))
                return false;

            return this.Equals((GVector3)obj);
        }


        public override string ToString()
        {
            return string.Format("({0},{1},{2})", x, y, z);
        }

        public static GVector3 Zero = new GVector3(0, 0, 0);

        public static float Distance(GVector3 position1, GVector3 position2)
        {
            return (position2 - position1).Length;
        }

        /// <summary>
        /// Defines a unit-length Vector3 that points towards the X-axis.
        /// </summary>
        public static readonly GVector3 UnitX = new GVector3(1, 0, 0);

        /// <summary>
        /// Defines a unit-length Vector3 that points towards the Y-axis.
        /// </summary>
        public static readonly GVector3 UnitY = new GVector3(0, 1, 0);

        /// <summary>
        /// /// Defines a unit-length Vector3 that points towards the Z-axis.
        /// </summary>
        public static readonly GVector3 UnitZ = new GVector3(0, 0, 1);

       
        /// <summary>
        /// Defines an instance with all components set to 1.
        /// </summary>
        public static readonly GVector3 One = new GVector3(1, 1, 1);

        public static GVector3 Normalize(GVector3 vec)
        {
            float scale = 1.0f / vec.Length;
            vec.x *= scale;
            vec.y *= scale;
            vec.z *= scale;
            return vec;
        }

        public static GVector3 Lerp(GVector3 a, GVector3 b, float blend)
        {
            a.x = blend * (b.X - a.X) + a.X;
            a.y = blend * (b.Y - a.Y) + a.Y;
            a.z = blend * (b.Z - a.Z) + a.Z;
            return a;
        }

        public static float Dot(GVector3 left, GVector3 right)
        {
            return left.X * right.X + left.Y * right.Y + left.Z * right.Z;
        }

        public static GVector3 TransformVector(GVector3 vec, Matrix4 mat)
        {
            GVector3 v;
            v.x = vec * new GVector3(mat.Column0);
            v.y = vec * new GVector3(mat.Column1);
            v.z = vec * new GVector3(mat.Column2);
            return v;
        }

        public static float CalculateAngle(GVector3 first, GVector3 second)
        {
            return (float)System.Math.Acos((GVector3.Dot(first, second)) / (first.Length * second.Length));
        }
        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Result of operation.</returns>
        public static GVector3 Add(GVector3 a, GVector3 b)
        {
            Add(ref a, ref b, out a);
            return a;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <param name="result">Result of operation.</param>
        public static void Add(ref GVector3 a, ref GVector3 b, out GVector3 result)
        {
            result = new GVector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }


        /// <summary>
        /// Caclulate the cross (vector) product of two vectors
        /// </summary>
        /// <param name="left">First operand</param>
        /// <param name="right">Second operand</param>
        /// <returns>The cross product of the two inputs</returns>
        public static GVector3 Cross(GVector3 left, GVector3 right)
        {
            GVector3 result;
            Cross(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Caclulate the cross (vector) product of two vectors
        /// </summary>
        /// <param name="left">First operand</param>
        /// <param name="right">Second operand</param>
        /// <returns>The cross product of the two inputs</returns>
        /// <param name="result">The cross product of the two inputs</param>
        public static void Cross(ref GVector3 left, ref GVector3 right, out GVector3 result)
        {
            result = new GVector3(left.Y * right.Z - left.Z * right.Y,
                left.Z * right.X - left.X * right.Z,
                left.X * right.Y - left.Y * right.X);
        }

        /// <summary>
        /// Multiplies a vector by a scalar.
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <returns>Result of the operation.</returns>
        public static GVector3 Multiply(GVector3 vector, float scale)
        {
            Multiply(ref vector, scale, out vector);
            return vector;
        }

        /// <summary>
        /// Multiplies a vector by a scalar.
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <param name="result">Result of the operation.</param>
        public static void Multiply(ref GVector3 vector, float scale, out GVector3 result)
        {
            result = new GVector3(vector.X * scale, vector.Y * scale, vector.Z * scale);
        }
        /// <summary>
        /// Transforms a vector by a quaternion rotation.
        /// </summary>
        /// <param name="vec">The vector to transform.</param>
        /// <param name="quat">The quaternion to rotate the vector by.</param>
        /// <returns>The result of the operation.</returns>
        public static GVector3 Transform(GVector3 vec, GQuaternion quat)
        {
            GVector3 result;
            Transform(ref vec, ref quat, out result);
            return result;
        }

        /// <summary>
        /// Transforms a vector by a quaternion rotation.
        /// </summary>
        /// <param name="vec">The vector to transform.</param>
        /// <param name="quat">The quaternion to rotate the vector by.</param>
        /// <param name="result">The result of the operation.</param>
        public static void Transform(ref GVector3 vec, ref GQuaternion quat, out GVector3 result)
        {
            // Since vec.W == 0, we can optimize quat * vec * quat^-1 as follows:
            // vec + 2.0 * cross(quat.xyz, cross(quat.xyz, vec) + quat.w * vec)
            GVector3 xyz = quat.Xyz, temp, temp2;
            GVector3.Cross(ref xyz, ref vec, out temp);
            GVector3.Multiply(ref vec, quat.W, out temp2);
            GVector3.Add(ref temp, ref temp2, out temp);
            GVector3.Cross(ref xyz, ref temp, out temp);
            GVector3.Multiply(ref temp, 2, out temp);
            GVector3.Add(ref vec, ref temp, out result);
        }

        /// <summary>Transform a Vector3 by the given Matrix, and project the resulting Vector4 back to a Vector3</summary>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <returns>The transformed vector</returns>
        public static GVector3 TransformPerspective(GVector3 vec, Matrix4 mat)
        {
            GVector3 result;
            TransformPerspective(ref vec, ref mat, out result);
            return result;
        }

        /// <summary>Transform a Vector3 by the given Matrix, and project the resulting Vector4 back to a Vector3</summary>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <param name="result">The transformed vector</param>
        public static void TransformPerspective(ref GVector3 vec, ref Matrix4 mat, out GVector3 result)
        {
            GVector4 v = new GVector4(vec, 1);
            GVector4.Transform(ref v, ref mat, out v);
            result.x = v.X / v.W;
            result.y = v.Y / v.W;
            result.z = v.Z / v.W;
        }

        public bool Equals(GVector3 other)
        {
            return this == other;
        }
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        public static GVector3 operator +(GVector3 l, GVector3 r)
        {
            return new GVector3 { x = l.x + r.x, y = l.y + r.y, z = l.z + r.z };
        }

        public static GVector3 operator -(GVector3 l, GVector3 r)
        {
            return new GVector3 { x = l.x - r.x, y = l.y - r.y, z = l.z - r.z };
        }
        public static GVector3 operator -(GVector3 vec)
        {
            vec.x = -vec.X;
            vec.y = -vec.Y;
            vec.z = -vec.Z;
            return vec;
        }

        public static GVector3 operator /(GVector3 v, float f)
        {
            return new GVector3 { x = v.x / f, y = v.y / f, z = v.z / f };
        }

        public static GVector3 operator *(GVector3 v, float f)
        {
            return new GVector3 { x = v.x * f, y = v.y * f, z = v.z * f };
        }

        public static GVector3 operator *(float f, GVector3 v)
        {
            return new GVector3 { x = v.x * f, y = v.y * f, z = v.z * f };
        }

        public static float operator *(GVector3 l, GVector3 r)
        {
            return Dot(l, r);
        }

        public static bool operator ==(GVector3 l, GVector3 r)
        {
            return Math.Abs(l.x - r.x) < MathHelper. EPSILON 
                       && Math.Abs(l.z - r.z) < MathHelper.EPSILON 
                       && Math.Abs(l.y - r.y) < MathHelper.EPSILON;
        }

        public static bool operator !=(GVector3 l, GVector3 r)
        {
            return Math.Abs(l.x - r.x) > MathHelper.EPSILON 
                       || Math.Abs(l.z - r.z) > MathHelper.EPSILON
                       || Math.Abs(l.y - r.y) > MathHelper.EPSILON;
        }
	}
}

