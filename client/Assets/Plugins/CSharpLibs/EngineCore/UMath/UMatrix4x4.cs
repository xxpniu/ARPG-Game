using System;

namespace UMath
{
    /// <summary>
    /// column-major left-hand 
    /// </summary>
    [Serializable]
    public struct UMatrix4x4
    {
        /// <summary>
        /// The m11.
        /// </summary>
        public float m11, m12, m13, m14,
            m21, m22, m23, m24,
            m31, m32, m33, m34,
            m41, m42, m43, m44;

        public UMatrix4x4(float m11, float m12, float m13,float m14,
            float m21,float m22, float m23,float m24,
            float m31,float m32,float m33,float m34,
            float m41,float m42,float m43,float m44)
        {
            this.m11 = m11;
            this.m12 = m12;
            this.m13 = m13;
            this.m14 = m14;

            this.m21 = m21;
            this.m22 = m22;
            this.m23 = m23;
            this.m24 = m24;

            this.m31 = m31;
            this.m32 = m32;
            this.m33 = m33;
            this.m34 = m34;

            this.m41 = m41;
            this.m42 = m42;
            this.m43 = m43;
            this.m44 = m44;
        }


        #region public 

        /// <summary>
        /// Gets the determinant of this matrix.
        /// </summary>
        public float Determinant
        {
            get
            {
                return
                    m11 * m22 * m33 * m44 - m11 * m22 * m34 * m43 + m11 * m23 * m34 * m42 - m11 * m23 * m32 * m44
                    + m11 * m24 * m32 * m43 - m11 * m24 * m33 * m42 - m12 * m23 * m34 * m41 + m12 * m23 * m31 * m44
                    - m12 * m24 * m31 * m43 + m12 * m24 * m33 * m41 - m12 * m21 * m33 * m44 + m12 * m21 * m34 * m43
                    + m13 * m24 * m31 * m42 - m13 * m24 * m32 * m41 + m13 * m21 * m32 * m44 - m13 * m21 * m34 * m42
                    + m13 * m22 * m34 * m41 - m13 * m22 * m31 * m44 - m14 * m21 * m32 * m43 + m14 * m21 * m33 * m42
                    - m14 * m22 * m33 * m41 + m14 * m22 * m31 * m43 - m14 * m23 * m31 * m42 + m14 * m23 * m32 * m41;
            }
        }
           

