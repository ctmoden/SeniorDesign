using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using SeniorDesign.ButtonStates;

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
        public Vector2 Position;

        public Vector2 StartPosition;

        public Vector2 Velocity;

        public Color Color;

        public float Scale;

        private Texture2D texture;

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
    }
}
