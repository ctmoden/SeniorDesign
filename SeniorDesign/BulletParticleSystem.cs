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
        private int freeIndex;//index of first bullet ready to fire
        private double animationTimer;
        private Color color;
        private bool fired;
        private const int FIRE_VELOCITY = 3;
        private Color[] colors = new Color[]
        {
            Color.Red,
            Color.OrangeRed,
            Color.Yellow
        };
        Particle[] bullets;
        public BulletParticleSystem(Vector2 chopperPos)
        {
            this.chopperPos = chopperPos;
            bullets = new Particle[100];
            freeIndex = 0;
        }
        /// <summary>
        /// FIXME protected or private?
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("red_laser");
        }
        /// <summary>
        /// update position of bullets ready to fire to chopper
        /// fire bullets
        /// TAKE INSPIRATION FROM COIN SPRITE IN OTHER GAME
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="originPos"></param>
        public void Update(GameTime gameTime, Vector2 originPos)
        {
            //FIXME might want to modify to start at first available bullet...
            for(int i = freeIndex; i < bullets.Length;i++)
            {
                if (bullets[i].Fired)
                {
                    //update in air position, check bounding
                    FireControl(i);
                }
                else
                {
                    //set start position to origin position
                    //actually only want to reset position just before launch
                    //already did some of this in missile sprite code base 
                    bullets[i].StartPosition = originPos;

                }
            }
        }
        /// <summary>
        /// fire control for 
        /// </summary>
        /// <param name="index"></param>
        private void FireControl(int index)
        {
            if(bullets[index].Position.X < Constants.GAME_WIDTH && bullets[index].Fired)
            {
                bullets[index].Position += new Vector2(FIRE_VELOCITY, 0);
            }
            if (bullets[index].Position.X >= Constants.GAME_WIDTH && bullets[index].Fired)
            {
                bullets[index].Position += new Vector2(0, 0);
                bullets[index]
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
            

            foreach(Particle p in bullets)
            {
                color = colors[HelperMethods.Next(colors.Length)];
                spriteBatch.Draw(texture, p.Position, color);
            }
            
        }
    }
}