        /// <summary>
        /// Gets the translate.
        /// </summary>
        /// <value>The translate.</value>
        public UVector3 Translate
        {
            get
            {
                return new UVector3(m14, m24, m34);
            }
        }
        /// <summary>
        /// Clears the translate.
        /// </summary>
        public void ClearTranslate()
        {
            m14 = m24 = m34 = 0;
        }
        /// <summary>
        /// Gets the scale.
        /// </summary>
        /// <value>The scale.</value>
        public UVector3 Scale
        {
            get
            {
                var x = new UVector3(m11, m21, m31).magnitude;
                var y = new UVector3(m12, m22, m32).magnitude;
                var z = new UVector3(m13, m23, m33).magnitude;
                return new UVector3(x, y, z);
            }
        }
        /// <summary>
        /// Clears the scale.
        /// </summary>
        public void ClearScale()
        {
            var x = new UVector3(m11, m21, m31).normalized;
            var y = new UVector3(m12, m22, m32).normalized;
            var z = new UVector3(m13, m23, m33).normalized;
            m11 = x[1];
            m21 = x[2];
            m31 = x[3];

            m12 = y[1];
            m22 = y[2];
            m32 = y[3];

            m13 = z[1];
            m23 = z[2];
            m33 = z[3];
        }
        /// <summary>
        /// Gets the rotation.
        /// </summary>
        /// <value>The rotation.</value>
        public UQuaternion Rotation
        {
            get
            { 
                var q = new UQuaternion();
                float tr = m11 + m22 + m33;

                if (tr > MathHelper.Epsilon)
                { 
                    float S = (float)Math.Sqrt(tr + 1.0f) * 2f; // S=4*qw 
                    q.w = 0.25f * S;
                    q.x = (m32 - m23) / S;
                    q.y = (m13 - m31) / S; 
                    q.z = (m21 - m12) / S; 
                }
                else if ((m11 > m22) && (m11 > m33))
                { 
                    float S = (float)Math.Sqrt(1.0 + m11 - m22 - m33) * 2; // S=4*qx 
                    q.w = (m32 - m23) / S;
                    q.x = 0.25f * S;
                    q.y = (m12 + m21) / S; 
                    q.z = (m13 + m31) / S; 
                }
                else if (m22 > m33)
                { 
                    float S = (float)Math.Sqrt(1.0 + m22 - m11 - m33) * 2; // S=4*qy
                    q.w = (m13 - m31) / S;
                    q.x = (m12 + m21) / S; 
                    q.y = 0.25f * S;
                    q.z = (m23 + m32) / S; 
                }
                else
                { 
                    float S = (float)Math.Sqrt(1.0 + m33 - m11 - m22) * 2; // S=4*qz
                    q.w = (m21 - m12) / S;
                    q.x = (m13 + m31) / S;
                    q.y = (m23 + m32) / S;
                    q.z = 0.25f * S;
                }
                q.Normalize();
                return q;
            }
        }
        /// <summary>
        /// Clears the rotation.
        /// </summary>
        public void ClearRotation()
        {
            var scale = this.Scale;
            this[1, 1] = this[1, 2] = this[1, 3] =
                this[2, 1] = this[2, 2] = this[2, 3] =
                    this[3, 1] = this[3, 2] = this[3, 3] = 0;
            this[1, 1] = scale.x;
            this[2, 2] = scale.y;
            this[3, 3] = scale.z;
        }
        /// <summary>
        /// Gets or sets the <see cref="UMath.UMatrix4x4"/> with the specified row column.
        /// </summary>
        /// <param name="row">Row.</param>
        /// <param name="column">column.</param>
        public float this [int row, int column]
        {
            set
            {
                #region set values
                switch (row)
                {
                    case 1:
                        switch (column)
                        {
                            case 1:
                                m11 = value;
                                break;
                            case 2:
                                m12 = value;
                                break;
                            case 3:
                                m13 = value;
                                break;
                            case 4:
                                m14 = value;
                                break;
                            default:
                                throw new IndexOutOfRangeException("column out of index:" + column);
                        }
                        break;
                    case 2:
                        switch (column)
                        {
                            case 1:
                                m21 = value;
                                break;
                            case 2:
                                m22 = value;
                                break;
                            case 3:
                                m23 = value;
                                break;
                            case 4:
                                m24 = value;
                                break;
                            default:
                                throw new IndexOutOfRangeException("column out of index:" + column);
             
                        }
                        break;
                    case 3:
                        switch (column)
                        {
                            case 1:
                                m31 = value;
                                break;
                            case 2:
                                m32 = value;
                                break;
                            case 3:
                                m33 = value;
                                break;
                            case 4:
                                m34 = value;
                                break;
                            default:
                                throw new IndexOutOfRangeException("column out of index:" + column);
                        }
                        break;
                    case 4:
                        switch (column)
                        {
                            case 1:
                                m41 = value;
                                break;
                            case 2:
                                m42 = value;
                                break;
                            case 3:
                                m43 = value;
                                break;
                            case 4:
                                m44 = value;
                                break;
                            default:
                                throw new IndexOutOfRangeException("column out of index:" + column);
                        }
                        break;
                    default:
                        throw new IndexOutOfRangeException("row out of index:" + row);
                }
                #endregion
            }

            get
            {
                #region get
                switch (row)
                {
                    case 1:
                        switch (column)
                        {
                            case 1:
                                return  m11;

                            case 2:
                                return m12;

                            case 3:
                                return m13;

                            case 4:
                                return m14;
                            default:
                                throw new IndexOutOfRangeException("column out of index:" + column);
                               
                        }
                    case 2:
                        switch (column)
                        {
                            case 1:
                                return m21;
                            case 2:
                                return m22;
                            case 3:
                                return m23;
                            case 4:
                                return m24;
                            default:
                                throw new IndexOutOfRangeException("column out of index:" + column);

                        }
                    case 3:
                        switch (column)
                        {
                            case 1:
                                return m31;
                            case 2:
                                return m32;
                            case 3:
                                return m33;
                            case 4:
                                return m34;
                            default:
                                throw new IndexOutOfRangeException("column out of index:" + column);
                               
                        }
                    case 4:
                        switch (column)
                        {
                            case 1:
                                return  m41;
                            case 2:
                                return  m42;
                            case 3:
                                return  m43;
                            case 4:
                                return m44;
                            default:
                                throw new IndexOutOfRangeException("column out of index:" + column);

                        }
                    default:
                        throw new IndexOutOfRangeException("row out of index:" + row);

                }
                #endregion
            }
        }              
        /// <summary>
        /// Invert the specified mat.
        /// </summary>
        /// <param name="mat">Mat.</param>
        public UMatrix4x4 Inverted()
        {
            var result = UMatrix4x4.zero;
            int[] colIdx = { 0, 0, 0, 0 };
            int[] rowIdx = { 0, 0, 0, 0 };
            int[] pivotIdx = { -1, -1, -1, -1 };

            // convert the matrix to an array for easy looping
            float[,] inverse =  {
                {m11, m12, m13, m14},
                {m21, m22, m23, m24},
                {m31, m32, m33, m34},
                {m41, m42, m43, m44} 
            };
            int icol = 0;
            int irow = 0;
            for (int i = 0; i < 4; i++)
            {
                // Find the largest pivot value
                float maxPivot = 0.0f;
                for (int j = 0; j < 4; j++)
                {
                    if (pivotIdx[j] != 0)
                    {
                        for (int k = 0; k < 4; ++k)
                        {
                            if (pivotIdx[k] == -1)
                            {
                                float absVal = System.Math.Abs(inverse[j, k]);
                                if (absVal > maxPivot)
                                {
                                    maxPivot = absVal;
                                    irow = j;
                                    icol = k;
                                }
                            }
                            else if (pivotIdx[k] > 0)
                            {
                                result = this;
                                return result;
                            }
                        }
                    }
                }

                ++(pivotIdx[icol]);

                // Swap rows over so pivot is on diagonal
                if (irow != icol)
                {
                    for (int k = 0; k < 4; ++k)
                    {
                        float f = inverse[irow, k];
                        inverse[irow, k] = inverse[icol, k];
                        inverse[icol, k] = f;
                    }
                }

                rowIdx[i] = irow;
                colIdx[i] = icol;

                float pivot = inverse[icol, icol];
                // check for singular matrix
                if (pivot == 0.0f)
                {
                    throw new InvalidOperationException("Matrix is singular and cannot be inverted.");
                }

                // Scale row so it has a unit diagonal
                float oneOverPivot = 1.0f / pivot;
                inverse[icol, icol] = 1.0f;
                for (int k = 0; k < 4; ++k)
                    inverse[icol, k] *= oneOverPivot;

                // Do elimination of non-diagonal elements
                for (int j = 0; j < 4; ++j)
                {
                    // check this isn't on the diagonal
                    if (icol != j)
                    {
                        float f = inverse[j, icol];
                        inverse[j, icol] = 0.0f;
                        for (int k = 0; k < 4; ++k)
                            inverse[j, k] -= inverse[icol, k] * f;
                    }
                }
            }

            for (int j = 3; j >= 0; --j)
            {
                int ir = rowIdx[j];
                int ic = colIdx[j];
                for (int k = 0; k < 4; ++k)
                {
                    float f = inverse[k, ir];
                    inverse[k, ir] = inverse[k, ic];
                    inverse[k, ic] = f;
                }
            }

            result.m11 = inverse[0, 0];
            result.m12 = inverse[0, 1];
            result.m13 = inverse[0, 2];
            result.m14 = inverse[0, 3];
            result.m21 = inverse[1, 0];
            result.m22 = inverse[1, 1];
            result.m23 = inverse[1, 2];
            result.m24 = inverse[1, 3];
            result.m31 = inverse[2, 0];
            result.m32 = inverse[2, 1];
            result.m33 = inverse[2, 2];
            result.m34 = inverse[2, 3];
            result.m41 = inverse[3, 0];
            result.m42 = inverse[3, 1];
            result.m43 = inverse[3, 2];
            result.m44 = inverse[3, 3];
            return result;
        }     
        #endregion

