using System;
namespace EngineCore
{
    public struct GVector4 : IEquatable<GVector4>
    {


        #region Fields

        /// <summary>
        /// The X component of the Vector4.
        /// </summary>
        public float X;

        /// <summary>
        /// The Y component of the Vector4.
        /// </summary>
        public float Y;

        /// <summary>
        /// The Z component of the Vector4.
        /// </summary>
        public float Z;

        /// <summary>
        /// The W component of the Vector4.
        /// </summary>
        public float W;

        /// <summary>
        /// Defines a unit-length Vector4 that points towards the X-axis.
        /// </summary>
        public static readonly GVector4 UnitX = new GVector4(1, 0, 0, 0);

        /// <summary>
        /// Defines a unit-length Vector4 that points towards the Y-axis.
        /// </summary>
        public static readonly GVector4 UnitY = new GVector4(0, 1, 0, 0);

        /// <summary>
        /// Defines a unit-length Vector4 that points towards the Z-axis.
        /// </summary>
        public static readonly GVector4 UnitZ = new GVector4(0, 0, 1, 0);

        /// <summary>
        /// Defines a unit-length Vector4 that points towards the W-axis.
        /// </summary>
        public static readonly GVector4 UnitW = new GVector4(0, 0, 0, 1);

        /// <summary>
        /// Defines a zero-length Vector4.
        /// </summary>
        public static readonly GVector4 Zero = new GVector4(0, 0, 0, 0);

        /// <summary>
        /// Defines an instance with all components set to 1.
        /// </summary>
        public static readonly GVector4 One = new GVector4(1, 1, 1, 1);


        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        /// <param name="value">The value that will initialize this instance.</param>
        public GVector4(float value)
        {
            X = value;
            Y = value;
            Z = value;
            W = value;
        }

        /// <summary>
        /// Constructs a new Vector4.
        /// </summary>
        /// <param name="x">The x component of the Vector4.</param>
        /// <param name="y">The y component of the Vector4.</param>
        /// <param name="z">The z component of the Vector4.</param>
        /// <param name="w">The w component of the Vector4.</param>
        public GVector4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }



        /// <summary>
        /// Constructs a new Vector4 from the given GVector3.
        /// The w component is initialized to 0.
        /// </summary>
        /// <param name="v">The Vector3 to copy components from.</param>
        /// <remarks><seealso cref="Vector4(Vector3, float)"/></remarks>
        public GVector4(GVector3 v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
            W = 0.0f;
        }

