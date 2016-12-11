using System;

namespace UMath
{
    /// <summary>
    /// quaternion.
    /// rotation 
    /// </summary>
    [Serializable]
    public struct UQuaternion
    {
        public float x,y,z,w;

        public UQuaternion(float x,float y,float z,float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public UQuaternion(UVector3 xyz,float w):this(xyz.x,xyz.y,xyz.z,w)
        {
            
        }

        #region public 
        /// <summary>
        /// Gets or sets the xyzw.
        /// </summary>
        /// <value>The xyzw.</value>
        public UVector4 Xyzw
        {
            get
            { 
                return new UVector4(x, y, z, w);
            }
            set
            {
                x = value.x;
                y = value.y;
                z = value.z;
                w = value.w;
            }
        }
        /// <summary>
        /// Gets or sets the xyz.
        /// </summary>
        /// <value>The xyz.</value>
        public UVector3 Xyz
        {
            set
            {
                this.x = value.x;
                this.y = value.y;
                this.z = value.z;
            }
            get
            {
                return new UVector3(x, y, z);
            }
        }
        /// <summary>
        /// Inverts the Vector3 component of this Quaternion.
        /// </summary>
        public void Conjugate()
        {
            x = -x;
            y = -y;
            z = -z;
        }

        /// <summary>
        /// Inverts the Vector3 component of this Quaternion.
        /// </summary>
        public UQuaternion conjugated
        {
            get
            {
                var q = this;
                q.Conjugate();
                return q;
            }
        }
        /// <summary>
        /// Invert this instance.
        /// </summary>
        public void Invert()
        {
            w = -w;
        }
        /// <summary>
        /// Tos the martix.
        /// </summary>
        /// <returns>The martix.</returns>
        public UMatrix4x4 ToMartix()
        {
            //var m = UMatrix4x4.identity;
            float x2 = x * x;
            float y2 = y * y;
            float z2 = z * z;
            float xy = x * y;
            float xz = x * z;
            float yz = y * z;
            float wx = w * x;
            float wy = w * y;
            float wz = w * z;

            return new UMatrix4x4(1.0f - 2.0f * (y2 + z2), 2.0f * (xy - wz), 2.0f * (xz + wy), 0.0f,
                2.0f * (xy + wz), 1.0f - 2.0f * (x2 + z2), 2.0f * (yz - wx), 0.0f,
                2.0f * (xz - wy), 2.0f * (yz + wx), 1.0f - 2.0f * (x2 + y2), 0.0f,
                0.0f, 0.0f, 0.0f, 1.0f);
        }
        /// <summary>
        /// Gets the euler angles.
        /// </summary>
        /// <value>The euler angles.</value>
        public UVector3 eulerAngles
        {
            get
            {
                var n = this;
                n.Normalize();
                var ysqr = n.y * n.y;
                var t0 = -2.0f * (ysqr + n.z * n.z) + 1.0f;
                var t1 = -2.0f * (n.x * n.y - n.w * n.z);
                var t2 = 2.0f * (n.x * n.z + n.w * n.y);
                var t3 = -2.0f * (n.y * n.z - n.w * n.x);
                var t4 = -2.0f * (n.x * n.x + ysqr) + 1.0f;
                t2 = t2 > 1.0f ? 1.0f : t2;
                t2 = t2 < -1.0f ? -1.0f : t2;

                var pitch = MathHelper.AngleFormat((float)Math.Asin(t2) * MathHelper.Rad2Deg);
                var roll = MathHelper.AngleFormat((float)Math.Atan2(t3, t4) * MathHelper.Rad2Deg);
                var yaw = MathHelper.AngleFormat((float)Math.Atan2(t1, t0) * MathHelper.Rad2Deg);
                return new UVector3(roll, pitch, yaw);
            }
        }
        /// <summary>
        /// Tos the angle axis.
        /// </summary>
        /// <param name="angle">Angle.</param>
        /// <param name="axis">Axis.</param>
        public void ToAngleAxis(out float angle,out UVector3 axis)
        {
            axis= this.Xyz.normalized;
            angle = (float)Math.Acos(w) * 2.0f * MathHelper.Rad2Deg;
        }
        /// <summary>
        /// Gets the square of the quaternion length (magnitude).
        /// </summary>
        public float LengthSquared
        {
            get
            {
                return w * w + Xyz.sqrMagnitude;
            }
        }
        /// <summary>
        /// Gets the length (magnitude) of the quaternion.
        /// </summary>
        /// <seealso cref="LengthSquared"/>
        public float Length
        {
            get
            {
                return (float)System.Math.Sqrt(LengthSquared);
            }
        }
        /// <summary>
        /// Scales the Quaternion to unit length.
        /// </summary>
        public void Normalize()
        {
            if (this.Length <= MathHelper.Epsilon)
            {
                Xyzw = identity.Xyzw;
                    return;
            }
            float scale = 1.0f / this.Length;
            Xyz *= scale;
            w *= scale;
        }
        #endregion

        #region static
        /// <summary>
        /// The identity (0,0,0,0).
        /// </summary>
        public static UQuaternion identity =new  UQuaternion(0,0,0,1);
        /// <summary>
        /// Angles the axis.
        /// </summary>
        /// <returns>The axis.</returns>
        /// <param name="angle">Angle.</param>
        /// <param name="axis">Axis.</param>
        public static UQuaternion AngleAxis(float angle,UVector3 axis)
        {
            if (Math.Abs(axis.sqrMagnitude) < MathHelper.Epsilon)
                return identity;
            
            axis.Normalized();
            var sin = (float)Math.Sin(angle * MathHelper.Deg2Rad * 0.5f);
            var cos = (float)Math.Cos(angle * MathHelper.Deg2Rad * 0.5f);
            float w = cos;
            float x = axis.x * sin;
            float y = axis.y * sin;
            float z = axis.z * sin;

            return new UQuaternion(x, y, z, w);
        }
        /// <summary>
        /// Euler the specified x, y and z.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">The z coordinate.</param>
        public static UQuaternion Euler(float x,float y,float z)
        {
            return 
            AngleAxis(y, UVector3.up)
            * AngleAxis(x, UVector3.right)
            * AngleAxis(z, UVector3.forward);
        }
        /// <summary>
        /// Euler the specified eulerAngles.
        /// </summary>
        /// <param name="eulerAngles">Euler angles.</param>
        public static UQuaternion Euler(UVector3 eulerAngles)
        {
            return Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z);
        }

