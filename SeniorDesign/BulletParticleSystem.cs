using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using SeniorDesign.ButtonStates;
using Microsoft.Xna.Framework.Content;

namespace SeniorDesign
{
    public class BulletParticleSystem 
    {
        private Vector2 chopperPos;
        private Texture2D texture;
        private double animationTimer;
        Particle[] bullets;
        public BulletParticleSystem(Vector2 chopperPos)
        {
            this.chopperPos = chopperPos;
            bullets = new Particle[100]; 
        }
        /// <summary>
        /// FIXME protected or private?
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("red_laser");
        }

        public void Update(GameTime gameTime, Vector2 originPos)
        {

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }
    }
}