        /// <summary>
        /// Constructs a new Vector4 from the specified Vector3 and w component.
        /// </summary>
        /// <param name="v">The Vector3 to copy components from.</param>
        /// <param name="w">The w component of the new Vector4.</param>
        public GVector4(GVector3 v, float w)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
            W = w;
        }

        /// <summary>
        /// Constructs a new Vector4 from the given Vector4.
        /// </summary>
        /// <param name="v">The Vector4 to copy components from.</param>
        public GVector4(GVector4 v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
            W = v.W;
        }

        #endregion

        #region Public Members

        #region Indexer

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
                    case 0:
                        return X;
                    case 1:
                        return Y;
                    case 2:
                        return Z;
                    case 3:
                        return W;
                    default:
                        throw new ArgumentOutOfRangeException("You tried to access this vector at index: " + index);
                }
            }

            set
            {
                switch (index)
                {
                    case 0:
                        X = value;
                        break;
                    case 1:
                        Y = value;
                        break;
                    case 2:
                        Z = value;
                        break;
                    case 3:
                        W = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("You tried to set this vector at index: " + index);
                }
            }
        }

        #endregion

        #region Instance

        #region public void Add()

        /// <summary>Add the Vector passed as parameter to this instance.</summary>
        /// <param name="right">Right operand. This parameter is only read from.</param>
        [Obsolete("Use static Add() method instead.")]
        public void Add(GVector4 right)
        {
            this.X += right.X;
            this.Y += right.Y;
            this.Z += right.Z;
            this.W += right.W;
        }

        /// <summary>Add the Vector passed as parameter to this instance.</summary>
        /// <param name="right">Right operand. This parameter is only read from.</param>
        //[CLSCompliant(false)]
        [Obsolete("Use static Add() method instead.")]
        public void Add(ref GVector4 right)
        {
            this.X += right.X;
            this.Y += right.Y;
            this.Z += right.Z;
            this.W += right.W;
        }

        #endregion public void Add()

        #region public void Sub()

        /// <summary>Subtract the Vector passed as parameter from this instance.</summary>
        /// <param name="right">Right operand. This parameter is only read from.</param>
        [Obsolete("Use static Subtract() method instead.")]
        public void Sub(GVector4 right)
        {
            this.X -= right.X;
            this.Y -= right.Y;
            this.Z -= right.Z;
            this.W -= right.W;
        }

        /// <summary>Subtract the Vector passed as parameter from this instance.</summary>
        /// <param name="right">Right operand. This parameter is only read from.</param>
        //[CLSCompliant(false)]
        [Obsolete("Use static Subtract() method instead.")]
        public void Sub(ref GVector4 right)
        {
            this.X -= right.X;
            this.Y -= right.Y;
            this.Z -= right.Z;
            this.W -= right.W;
        }

        #endregion public void Sub()

        #region public void Mult()

        /// <summary>Multiply this instance by a scalar.</summary>
        /// <param name="f">Scalar operand.</param>
        [Obsolete("Use static Multiply() method instead.")]
        public void Mult(float f)
        {
            this.X *= f;
            this.Y *= f;
            this.Z *= f;
            this.W *= f;
        }

        #endregion public void Mult()

        #region public void Div()

        /// <summary>Divide this instance by a scalar.</summary>
        /// <param name="f">Scalar operand.</param>
        [Obsolete("Use static Divide() method instead.")]
        public void Div(float f)
        {
            float mult = 1.0f / f;
            this.X *= mult;
            this.Y *= mult;
            this.Z *= mult;
            this.W *= mult;
        }

        #endregion public void Div()

        #region public float Length

        /// <summary>
        /// Gets the length (magnitude) of the vector.
        /// </summary>
        /// <see cref="LengthFast"/>
        /// <seealso cref="LengthSquared"/>
        public float Length
        {
            get
            {
                return (float)System.Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
            }
        }

        #endregion

        #region public float LengthFast

        /// <summary>
        /// Gets an approximation of the vector length (magnitude).
        /// </summary>
        /// <remarks>
        /// This property uses an approximation of the square root function to calculate vector magnitude, with
        /// an upper error bound of 0.001.
        /// </remarks>
        /// <see cref="Length"/>
        /// <seealso cref="LengthSquared"/>
        public float LengthFast
        {
            get
            {
                return Length;
            }
        }

        #endregion

        #region public float LengthSquared

        /// <summary>
        /// Gets the square of the vector length (magnitude).
        /// </summary>
        /// <remarks>
        /// This property avoids the costly square root operation required by the Length property. This makes it more suitable
        /// for comparisons.
        /// </remarks>
        /// <see cref="Length"/>
        /// <seealso cref="LengthFast"/>
        public float LengthSquared
        {
            get
            {
                return X * X + Y * Y + Z * Z + W * W;
            }
        }

        #endregion

        /// <summary>
        /// Returns a copy of the Vector4 scaled to unit length.
        /// </summary>
        public GVector4 Normalized()
        {
            GVector4 v = this;
            v.Normalize();
            return v;
        }

        #region public void Normalize()

        /// <summary>
        /// Scales the Vector4 to unit length.
        /// </summary>
        public void Normalize()
        {
            float scale = 1.0f / this.Length;
            X *= scale;
            Y *= scale;
            Z *= scale;
            W *= scale;
        }

        #endregion



        #region public void Scale()

        /// <summary>
        /// Scales the current Vector4 by the given amounts.
        /// </summary>
        /// <param name="sx">The scale of the X component.</param>
        /// <param name="sy">The scale of the Y component.</param>
        /// <param name="sz">The scale of the Z component.</param>
        /// <param name="sw">The scale of the Z component.</param>
        [Obsolete("Use static Multiply() method instead.")]
        public void Scale(float sx, float sy, float sz, float sw)
        {
            this.X = X * sx;
            this.Y = Y * sy;
            this.Z = Z * sz;
            this.W = W * sw;
        }

        /// <summary>Scales this instance by the given parameter.</summary>
        /// <param name="scale">The scaling of the individual components.</param>
        [Obsolete("Use static Multiply() method instead.")]
        public void Scale(GVector4 scale)
        {
            this.X *= scale.X;
            this.Y *= scale.Y;
            this.Z *= scale.Z;
            this.W *= scale.W;
        }

        /// <summary>Scales this instance by the given parameter.</summary>
        /// <param name="scale">The scaling of the individual components.</param>
        //[CLSCompliant(false)]
        [Obsolete("Use static Multiply() method instead.")]
        public void Scale(ref GVector4 scale)
        {
            this.X *= scale.X;
            this.Y *= scale.Y;
            this.Z *= scale.Z;
            this.W *= scale.W;
        }

        #endregion public void Scale()

        #endregion

        #region Static

        #region Obsolete

        #region Sub

        /// <summary>
        /// Subtract one Vector from another
        /// </summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <returns>Result of subtraction</returns>
        public static GVector4 Sub(GVector4 a, GVector4 b)
        {
            a.X -= b.X;
            a.Y -= b.Y;
            a.Z -= b.Z;
            a.W -= b.W;
            return a;
        }

        /// <summary>
        /// Subtract one Vector from another
        /// </summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <param name="result">Result of subtraction</param>
        public static void Sub(ref GVector4 a, ref GVector4 b, out GVector4 result)
        {
            result.X = a.X - b.X;
            result.Y = a.Y - b.Y;
            result.Z = a.Z - b.Z;
            result.W = a.W - b.W;
        }

        #endregion

        #region Mult

        /// <summary>
        /// Multiply a vector and a scalar
        /// </summary>
        /// <param name="a">Vector operand</param>
        /// <param name="f">Scalar operand</param>
        /// <returns>Result of the multiplication</returns>
        public static GVector4 Mult(GVector4 a, float f)
        {
            a.X *= f;
            a.Y *= f;
            a.Z *= f;
            a.W *= f;
            return a;
        }

        /// <summary>
        /// Multiply a vector and a scalar
        /// </summary>
        /// <param name="a">Vector operand</param>
        /// <param name="f">Scalar operand</param>
        /// <param name="result">Result of the multiplication</param>
        public static void Mult(ref GVector4 a, float f, out GVector4 result)
        {
            result.X = a.X * f;
            result.Y = a.Y * f;
            result.Z = a.Z * f;
            result.W = a.W * f;
        }

        #endregion

        #region Div

        /// <summary>
        /// Divide a vector by a scalar
        /// </summary>
        /// <param name="a">Vector operand</param>
        /// <param name="f">Scalar operand</param>
        /// <returns>Result of the division</returns>
        public static GVector4 Div(GVector4 a, float f)
        {
            float mult = 1.0f / f;
            a.X *= mult;
            a.Y *= mult;
            a.Z *= mult;
            a.W *= mult;
            return a;
        }

        /// <summary>
        /// Divide a vector by a scalar
        /// </summary>
        /// <param name="a">Vector operand</param>
        /// <param name="f">Scalar operand</param>
        /// <param name="result">Result of the division</param>
        public static void Div(ref GVector4 a, float f, out GVector4 result)
        {
            float mult = 1.0f / f;
            result.X = a.X * mult;
            result.Y = a.Y * mult;
            result.Z = a.Z * mult;
            result.W = a.W * mult;
        }

        #endregion

        #endregion

        #region Add

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Result of operation.</returns>
        public static GVector4 Add(GVector4 a, GVector4 b)
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
        public static void Add(ref GVector4 a, ref GVector4 b, out GVector4 result)
        {
            result = new GVector4(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
        }

        #endregion

        #region Subtract

        /// <summary>
        /// Subtract one Vector from another
        /// </summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <returns>Result of subtraction</returns>
        public static GVector4 Subtract(GVector4 a, GVector4 b)
        {
            Subtract(ref a, ref b, out a);
            return a;
        }

        /// <summary>
        /// Subtract one Vector from another
        /// </summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <param name="result">Result of subtraction</param>
        public static void Subtract(ref GVector4 a, ref GVector4 b, out GVector4 result)
        {
            result = new GVector4(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
        }

        #endregion

        #region Multiply

        /// <summary>
        /// Multiplies a vector by a scalar.
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <returns>Result of the operation.</returns>
        public static GVector4 Multiply(GVector4 vector, float scale)
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
        public static void Multiply(ref GVector4 vector, float scale, out GVector4 result)
        {
            result = new GVector4(vector.X * scale, vector.Y * scale, vector.Z * scale, vector.W * scale);
        }

        /// <summary>
        /// Multiplies a vector by the components a vector (scale).
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <returns>Result of the operation.</returns>
        public static GVector4 Multiply(GVector4 vector, GVector4 scale)
        {
            Multiply(ref vector, ref scale, out vector);
            return vector;
        }

        /// <summary>
        /// Multiplies a vector by the components of a vector (scale).
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <param name="result">Result of the operation.</param>
        public static void Multiply(ref GVector4 vector, ref GVector4 scale, out GVector4 result)
        {
            result = new GVector4(vector.X * scale.X, vector.Y * scale.Y, vector.Z * scale.Z, vector.W * scale.W);
        }

        #endregion

        #region Divide

        /// <summary>
        /// Divides a vector by a scalar.
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <returns>Result of the operation.</returns>
        public static GVector4 Divide(GVector4 vector, float scale)
        {
            Divide(ref vector, scale, out vector);
            return vector;
        }

        /// <summary>
        /// Divides a vector by a scalar.
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <param name="result">Result of the operation.</param>
        public static void Divide(ref GVector4 vector, float scale, out GVector4 result)
        {
            Multiply(ref vector, 1 / scale, out result);
        }

        /// <summary>
        /// Divides a vector by the components of a vector (scale).
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <returns>Result of the operation.</returns>
        public static GVector4 Divide(GVector4 vector, GVector4 scale)
        {
            Divide(ref vector, ref scale, out vector);
            return vector;
        }

        /// <summary>
        /// Divide a vector by the components of a vector (scale).
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <param name="result">Result of the operation.</param>
        public static void Divide(ref GVector4 vector, ref GVector4 scale, out GVector4 result)
        {
            result = new GVector4(vector.X / scale.X, vector.Y / scale.Y, vector.Z / scale.Z, vector.W / scale.W);
        }

        #endregion

        #region Min

        /// <summary>
        /// Calculate the component-wise minimum of two vectors
        /// </summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <returns>The component-wise minimum</returns>
        public static GVector4 Min(GVector4 a, GVector4 b)
        {
            a.X = a.X < b.X ? a.X : b.X;
            a.Y = a.Y < b.Y ? a.Y : b.Y;
            a.Z = a.Z < b.Z ? a.Z : b.Z;
            a.W = a.W < b.W ? a.W : b.W;
            return a;
        }

        /// <summary>
        /// Calculate the component-wise minimum of two vectors
        /// </summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <param name="result">The component-wise minimum</param>
        public static void Min(ref GVector4 a, ref GVector4 b, out GVector4 result)
        {
            result.X = a.X < b.X ? a.X : b.X;
            result.Y = a.Y < b.Y ? a.Y : b.Y;
            result.Z = a.Z < b.Z ? a.Z : b.Z;
            result.W = a.W < b.W ? a.W : b.W;
        }

        #endregion

        #region Max

        /// <summary>
        /// Calculate the component-wise maximum of two vectors
        /// </summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <returns>The component-wise maximum</returns>
        public static GVector4 Max(GVector4 a, GVector4 b)
        {
            a.X = a.X > b.X ? a.X : b.X;
            a.Y = a.Y > b.Y ? a.Y : b.Y;
            a.Z = a.Z > b.Z ? a.Z : b.Z;
            a.W = a.W > b.W ? a.W : b.W;
            return a;
        }

        /// <summary>
        /// Calculate the component-wise maximum of two vectors
        /// </summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <param name="result">The component-wise maximum</param>
        public static void Max(ref GVector4 a, ref GVector4 b, out GVector4 result)
        {
            result.X = a.X > b.X ? a.X : b.X;
            result.Y = a.Y > b.Y ? a.Y : b.Y;
            result.Z = a.Z > b.Z ? a.Z : b.Z;
            result.W = a.W > b.W ? a.W : b.W;
        }

        #endregion

        #region Clamp

        /// <summary>
        /// Clamp a vector to the given minimum and maximum vectors
        /// </summary>
        /// <param name="vec">Input vector</param>
        /// <param name="min">Minimum vector</param>
        /// <param name="max">Maximum vector</param>
        /// <returns>The clamped vector</returns>
        public static GVector4 Clamp(GVector4 vec, GVector4 min, GVector4 max)
        {
            vec.X = vec.X < min.X ? min.X : vec.X > max.X ? max.X : vec.X;
            vec.Y = vec.Y < min.Y ? min.Y : vec.Y > max.Y ? max.Y : vec.Y;
            vec.Z = vec.X < min.Z ? min.Z : vec.Z > max.Z ? max.Z : vec.Z;
            vec.W = vec.Y < min.W ? min.W : vec.W > max.W ? max.W : vec.W;
            return vec;
        }

        /// <summary>
        /// Clamp a vector to the given minimum and maximum vectors
        /// </summary>
        /// <param name="vec">Input vector</param>
        /// <param name="min">Minimum vector</param>
        /// <param name="max">Maximum vector</param>
        /// <param name="result">The clamped vector</param>
        public static void Clamp(ref GVector4 vec, ref GVector4 min, ref GVector4 max, out GVector4 result)
        {
            result.X = vec.X < min.X ? min.X : vec.X > max.X ? max.X : vec.X;
            result.Y = vec.Y < min.Y ? min.Y : vec.Y > max.Y ? max.Y : vec.Y;
            result.Z = vec.X < min.Z ? min.Z : vec.Z > max.Z ? max.Z : vec.Z;
            result.W = vec.Y < min.W ? min.W : vec.W > max.W ? max.W : vec.W;
        }

        #endregion

        #region Normalize

        /// <summary>
        /// Scale a vector to unit length
        /// </summary>
        /// <param name="vec">The input vector</param>
        /// <returns>The normalized vector</returns>
        public static GVector4 Normalize(GVector4 vec)
        {
            float scale = 1.0f / vec.Length;
            vec.X *= scale;
            vec.Y *= scale;
            vec.Z *= scale;
            vec.W *= scale;
            return vec;
        }

        /// <summary>
        /// Scale a vector to unit length
        /// </summary>
        /// <param name="vec">The input vector</param>
        /// <param name="result">The normalized vector</param>
        public static void Normalize(ref GVector4 vec, out GVector4 result)
        {
            float scale = 1.0f / vec.Length;
            result.X = vec.X * scale;
            result.Y = vec.Y * scale;
            result.Z = vec.Z * scale;
            result.W = vec.W * scale;
        }

        #endregion



        #region Dot

        /// <summary>
        /// Calculate the dot product of two vectors
        /// </summary>
        /// <param name="left">First operand</param>
        /// <param name="right">Second operand</param>
        /// <returns>The dot product of the two inputs</returns>
        public static float Dot(GVector4 left, GVector4 right)
        {
            return left.X * right.X + left.Y * right.Y + left.Z * right.Z + left.W * right.W;
        }

        /// <summary>
        /// Calculate the dot product of two vectors
        /// </summary>
        /// <param name="left">First operand</param>
        /// <param name="right">Second operand</param>
        /// <param name="result">The dot product of the two inputs</param>
        public static void Dot(ref GVector4 left, ref GVector4 right, out float result)
        {
            result = left.X * right.X + left.Y * right.Y + left.Z * right.Z + left.W * right.W;
        }

        #endregion

        #region Lerp

        /// <summary>
        /// Returns a new Vector that is the linear blend of the 2 given Vectors
        /// </summary>
        /// <param name="a">First input vector</param>
        /// <param name="b">Second input vector</param>
        /// <param name="blend">The blend factor. a when blend=0, b when blend=1.</param>
        /// <returns>a when blend=0, b when blend=1, and a linear combination otherwise</returns>
        public static GVector4 Lerp(GVector4 a, GVector4 b, float blend)
        {
            a.X = blend * (b.X - a.X) + a.X;
            a.Y = blend * (b.Y - a.Y) + a.Y;
            a.Z = blend * (b.Z - a.Z) + a.Z;
            a.W = blend * (b.W - a.W) + a.W;
            return a;
        }

        /// <summary>
        /// Returns a new Vector that is the linear blend of the 2 given Vectors
        /// </summary>
        /// <param name="a">First input vector</param>
        /// <param name="b">Second input vector</param>
        /// <param name="blend">The blend factor. a when blend=0, b when blend=1.</param>
        /// <param name="result">a when blend=0, b when blend=1, and a linear combination otherwise</param>
        public static void Lerp(ref GVector4 a, ref GVector4 b, float blend, out GVector4 result)
        {
            result.X = blend * (b.X - a.X) + a.X;
            result.Y = blend * (b.Y - a.Y) + a.Y;
            result.Z = blend * (b.Z - a.Z) + a.Z;
            result.W = blend * (b.W - a.W) + a.W;
        }

        #endregion

        #region Barycentric

        /// <summary>
        /// Interpolate 3 Vectors using Barycentric coordinates
        /// </summary>
        /// <param name="a">First input Vector</param>
        /// <param name="b">Second input Vector</param>
        /// <param name="c">Third input Vector</param>
        /// <param name="u">First Barycentric Coordinate</param>
        /// <param name="v">Second Barycentric Coordinate</param>
        /// <returns>a when u=v=0, b when u=1,v=0, c when u=0,v=1, and a linear combination of a,b,c otherwise</returns>
        public static GVector4 BaryCentric(GVector4 a, GVector4 b, GVector4 c, float u, float v)
        {
            return a + u * (b - a) + v * (c - a);
        }

        /// <summary>Interpolate 3 Vectors using Barycentric coordinates</summary>
        /// <param name="a">First input Vector.</param>
        /// <param name="b">Second input Vector.</param>
        /// <param name="c">Third input Vector.</param>
        /// <param name="u">First Barycentric Coordinate.</param>
        /// <param name="v">Second Barycentric Coordinate.</param>
        /// <param name="result">Output Vector. a when u=v=0, b when u=1,v=0, c when u=0,v=1, and a linear combination of a,b,c otherwise</param>
        public static void BaryCentric(ref GVector4 a, ref GVector4 b, ref GVector4 c, float u, float v, out GVector4 result)
        {
            result = a; // copy

            GVector4 temp = b; // copy
            Subtract(ref temp, ref a, out temp);
            Multiply(ref temp, u, out temp);
            Add(ref result, ref temp, out result);

            temp = c; // copy
            Subtract(ref temp, ref a, out temp);
            Multiply(ref temp, v, out temp);
            Add(ref result, ref temp, out result);
        }

        #endregion

        #region Transform

        /// <summary>Transform a Vector by the given Matrix</summary>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <returns>The transformed vector</returns>
        public static GVector4 Transform(GVector4 vec, Matrix4 mat)
        {
            GVector4 result;
            Transform(ref vec, ref mat, out result);
            return result;
        }

        /// <summary>Transform a Vector by the given Matrix</summary>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <param name="result">The transformed vector</param>
        public static void Transform(ref GVector4 vec, ref Matrix4 mat, out GVector4 result)
        {
            result = new GVector4(
                vec.X * mat.Row0.X + vec.Y * mat.Row1.X + vec.Z * mat.Row2.X + vec.W * mat.Row3.X,
                vec.X * mat.Row0.Y + vec.Y * mat.Row1.Y + vec.Z * mat.Row2.Y + vec.W * mat.Row3.Y,
                vec.X * mat.Row0.Z + vec.Y * mat.Row1.Z + vec.Z * mat.Row2.Z + vec.W * mat.Row3.Z,
                vec.X * mat.Row0.W + vec.Y * mat.Row1.W + vec.Z * mat.Row2.W + vec.W * mat.Row3.W);
        }

        /// <summary>
        /// Transforms a vector by a quaternion rotation.
        /// </summary>
        /// <param name="vec">The vector to transform.</param>
        /// <param name="quat">The quaternion to rotate the vector by.</param>
        /// <returns>The result of the operation.</returns>
        public static GVector4 Transform(GVector4 vec, Quaternion quat)
        {
            GVector4 result;
            Transform(ref vec, ref quat, out result);
            return result;
        }

        /// <summary>
        /// Transforms a vector by a quaternion rotation.
        /// </summary>
        /// <param name="vec">The vector to transform.</param>
        /// <param name="quat">The quaternion to rotate the vector by.</param>
        /// <param name="result">The result of the operation.</param>
        public static void Transform(ref GVector4 vec, ref Quaternion quat, out GVector4 result)
        {
            Quaternion v = new Quaternion(vec.X, vec.Y, vec.Z, vec.W), i, t;
            Quaternion.Invert(ref quat, out i);
            Quaternion.Multiply(ref quat, ref v, out t);
            Quaternion.Multiply(ref t, ref i, out v);

            result = new GVector4(v.X, v.Y, v.Z, v.W);
        }

        #endregion

        #endregion





        #region 3-component

        /// <summary>
        /// Gets or sets an OpenTK.Vector3 with the X, Y, and Z components of this instance.
        /// </summary>

        public GVector3 Xyz { get { return new GVector3(X, Y, Z); } set { X = value.X; Y = value.Y; Z = value.Z; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector3 with the X, Y, and Z components of this instance.
        /// </summary>

        public GVector3 Xyw { get { return new GVector3(X, Y, W); } set { X = value.X; Y = value.Y; W = value.Z; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector3 with the X, Z, and Y components of this instance.
        /// </summary>

        public GVector3 Xzy { get { return new GVector3(X, Z, Y); } set { X = value.X; Z = value.Y; Y = value.Z; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector3 with the X, Z, and W components of this instance.
        /// </summary>

        public GVector3 Xzw { get { return new GVector3(X, Z, W); } set { X = value.X; Z = value.Y; W = value.Z; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector3 with the X, W, and Y components of this instance.
        /// </summary>

        public GVector3 Xwy { get { return new GVector3(X, W, Y); } set { X = value.X; W = value.Y; Y = value.Z; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector3 with the X, W, and Z components of this instance.
        /// </summary>

        public GVector3 Xwz { get { return new GVector3(X, W, Z); } set { X = value.X; W = value.Y; Z = value.Z; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector3 with the Y, X, and Z components of this instance.
        /// </summary>

        public GVector3 Yxz { get { return new GVector3(Y, X, Z); } set { Y = value.X; X = value.Y; Z = value.Z; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector3 with the Y, X, and W components of this instance.
        /// </summary>

        public GVector3 Yxw { get { return new GVector3(Y, X, W); } set { Y = value.X; X = value.Y; W = value.Z; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector3 with the Y, Z, and X components of this instance.
        /// </summary>

        public GVector3 Yzx { get { return new GVector3(Y, Z, X); } set { Y = value.X; Z = value.Y; X = value.Z; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector3 with the Y, Z, and W components of this instance.
        /// </summary>

        public GVector3 Yzw { get { return new GVector3(Y, Z, W); } set { Y = value.X; Z = value.Y; W = value.Z; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector3 with the Y, W, and X components of this instance.
        /// </summary>

        public GVector3 Ywx { get { return new GVector3(Y, W, X); } set { Y = value.X; W = value.Y; X = value.Z; } }

        /// <summary>
        /// Gets an OpenTK.GVector3 with the Y, W, and Z components of this instance.
        /// </summary>

        public GVector3 Ywz { get { return new GVector3(Y, W, Z); } set { Y = value.X; W = value.Y; Z = value.Z; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector3 with the Z, X, and Y components of this instance.
        /// </summary>

        public GVector3 Zxy { get { return new GVector3(Z, X, Y); } set { Z = value.X; X = value.Y; Y = value.Z; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector3 with the Z, X, and W components of this instance.
        /// </summary>

        public GVector3 Zxw { get { return new GVector3(Z, X, W); } set { Z = value.X; X = value.Y; W = value.Z; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector3 with the Z, Y, and X components of this instance.
        /// </summary>

        public GVector3 Zyx { get { return new GVector3(Z, Y, X); } set { Z = value.X; Y = value.Y; X = value.Z; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector3 with the Z, Y, and W components of this instance.
        /// </summary>

        public GVector3 Zyw { get { return new GVector3(Z, Y, W); } set { Z = value.X; Y = value.Y; W = value.Z; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector3 with the Z, W, and X components of this instance.
        /// </summary>

        public GVector3 Zwx { get { return new GVector3(Z, W, X); } set { Z = value.X; W = value.Y; X = value.Z; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector3 with the Z, W, and Y components of this instance.
        /// </summary>

        public GVector3 Zwy { get { return new GVector3(Z, W, Y); } set { Z = value.X; W = value.Y; Y = value.Z; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector3 with the W, X, and Y components of this instance.
        /// </summary>

        public GVector3 Wxy { get { return new GVector3(W, X, Y); } set { W = value.X; X = value.Y; Y = value.Z; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector3 with the W, X, and Z components of this instance.
        /// </summary>

        public GVector3 Wxz { get { return new GVector3(W, X, Z); } set { W = value.X; X = value.Y; Z = value.Z; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector3 with the W, Y, and X components of this instance.
        /// </summary>

        public GVector3 Wyx { get { return new GVector3(W, Y, X); } set { W = value.X; Y = value.Y; X = value.Z; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector3 with the W, Y, and Z components of this instance.
        /// </summary>

        public GVector3 Wyz { get { return new GVector3(W, Y, Z); } set { W = value.X; Y = value.Y; Z = value.Z; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector3 with the W, Z, and X components of this instance.
        /// </summary>

        public GVector3 Wzx { get { return new GVector3(W, Z, X); } set { W = value.X; Z = value.Y; X = value.Z; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector3 with the W, Z, and Y components of this instance.
        /// </summary>

        public GVector3 Wzy { get { return new GVector3(W, Z, Y); } set { W = value.X; Z = value.Y; Y = value.Z; } }

        #endregion

        #region 4-component

        /// <summary>
        /// Gets or sets an OpenTK.GVector4 with the X, Y, W, and Z components of this instance.
        /// </summary>

        public GVector4 Xywz { get { return new GVector4(X, Y, W, Z); } set { X = value.X; Y = value.Y; W = value.Z; Z = value.W; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector4 with the X, Z, Y, and W components of this instance.
        /// </summary>

        public GVector4 Xzyw { get { return new GVector4(X, Z, Y, W); } set { X = value.X; Z = value.Y; Y = value.Z; W = value.W; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector4 with the X, Z, W, and Y components of this instance.
        /// </summary>

        public GVector4 Xzwy { get { return new GVector4(X, Z, W, Y); } set { X = value.X; Z = value.Y; W = value.Z; Y = value.W; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector4 with the X, W, Y, and Z components of this instance.
        /// </summary>

        public GVector4 Xwyz { get { return new GVector4(X, W, Y, Z); } set { X = value.X; W = value.Y; Y = value.Z; Z = value.W; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector4 with the X, W, Z, and Y components of this instance.
        /// </summary>

        public GVector4 Xwzy { get { return new GVector4(X, W, Z, Y); } set { X = value.X; W = value.Y; Z = value.Z; Y = value.W; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector4 with the Y, X, Z, and W components of this instance.
        /// </summary>

        public GVector4 Yxzw { get { return new GVector4(Y, X, Z, W); } set { Y = value.X; X = value.Y; Z = value.Z; W = value.W; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector4 with the Y, X, W, and Z components of this instance.
        /// </summary>

        public GVector4 Yxwz { get { return new GVector4(Y, X, W, Z); } set { Y = value.X; X = value.Y; W = value.Z; Z = value.W; } }

        /// <summary>
        /// Gets an OpenTK.GVector4 with the Y, Y, Z, and W components of this instance.
        /// </summary>

        public GVector4 Yyzw { get { return new GVector4(Y, Y, Z, W); } set { X = value.X; Y = value.Y; Z = value.Z; W = value.W; } }

        /// <summary>
        /// Gets an OpenTK.GVector4 with the Y, Y, W, and Z components of this instance.
        /// </summary>

        public GVector4 Yywz { get { return new GVector4(Y, Y, W, Z); } set { X = value.X; Y = value.Y; W = value.Z; Z = value.W; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector4 with the Y, Z, X, and W components of this instance.
        /// </summary>

        public GVector4 Yzxw { get { return new GVector4(Y, Z, X, W); } set { Y = value.X; Z = value.Y; X = value.Z; W = value.W; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector4 with the Y, Z, W, and X components of this instance.
        /// </summary>

        public GVector4 Yzwx { get { return new GVector4(Y, Z, W, X); } set { Y = value.X; Z = value.Y; W = value.Z; X = value.W; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector4 with the Y, W, X, and Z components of this instance.
        /// </summary>

        public GVector4 Ywxz { get { return new GVector4(Y, W, X, Z); } set { Y = value.X; W = value.Y; X = value.Z; Z = value.W; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector4 with the Y, W, Z, and X components of this instance.
        /// </summary>

        public GVector4 Ywzx { get { return new GVector4(Y, W, Z, X); } set { Y = value.X; W = value.Y; Z = value.Z; X = value.W; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector4 with the Z, X, Y, and Z components of this instance.
        /// </summary>

        public GVector4 Zxyw { get { return new GVector4(Z, X, Y, W); } set { Z = value.X; X = value.Y; Y = value.Z; W = value.W; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector4 with the Z, X, W, and Y components of this instance.
        /// </summary>

        public GVector4 Zxwy { get { return new GVector4(Z, X, W, Y); } set { Z = value.X; X = value.Y; W = value.Z; Y = value.W; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector4 with the Z, Y, X, and W components of this instance.
        /// </summary>

        public GVector4 Zyxw { get { return new GVector4(Z, Y, X, W); } set { Z = value.X; Y = value.Y; X = value.Z; W = value.W; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector4 with the Z, Y, W, and X components of this instance.
        /// </summary>

        public GVector4 Zywx { get { return new GVector4(Z, Y, W, X); } set { Z = value.X; Y = value.Y; W = value.Z; X = value.W; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector4 with the Z, W, X, and Y components of this instance.
        /// </summary>

        public GVector4 Zwxy { get { return new GVector4(Z, W, X, Y); } set { Z = value.X; W = value.Y; X = value.Z; Y = value.W; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector4 with the Z, W, Y, and X components of this instance.
        /// </summary>

        public GVector4 Zwyx { get { return new GVector4(Z, W, Y, X); } set { Z = value.X; W = value.Y; Y = value.Z; X = value.W; } }

        /// <summary>
        /// Gets an OpenTK.GVector4 with the Z, W, Z, and Y components of this instance.
        /// </summary>

        public GVector4 Zwzy { get { return new GVector4(Z, W, Z, Y); } set { X = value.X; W = value.Y; Z = value.Z; Y = value.W; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector4 with the W, X, Y, and Z components of this instance.
        /// </summary>

        public GVector4 Wxyz { get { return new GVector4(W, X, Y, Z); } set { W = value.X; X = value.Y; Y = value.Z; Z = value.W; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector4 with the W, X, Z, and Y components of this instance.
        /// </summary>

        public GVector4 Wxzy { get { return new GVector4(W, X, Z, Y); } set { W = value.X; X = value.Y; Z = value.Z; Y = value.W; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector4 with the W, Y, X, and Z components of this instance.
        /// </summary>

        public GVector4 Wyxz { get { return new GVector4(W, Y, X, Z); } set { W = value.X; Y = value.Y; X = value.Z; Z = value.W; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector4 with the W, Y, Z, and X components of this instance.
        /// </summary>

        public GVector4 Wyzx { get { return new GVector4(W, Y, Z, X); } set { W = value.X; Y = value.Y; Z = value.Z; X = value.W; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector4 with the W, Z, X, and Y components of this instance.
        /// </summary>

        public GVector4 Wzxy { get { return new GVector4(W, Z, X, Y); } set { W = value.X; Z = value.Y; X = value.Z; Y = value.W; } }

        /// <summary>
        /// Gets or sets an OpenTK.GVector4 with the W, Z, Y, and X components of this instance.
        /// </summary>

        public GVector4 Wzyx { get { return new GVector4(W, Z, Y, X); } set { W = value.X; Z = value.Y; Y = value.Z; X = value.W; } }

        /// <summary>
        /// Gets an OpenTK.GVector4 with the W, Z, Y, and W components of this instance.
        /// </summary>

        public GVector4 Wzyw { get { return new GVector4(W, Z, Y, W); } set { X = value.X; Z = value.Y; Y = value.Z; W = value.W; } }

        #endregion

        #endregion

        #region Operators

        /// <summary>
        /// Adds two instances.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>The result of the calculation.</returns>
        public static GVector4 operator +(GVector4 left, GVector4 right)
        {
            left.X += right.X;
            left.Y += right.Y;
            left.Z += right.Z;
            left.W += right.W;
            return left;
        }

        /// <summary>
        /// Subtracts two instances.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>The result of the calculation.</returns>
        public static GVector4 operator -(GVector4 left, GVector4 right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            left.Z -= right.Z;
            left.W -= right.W;
            return left;
        }

        /// <summary>
        /// Negates an instance.
        /// </summary>
        /// <param name="vec">The instance.</param>
        /// <returns>The result of the calculation.</returns>
        public static GVector4 operator -(GVector4 vec)
        {
            vec.X = -vec.X;
            vec.Y = -vec.Y;
            vec.Z = -vec.Z;
            vec.W = -vec.W;
            return vec;
        }

        /// <summary>
        /// Multiplies an instance by a scalar.
        /// </summary>
        /// <param name="vec">The instance.</param>
        /// <param name="scale">The scalar.</param>
        /// <returns>The result of the calculation.</returns>
        public static GVector4 operator *(GVector4 vec, float scale)
        {
            vec.X *= scale;
            vec.Y *= scale;
            vec.Z *= scale;
            vec.W *= scale;
            return vec;
        }

        /// <summary>
        /// Multiplies an instance by a scalar.
        /// </summary>
        /// <param name="scale">The scalar.</param>
        /// <param name="vec">The instance.</param>
        /// <returns>The result of the calculation.</returns>
        public static GVector4 operator *(float scale, GVector4 vec)
        {
            vec.X *= scale;
            vec.Y *= scale;
            vec.Z *= scale;
            vec.W *= scale;
            return vec;
        }

        /// <summary>
        /// Divides an instance by a scalar.
        /// </summary>
        /// <param name="vec">The instance.</param>
        /// <param name="scale">The scalar.</param>
        /// <returns>The result of the calculation.</returns>
        public static GVector4 operator /(GVector4 vec, float scale)
        {
            float mult = 1.0f / scale;
            vec.X *= mult;
            vec.Y *= mult;
            vec.Z *= mult;
            vec.W *= mult;
            return vec;
        }

        /// <summary>
        /// Compares two instances for equality.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>True, if left equals right; false otherwise.</returns>
        public static bool operator ==(GVector4 left, GVector4 right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares two instances for inequality.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>True, if left does not equa lright; false otherwise.</returns>
        public static bool operator !=(GVector4 left, GVector4 right)
        {
            return !left.Equals(right);
        }



        #endregion

        #region Overrides

        #region public override string ToString()

        private static string listSeparator = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;
        /// <summary>
        /// Returns a System.String that represents the current GVector4.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("({0}{4} {1}{4} {2}{4} {3})", X, Y, Z, W, listSeparator);
        }

        #endregion

        #region public override int GetHashCode()

        /// <summary>
        /// Returns the hashcode for this instance.
        /// </summary>
        /// <returns>A System.Int32 containing the unique hashcode for this instance.</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode() ^ W.GetHashCode();
        }

        #endregion

        #region public override bool Equals(object obj)

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>True if the instances are equal; false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is GVector4))
                return false;

            return this.Equals((GVector4)obj);
        }

        #endregion

        #endregion



        #region IEquatable<GVector4> Members

        /// <summary>Indicates whether the current vector is equal to another vector.</summary>
        /// <param name="other">A vector to compare with this vector.</param>
        /// <returns>true if the current vector is equal to the vector parameter; otherwise, false.</returns>
        public bool Equals(GVector4 other)
        {
            return Math.Abs(X - other.X) < MathHelper.EPSILON &&
Math.Abs(Y - other.Y) < MathHelper.EPSILON &&
Math.Abs(Z - other.Z) < MathHelper.EPSILON &&
Math.Abs(W - other.W) < MathHelper.EPSILON;
        }

        #endregion
    }
}