        /// <summary>
        /// Normaliz the specified q.
        /// </summary>
        /// <param name="q">Q.</param>
        public static UQuaternion Normalize(UQuaternion q)
        {
            q.Normalize();
            return q;
        }

        /// <summary>
        /// Do Spherical linear interpolation between two quaternions 
        /// </summary>
        /// <param name="q1">The first quaternion</param>
        /// <param name="q2">The second quaternion</param>
        /// <param name="blend">The blend factor</param>
        /// <returns>A smooth blend between the given quaternions</returns>
        public static UQuaternion Slerp(UQuaternion q1, UQuaternion q2, float blend)
        {
            // if either input is zero, return the other.
            if (q1.LengthSquared == 0.0f)
            {
                if (q2.LengthSquared == 0.0f)
                {
                    return identity;
                }
                return q2;
            }
            else if (q2.LengthSquared == 0.0f)
            {
                return q1;
            }


            float cosHalfAngle = q1.w * q2.w + UVector3.Dot(q1.Xyz, q2.Xyz);

            if (cosHalfAngle >= 1.0f || cosHalfAngle <= -1.0f)
            {
                // angle = 0.0f, so just return one input.
                return q1;
            }
            else if (cosHalfAngle < 0.0f)
            {
                q2.Xyz = -q2.Xyz;
                q2.w = -q2.w;
                cosHalfAngle = -cosHalfAngle;
            }

            float blendA;
            float blendB;
            if (cosHalfAngle < 0.99f)
            {
                // do proper slerp for big angles
                float halfAngle = (float)System.Math.Acos(cosHalfAngle);
                float sinHalfAngle = (float)System.Math.Sin(halfAngle);
                float oneOverSinHalfAngle = 1.0f / sinHalfAngle;
                blendA = (float)System.Math.Sin(halfAngle * (1.0f - blend)) * oneOverSinHalfAngle;
                blendB = (float)System.Math.Sin(halfAngle * blend) * oneOverSinHalfAngle;
            }
            else
            {
                // do lerp if angle is really small.
                blendA = 1.0f - blend;
                blendB = blend;
            }

            var result = new UQuaternion(
                blendA * q1.Xyz + blendB * q2.Xyz, 
                blendA * q1.w + blendB * q2.w);
            if (result.LengthSquared > 0.0f)
                return Normalize(result);
            else
                return identity;
        }
        /// <summary>
        /// Looks the rotation.
        /// </summary>
        /// <returns>The rotation.</returns>
        /// <param name="forward">Forward.</param>
        /// <param name="up">Up.</param>
        public static UQuaternion LookRotation(UVector3 forward, UVector3 up)
        {
            return UMatrix4x4.LookAt(UVector3.zero, forward, up).Rotation;
        }
        /// <summary>
        /// Looks the rotation.
        /// </summary>
        /// <returns>The rotation.</returns>
        /// <param name="forward">Forward.</param>
        public static UQuaternion LookRotation(UVector3 forward)
        {
            return LookRotation(forward, UVector3.up);
        }
        #endregion

