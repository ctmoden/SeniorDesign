using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using SeniorDesign.ButtonStates;
using SeniorDesign.Bounding_Regions;

namespace SeniorDesign
{
    public struct Particle 
    {
        //what all particles can share:
        /*
        texture file
        start position
        color
        sprite batch
        min/max particles

         */
        private BoundingRectangle bounds;
        public BoundingRectangle Bounds => bounds;
   
        public Vector2 Position;

        public Vector2 StartPosition;

        public Vector2 Velocity;

        public Color Color;

        public float Scale;

        
        public bool Fired;

        /// <summary>
        /// FIXME change params later
        /// need start pos and ve
        /// each particle will have it's own start position, velocity, and color
        /// </summary>
        public void Initialize(Vector2 startPos, Vector2 velocity, Color color)
        {
            this.Color = color;
            this.Velocity = velocity;
            this.StartPosition = startPos;
        }
        public void InitializeBounds(Vector2 position)
        {
            bounds = new BoundingRectangle(position.X, position.Y, 10, 15);
        }
        /// <summary>
        /// Updates bounds as particle is flying through the air
        /// </summary>
        public void UpdateBounds()
        {
            bounds.X = Position.X +75;
            bounds.Y = Position.Y;
        }       
    }
}
