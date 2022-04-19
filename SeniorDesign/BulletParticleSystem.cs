using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using SeniorDesign.ButtonStates;
using Microsoft.Xna.Framework.Content;
using SeniorDesign.Bounding_Regions;

namespace SeniorDesign
{//partition in horizontal 
    public class BulletParticleSystem 
    {
        private Vector2 chopperPos;
        private Texture2D texture;
        private Texture2D boundTexture;
        private int freeIndex;//index of first bullet ready to fire
        private double fireTimer;
        private Color color;
        private bool fired;
        private int firedBullets = 0;//indices 0->firedBullets is Bullets currently in the air
        private const int FIRE_VELOCITY = 3000;
        private Color[] colors = new Color[]
        {
            Color.Red,
            Color.OrangeRed,
            Color.Yellow
        };
        public Particle[] Bullets;

        public bool IsFiring { get; set; }

        public BulletParticleSystem(Vector2 chopperPos)
        {
            this.chopperPos = chopperPos;
            Bullets = new Particle[1000];
            freeIndex = 0;
        }
        /// <summary>
        /// FIXME protected or private?
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("red_laser");
            boundTexture = content.Load<Texture2D>(@"Debugging_Tools\Water32Frames8x4");
        }
        /// <summary>
        /// update position of Bullets ready to fire to chopper
        /// fire Bullets
        /// TAKE INSPIRATION FROM COIN SPRITE IN OTHER GAME
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="originPos"></param>
        public void Update(GameTime gameTime, Vector2 originPos)
        {
            if(IsFiring)
            {
                fireTimer += gameTime.ElapsedGameTime.TotalSeconds;
                //when .13 secs have passed, and user is still holding firing key, spawn another bullet
                if(fireTimer >= 0.13)
                {
                    SpawnBullet(originPos);
                    fireTimer = 0.0;
                }
            }
            else
            {
                fireTimer = 0.13;
            }

            for(int i = 0; i < Bullets.Length; i++)
            {
                if (!Bullets[i].Alive) continue;
                Bullets[i].Position += (float)gameTime.ElapsedGameTime.TotalSeconds * Bullets[i].Velocity;
                Bullets[i].UpdateBounds();
            }
        }
        
        /// <summary>
        /// Draw bullet and bounding region (for debugging)
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for(int i = 0; i < Bullets.Length; i++)
            {
                if (!Bullets[i].Fired || !Bullets[i].Alive) continue;
                color = colors[HelperMethods.Next(colors.Length)];
                spriteBatch.Draw(texture, Bullets[i].Position, null, color, 0.0f,Vector2.Zero, .1f, SpriteEffects.None,0.0f);
                var boundRect = new Rectangle((int)Bullets[i].Bounds.X, (int)Bullets[i].Bounds.Y, (int)Bullets[i].Bounds.Height, (int)Bullets[i].Bounds.Width);
                //spriteBatch.Draw(boundTexture, boundRect, Color.White);
            }
        }
        
        private void SpawnBullet(Vector2 position)
        {
            position.X += 100;
            position.Y += 87;
            for(int i = 0; i < Bullets.Length; i++)
            {
                if(!Bullets[i].Fired)
                {
                    Bullets[i].Position = position;
                    Bullets[i].Velocity = new Vector2(FIRE_VELOCITY, 0);
                    Bullets[i].Fired = true;
                    Bullets[i].Alive = true;
                    Bullets[i].InitializeBounds(position);
                    firedBullets++;
                    return;
                }
                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <param name="isDragonAlive"></param>
        /// <returns></returns>
        public int CollissionChecker(BoundingRectangle other, bool isDragonAlive)
        {
            //if (!isDragonAlive) return 0;
            int hitCount = 0;
            for (int i = 0; i < firedBullets; i++)
            {
                if (Bullets[i].Alive &&  Bullets[i].Bounds.CollidesWith(other))
                {
                    Bullets[i].Alive = false;
                    hitCount++;
                }
            }
            return hitCount;
        }

        private void bulletCheck()
        {
            for (int i = 0; i < firedBullets; i++)
            {
                if(Bullets[i].Position.X > Constants.GAME_WIDTH || Bullets[i].Position.X < 0)
                {
                    Bullets[i].Alive = false;
                }
                if (!Bullets[i].Alive)
                {
                    Bullets[i] = Bullets[firedBullets];
                    firedBullets--;
                }
            }
            
        }
    }
}
