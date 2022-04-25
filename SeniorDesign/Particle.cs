using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using SeniorDesign.ButtonStates;
using SeniorDesign.Bounding_Regions;

namespace SeniorDesign
{
    /// <summary>
    /// FIXME refactor to work for dragon flames
    /// </summary>
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
        private const int FIRE_VELOCITY_FLAME = 50;
        private const int FIRE_VELOCITY_BULLET = 3000; 
        public BoundingRectangle Bounds => bounds;
   
        public Vector2 Position;

        public Vector2 StartPosition;

        public Vector2 Velocity;

        public Color Color;

        public float Scale;

        public bool Fired;

        public bool Alive;

        /// <summary>
        /// FIXME change params later to initialize bullets
        /// need start pos and ve
        /// each particle will have it's own start position, velocity, and color
        /// </summary>
        public void Initialize()
        {
            /*this.Color = color;
            this.Velocity = velocity;
            this.StartPosition = startPos;*/
            //FIXME THIS IS FOR FLAMES ONLY
            Fired = true;
            Alive = true;
            Velocity = new Vector2(FIRE_VELOCITY_FLAME, 0);
        }
        public void InitializeBounds(Vector2 position, int width, int height)
        {
            bounds = new BoundingRectangle(position.X, position.Y, width, height);
        }
        /// <summary>
        /// Updates bounds as particle is flying through the air
        /// </summary>
        public void UpdateBounds(int xOffset, int yOffset)
        {
            bounds.X = Position.X + xOffset;
            bounds.Y = Position.Y + yOffset;
        }       
    }
}