        #region static
        /// <summary>
        /// The identity.
        /// </summary>
        public static UMatrix4x4 identity = new 
            UMatrix4x4(1, 0, 0, 0,
                       0, 1, 0, 0,
                       0, 0, 1, 0,
                       0, 0, 0, 1);
        /// <summary>
        /// The zero.
        /// </summary>
        public static UMatrix4x4 zero = new UMatrix4x4(0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0);

        /// <summary>
        /// Creates the TRS.
        /// </summary>
        /// <returns>The TR.</returns>
        /// <param name="trans">Trans.</param>
        /// <param name="rot">Rot.</param>
        /// <param name="scale">Scale.</param>
        public static UMatrix4x4 TRS(UVector3 trans,UQuaternion rot, UVector3 scale)
        {
            return CreateTranslate(trans) * CreateRotation(rot) * CreateScale(scale);
        }

        /// <summary>
        /// Creates the translate.
        /// </summary>
        /// <returns>The translate.</returns>
        /// <param name="trans">Trans.</param>
        public static UMatrix4x4 CreateTranslate(UVector3 trans)
        {
            var mTrasn = identity;
            mTrasn.m14 = trans.x;
            mTrasn.m24 = trans.y;
            mTrasn.m34 = trans.z;
            return mTrasn;
        }
        /// <summary>
        /// Creates the rotation.
        /// </summary>
        /// <returns>The rotation.</returns>
        /// <param name="rot">Rot.</param>
        public static UMatrix4x4 CreateRotation(UQuaternion rot)
        {
            return rot.ToMartix();
        }

