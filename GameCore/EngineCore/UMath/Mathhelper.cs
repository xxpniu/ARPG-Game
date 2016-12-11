using System.Collections;
using System.Collections.Generic;
using System;

namespace UMath
{
    /// <summary>
    /// Math helper.
    /// </summary>
    public sealed class MathHelper
    {
        /// <summary>
        /// The epsilon.
        /// </summary>
        public const float Epsilon = 0.0000001f;
        /// <summary>
        /// The rad2 deg.
        /// </summary>
        public const float Rad2Deg = 57.29578f;
        /// <summary>
        /// The deg2 RAD.
        /// </summary>
        public const float Deg2Rad = 0.01745329f;

        /// <summary>
        /// Angles to 0-360
        /// </summary>
        /// <returns>The format.</returns>
        /// <param name="angle">Angle.</param>
        public static float AngleFormat(float angle)
        {
            var a = (double)angle % 360.0;
            if (a <Epsilon)
            {
                a = (float)( 360.0 + a);
            }

            if (Math.Abs(360 - a) < Epsilon)
                return 0;
            return (float)a;
        }

        /// <summary>
        /// Cals the angle with axis y.
        /// </summary>
        /// <returns>The angle with axis y.</returns>
        /// <param name="forward">Forward.</param>
        public static float CalAngleWithAxisY(UVector3 forward)
        {
            forward.y = 0;
            forward.Normalized();
            var acos = Math.Acos(forward.z) * MathHelper.Rad2Deg;
            if (forward.x > 0)
                return (float)acos;
            else
                return 360f - (float)(acos);

        }
           
        public static float Repeat(float t, float lenght)
        {
            return t - (float)Math.Floor(t / lenght) * lenght;
        }

        public static float MoveTowards(float current,float target, float maxDelta)
        {
            if (Math.Abs(target - current) <= maxDelta)
                return target;
            return current + Math.Sign(target - current) * maxDelta;
        }

        /// <summary>
        /// Deltas the angle.
        /// </summary>
        /// <returns>The angle.</returns>
        /// <param name="a">The alpha component.</param>
        /// <param name="b">The blue component.</param>
        public static float DeltaAngle(float current, float targt)
        {
            float num = Repeat(targt - current, 360f);
            if (num > 180f)
            {
                num -= 360f;
            }
            return num;
        }

        /// <summary>
        /// Moves the towards angle.
        /// </summary>
        /// <returns>The towards angle.</returns>
        /// <param name="current">Current.</param>
        /// <param name="target">Target.</param>
        /// <param name="maxDelta">Max delta.</param>
        public static float MoveTowardsAngle(float current,float target, float maxDelta)
        {
            target = current + DeltaAngle(current, target);
            return MoveTowards(current, target, maxDelta);
        }
    }
}