        #region operator
        /// <summary>
        /// Adds two instances.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>The result of the calculation.</returns>
        public static UQuaternion operator +(UQuaternion left, UQuaternion right)
        {
            left.Xyz += right.Xyz;
            left.w+= right.w;
            return left;
        }
        /// <param name="l">L.</param>
        /// <param name="r">R.</param>
        public static UQuaternion operator *(UQuaternion l,UQuaternion r)
        {
            return new UQuaternion(
                l.w * r.x + l.x * r.w + l.y * r.z - l.z * r.y,
                l.w * r.y + l.y * r.w + l.z * r.x - l.x * r.z,
                l.w * r.z + l.z * r.w + l.x * r.y - l.y * r.x,
                l.w * r.w - l.x * r.x - l.y * r.y - l.z * r.z);
        }

        /// <summary>
        /// Multiplies an instance by a scalar.
        /// </summary>
        /// <param name="quaternion">The instance.</param>
        /// <param name="scale">The scalar.</param>
        /// <returns>A new instance containing the result of the calculation.</returns>
        public static UQuaternion operator *(UQuaternion quaternion, float scale)
        {
            return scale * quaternion;
        }

        /// <summary>
        /// Multiplies an instance by a scalar.
        /// </summary>
        /// <param name="quaternion">The instance.</param>
        /// <param name="scale">The scalar.</param>
        /// <returns>A new instance containing the result of the calculation.</returns>
        public static UQuaternion operator *(float scale, UQuaternion quaternion)
        {
            return new UQuaternion(
                quaternion.x * scale, 
                quaternion.y * scale, 
                quaternion.z * scale, 
                quaternion.w * scale);
             }
            
        /// <param name="q">Q.</param>
        /// <param name="v">V.</param>
        public static UVector3 operator *(UQuaternion q, UVector3 v)
        {
            var n = v;
            UQuaternion vecq =identity, resQ =identity;
            vecq.Xyz = n;
            vecq.w = 1;
            resQ = vecq * q.conjugated;
            resQ = q * resQ;
            return new UVector3(resQ.x, resQ.y, resQ.z);

        }

        #endregion
       
    }
}