        /// <summary>
        /// Creates the scale.
        /// </summary>
        /// <returns>The scale.</returns>
        /// <param name="scale">Scale.</param>
        public static UMatrix4x4 CreateScale(UVector3 scale)
        {
            var mscale = identity;
            mscale.m11 = scale.x;
            mscale.m22 = scale.y;
            mscale.m33 = scale.z;
            return mscale;
        }

        /// <summary>
        /// Looks at position, target and up.
        /// </summary>
        /// <returns>The <see cref="UMath.UMatrix4x4"/>.</returns>
        /// <param name="eye">Position.</param>
        /// <param name="target">Target.</param>
        /// <param name="up">Up.</param>
        public static UMatrix4x4 LookAt(UVector3 eye,UVector3 target,UVector3 up)
        {
            var w = (target - eye);
            w.Normalized();
            var u = UVector3.Cross(up, w);
            u.Normalized();
            var v = UVector3.Cross(w, u);
            v.Normalized();
            var trans = CreateTranslate(-eye);
            var m = new UMatrix4x4(
                        u.x, v.x, w.x, 0,
                        u.y, v.y, w.y, 0,
                        u.z, v.z, w.z, 0,
                        0, 0, 0, 1);
            return trans * m;
        }

        /// <summary>
        /// Decompose the specified m, trans, rotation and scale.
        /// </summary>
        /// <param name="m">M.</param>
        /// <param name="trans">Trans.</param>
        /// <param name="rotation">Rotation.</param>
        /// <param name="scale">Scale.</param>
        public static void Decompose(UMatrix4x4 m, out UVector3 trans,out UQuaternion rotation, out UVector3 scale)
        {
            trans = m.Translate;
            rotation = m.Rotation;
            scale = m.Scale;
        }
        #endregion

        #region operator

        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        public static UMatrix4x4 operator *(UMatrix4x4 left, UMatrix4x4 right)
        {
            var result = zero;
            for (int row = 1; row <= 4; row++)
            {
                for (var column = 1; column <= 4; column++)
                {
                    for (var k = 1; k <= 4; k++)
                    {
                        result[row, column] += left[row, k] * right[k, column];
                    }
                }
            }
            return result;
        }

        /// <param name="matrix">Matrix.</param>
        /// <param name="v">V.</param>
        public static UVector4 operator *(UMatrix4x4 matrix, UVector4 v)
        {
            if (Math.Abs( v.w)< MathHelper.Epsilon)
            {
                matrix.ClearScale();
                matrix.ClearTranslate();
            }
            var res = UVector4.zero;
            for (var index = 1; index <= 4; index++)
            {
                for (var k = 1; k <= 4; k++)
                {
                    res[index] += v[k] * matrix[index, k];
                }
            }
            return res;
        }

        #endregion
    }
}

