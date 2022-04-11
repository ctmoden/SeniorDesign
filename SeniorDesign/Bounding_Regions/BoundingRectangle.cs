using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SeniorDesign.Bounding_Regions
{
    /// <summary>
    /// This struct is based on CIS 580 Course Notes by Dr. Nathan Bean
    /// Represents rectangular bounding region of a sprite
    /// </summary>
    public struct BoundingRectangle
    {
        public float X;

        public float Y;

        public float Width;

        public float Height;

        public float Left => X;

        public float Right => X + Width;

        public float Top => Y;

        public float Bottom => Y + Height;

        public BoundingRectangle(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
        /// <summary>
        /// FIXME had to switch width and height(width, height) -> (height, width)
        /// </summary>
        /// <param name="position"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        public BoundingRectangle(Vector2 position, float height, float width)
        {
            X = position.X;
            Y = position.Y;
            Width = width;
            Height = height;
        }

        public bool CollidesWith(BoundingRectangle other)
        {
            return CollisionHelper.Collides(this, other);
        }

        public bool CollidesWith(BoundingCircle other)
        {
            return CollisionHelper.Collides(other, this);
        }
    }
}
