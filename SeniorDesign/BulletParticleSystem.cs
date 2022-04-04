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
        private double fireTimer;
        private Color color;
        private bool fired;
        private const int FIRE_VELOCITY = 3000;
        private Color[] colors = new Color[]
        {
            Color.Red,
            Color.OrangeRed,
            Color.Yellow
        };
        Particle[] bullets;

        public bool IsFiring { get; set; }

        public BulletParticleSystem(Vector2 chopperPos)
        {
            this.chopperPos = chopperPos;
            bullets = new Particle[1000];
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
            if(IsFiring)
            {
                fireTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if(fireTimer >= 0.1)
                {
                    SpawnBullet(originPos);
                    fireTimer = 0.0;
                }
            }
            else
            {
                fireTimer = 0.1;
            }

            for(int i = 0; i < bullets.Length; i++)
            {
                if (!bullets[i].Fired) continue;
                bullets[i].Position += (float)gameTime.ElapsedGameTime.TotalSeconds * bullets[i].Velocity;
            }
            /*
            int tempIndex = freeIndex;
            //FIXME might want to modify to start at first available bullet...
            //updating unfired bullets...
            for(int i = tempIndex; i < bullets.Length;i++)
            {                
                bullets[i].StartPosition = originPos;
                bullets[i].Fired = true;
                freeIndex++;    
            }
            //update in-flight bullets
            for(int i = 0; i < freeIndex; i++)
            {
                FireControl(i);
            }*/
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
                
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for(int i = 0; i < bullets.Length; i++)
            {
                if (!bullets[i].Fired) continue;
                color = colors[HelperMethods.Next(colors.Length)];
                spriteBatch.Draw(texture, bullets[i].Position, null, color, 0.0f,Vector2.Zero, .1f, SpriteEffects.None,0.0f);
            }
            /*
            fireTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (fireTimer > .25)
            {
                color = colors[HelperMethods.Next(colors.Length)];
                spriteBatch.Draw(texture, bullets[i].Position, color);
                fireTimer -= .25;
            }
            */
        }
        /*
         While space bar is being pressed,
        every .25 seconds in that press: 
            iteratively update bullet state (not fired => fired)
            draw active bullets flying down range 
                update start pos to current choppa pos when first fired
        
        Questions:
            where to put "holding m button" logic: controller or particle system?
            where to put the timers?  In/out of for loops or in controller?
            do i need two separate timers for update and draw with my current implmentation?
            better way to do the timing for the update?  I know I have to use a timer for the drawing of the bullets...
            
         */
        private void SpawnBullet(Vector2 position)
        {
            position.X += 100;
            position.Y += 87;
            for(int i = 0; i < bullets.Length; i++)
            {
                if(!bullets[i].Fired)
                {
                    bullets[i].Position = position;
                    bullets[i].Velocity = new Vector2(FIRE_VELOCITY, 0);
                    bullets[i].Fired = true;
                    return;
                }
            }
        }
    }
}
