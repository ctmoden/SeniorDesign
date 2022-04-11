using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeniorDesign.Bounding_Regions
{
    public static class CollisionHelper
    {
        /// <summary>
        /// Detects collision of two bounding rectangles
        /// </summary>
        /// <param name="a">first rect </param>
        /// <param name="b">second rect</param>
        /// <returns>true if colliding</returns>
        public static bool Collides(BoundingRectangle a, BoundingRectangle b)
        {
            //check if one rectangle is right, left, above, or below rectangle
            //check if they are NOT colliding
            return !(a.Right < b.Left || a.Left > b.Right ||
                     a.Top > b.Bottom || a.Bottom < b.Top);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c">circle</param>
        /// <param name="r">rectangle</param>
        /// <returns>true if dist of nearest point on rectangle is inside/touching circle</returns>
        public static bool Collides(BoundingCircle c, BoundingRectangle r)
        {
            //finds nearest point in rectangle to center of the circle 
            float nearestX = MathHelper.Clamp(c.Center.X, r.Left, r.Right);
            float nearestY = MathHelper.Clamp(c.Center.Y, r.Top, r.Bottom);
            return Math.Pow(c.Radius, 2) >=
                Math.Pow(c.Center.X - nearestX, 2) +
                Math.Pow(c.Center.Y - nearestY, 2);
            //if dist from point is less that rad, then circle is touching or in the box

        }

        public static bool Collides(BoundingRectangle r, BoundingCircle c)
        {
            //finds nearest point in rectangle to center of the circle 
            float nearestX = MathHelper.Clamp(c.Center.X, r.Left, r.Right);
            float nearestY = MathHelper.Clamp(c.Center.Y, r.Top, r.Bottom);
            return Math.Pow(c.Radius, 2) >=
                Math.Pow(c.Center.X - nearestX, 2) +
                Math.Pow(c.Center.Y - nearestY, 2);
            //if dist from point is less that rad, then circle is touching or in the box
        }
    }
}
