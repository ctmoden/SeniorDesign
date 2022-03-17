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
        private Vector2 position;
        public Vector2 Position=>position;

        private Vector2 startPosition;

        public Vector2 Velocity;

        public Color Color;

        public float Scale;

        private Texture2D texture;

        /// <summary>
        /// FIXME change params later
        /// </summary>
        public void Initialize()
        {

        }
    }
}
