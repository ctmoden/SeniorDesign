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
        private int FIRE_VELOCITY_FLAME;
        private const int FIRE_VELOCITY_BULLET = 3000; 
        public BoundingRectangle Bounds => bounds;
   
        public Vector2 Position;

        public Vector2 StartPosition;

        public Vector2 Velocity;

        public Color Color;

        public float Scale;

        public bool Fired;

        public bool Alive;

        private int hitPoints;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetPos"></param>
        /// <param name="newPosition"></param>
        public void Initialize(Vector2 targetPos, Vector2 newPosition)
        {
            /*this.Color = color;
            this.Velocity = velocity;
            this.StartPosition = startPos;*/
            //FIXME THIS IS FOR FLAMES ONLY
            Fired = true;
            Alive = true;
            FIRE_VELOCITY_FLAME = HelperMethods.Next(10, 20);//50,100
            Velocity = targetPos;
            Velocity.Normalize();
            Velocity *= FIRE_VELOCITY_FLAME;
            Position = newPosition;
        }
        /// <summary>
        /// FIXME make code one central method
        /// </summary>
        /// <param name="targetPos"></param>
        public void UpdateVelocity(Vector2 targetPos)
        {
            Velocity = targetPos;
            Velocity.Normalize();
            Velocity *= FIRE_VELOCITY_FLAME;
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
