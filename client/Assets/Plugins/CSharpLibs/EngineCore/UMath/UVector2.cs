using System;

namespace UMath
{
    /// <summary>
    /// U vector2.
    /// </summary>
    public struct UVector2
    {
        /// <summary>
        /// The x,y
        /// </summary>
        public float x,y;

        /// <summary>
        /// Initializes a new instance of the <see cref="UMath.UVector2"/> struct.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public UVector2(float x,float y)
        {
            this.x = x;
            this.y = y;
        }
            

        #region Operator

        public static UVector2 operator +(UVector2 v1, UVector2 v2)
        {
            return new UVector2(v1.x + v2.x, v1.y + v2.y);
        }

        #endregion
    }
}

