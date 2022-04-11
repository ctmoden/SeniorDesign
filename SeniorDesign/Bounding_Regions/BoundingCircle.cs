using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SeniorDesign.Bounding_Regions
{
    /// <summary>
    /// This struct is based on CIS 580 Course Notes by Dr. Nathan Bean
    /// </summary>
    public struct BoundingCircle
    {
        /// <summary>
        /// center of bounding circle
        /// </summary>
        public Vector2 Center;
        /// <summary>
        /// radius of bounding circle
        /// </summary>
        public float Radius;
        /// <summary>
        /// all fields in struct must be explicitly set in constructor
        /// </summary>
        public BoundingCircle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }


        public bool CollidesWith(BoundingRectangle other)
        {
            return CollisionHelper.Collides(this, other);
        }
    }
}
