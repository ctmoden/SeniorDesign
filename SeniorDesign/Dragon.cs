using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using SeniorDesign.Bounding_Regions;
namespace SeniorDesign
{
    
    public class Dragon
    {
        private Texture2D dragonTexture;

        private Texture2D fireTexture;

        private Texture2D boundingTexture;

        private Texture2D explosionTexture;

        private bool isExploding;

        private bool isExploded;

        private double animationTimer;

        private double directionTimer;

        private double flameTimer;

        private double explosionTimer;

        private double fireRate;

        private int animationRow;//y component

        private int animationFrame;//x component

        private int boomAnimationFrame = 0;

        private int boomAnimationRow = 0;

        private Vector2 position;

        //public Vector2 Position => position;

        private const int PIXEL_SPEED = 150;

        //FIXME need a public position?

        private int hitCount;

        private bool resetTimer;

        private double flyTime;

        private double velocityTimer;

        public bool Alive;

        public static int killCount = 0;

        private bool killCounted = false;
        /// <summary>
        /// when hit by a missile, subtract 50-60
        /// when hit by a bullet, subtract 2-3
        /// </summary>
        private int hitPoints = 100;
        public int HitPoints { get { return hitPoints; } set { hitPoints = value; } }
        private int x_pos;
        /// <summary>
        /// public getter for dragon
        /// </summary>
        public Vector2 Position => position;
        private Direction direction;
        /// <summary>
        /// collision bounds of sprite
        /// </summary>
        private BoundingRectangle bounds;
        public BoundingRectangle Bounds => bounds;
        /// <summary>
        /// Constructor takes in animation row to iterate over certain
        /// dragon animation amongst the different types of dragon textures
        /// </summary>
        /// <param name="animationRow"></param>
        public Dragon(int animationRow)
        {
            hitCount = 0;
            this.position = new Vector2(HelperMethods.Next(500, 700), HelperMethods.Next(100,400));
            //this.position = position;
            this.animationRow = animationRow;
            direction = (Direction)(HelperMethods.Next(2, 3+1));
            resetTimer = false;
            x_pos = 400;
            bounds = new BoundingRectangle(new Vector2(position.X, position.Y), 25, 60);
            Alive = true;
            setFireRate();
        }
        public void LoadContent(ContentManager content)
        {
            dragonTexture = content.Load<Texture2D>(@"Dragon_Files\PNG\144x128\flying_dragon-red");//FIXME will this cause issues outside my machine?
            boundingTexture = content.Load<Texture2D>(@"Debugging_Tools\Water32Frames8X4");
            explosionTexture = content.Load<Texture2D>(@"Explosion_Files\blue_explosion");
        }

        private bool spawnFlame(GameTime gameTime)
        {
            flameTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (flameTimer > fireRate)
            {
                setFireRate();
                flameTimer = 0.0;
                return true;
            }
            else return false;

        }
        private void setFireRate()
        {
            //double between ~.5 and 5 ish
            fireRate = HelperMethods.Next(1, 7) * HelperMethods.NextDouble();
        }
        public void Update(GameTime gameTime, out bool spawnFire)
        {
            bounds.X = position.X-35;
            bounds.Y = position.Y-10;
            if (!Alive)//FIXME try getting rid of this since dragon is checked beforehand
            {
                bounds.X = 1000;
                bounds.Y = 1000;
                isExploding = true;
                
            }
            if (hitPoints <= 0)
            {
                Alive = false;
            }
            
            directionTimer += gameTime.ElapsedGameTime.TotalSeconds;
            spawnFire = spawnFlame(gameTime);
            if (!resetTimer)
            {
                flyTime = HelperMethods.NextDouble() * HelperMethods.Next(1, 3);//
                resetTimer = true;
            }
            if(directionTimer > flyTime || position.Y > Constants.GAME_HEIGHT
                || position.Y < 0)
            {
                switch (direction)
                {
                    case Direction.Down:
                        direction = Direction.Up;
                        break;
                    case Direction.Up:
                        direction = Direction.Down;
                        break;
                }
                directionTimer -= flyTime;
                resetTimer = false;
            }
            //change velocity based on a timer, not per frame
            velocityTimer += gameTime.ElapsedGameTime.TotalSeconds;
            //FIXME make velocity change at a random time, just sticking with .25-.5 secs for now
            if(velocityTimer > 0.02)
            {
                switch (direction)//second timer for speed timing
                {
                    //FIXME uncomment positioning after testing
                    case Direction.Down:
                        //position += HelperMethods.RandomYVelGenerator(-2, 0) * PIXEL_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        break;
                    case Direction.Up:
                        //position += HelperMethods.RandomYVelGenerator(1, 3) * PIXEL_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        break;
                }
                velocityTimer -= 0.02;
            }
            
            if(position.Y < 0)
            {
                direction = Direction.Down;
            }
            if(position.Y > Constants.GAME_HEIGHT)
            {
                direction = Direction.Up;
            }
            if (!Alive && !killCounted)
            {
                killCount++;
                killCounted = true;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if(animationTimer > .25)
            {
                animationFrame++;
                if(animationFrame == 2)
                {
                    animationFrame = 0;
                }
                animationTimer -= .25;
            }//144(x) by 128(y)
            var sourceRectangle = new Rectangle(animationFrame * 144, animationRow * 128, 144, 128);
            if(Alive) spriteBatch.Draw(dragonTexture, position, sourceRectangle, Color.White, 0f, new Vector2(72, 64), .75f, SpriteEffects.None,0);
            var debugRect = new Rectangle((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height);
            if (Alive)
            {
                //spriteBatch.Draw(boundingTexture, debugRect, Color.White);
            }
            if (boomAnimationRow == 2 && boomAnimationFrame == 2) isExploded = true;
            if (!Alive && !(boomAnimationRow == 2 && boomAnimationFrame == 2) && !isExploded) drawExplosion(gameTime, spriteBatch);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        private void drawExplosion(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (isExploding)
            {
                explosionTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (explosionTimer > 0.06)//3x3 spritesheet
                {
                    boomAnimationFrame++;
                    if (boomAnimationFrame > 2)
                    {
                        boomAnimationFrame = 0;
                        boomAnimationRow++;
                        if (boomAnimationRow > 2) boomAnimationRow = 0;
                    }
                    explosionTimer = 0.0;
                }
            }
            if (boomAnimationRow == 2 && boomAnimationFrame == 2)
            {
                isExploding = false;
                isExploded = true;
            }
            var sourceRectangle = new Rectangle(boomAnimationFrame * 128, boomAnimationRow * 128, 128, 128);
            if (isExploding)
            {
                spriteBatch.Draw(explosionTexture, new Vector2(position.X, position.Y-10), sourceRectangle, Color.White, 0f, new Vector2(64, 64), 1f, SpriteEffects.None, 0);
            }
        }

        public void DetractHitPoints(int hitCount, MunitionType munitionType)
        {
            hitPoints -= hitCount * (int)munitionType;
            if (hitPoints <= 0)
            {
                Alive = false;
            }
        }
    }
}
