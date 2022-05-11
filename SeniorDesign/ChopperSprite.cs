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
    
    public class ChopperSprite
    {
        /// <summary>
        /// private backing for hit points
        /// </summary>
        private const int INITIAL_HP = 120;
        private int hitPoints;
        /// <summary>
        ///public get set for hit point values
        /// </summary>
        public int HitPoints => hitPoints;
        private KeyboardState keyboardState;

        private List<MissileSprite> missiles;
        /// <summary>
        /// pixel speed of animation
        /// </summary>
        private const int PIXEL_SPEED = 150;
        /// <summary>
        /// Texture for helicopter
        /// </summary>
        private Texture2D flyingTexture;

        private Texture2D explosionTexture;
        /// <summary>
        /// Texture for testing bounding regions
        /// </summary>
        private Texture2D boundTexture;
        /// <summary>
        /// direction timer.  Times how long chopper moves in certian direction.
        /// </summary>
        //private double directionTimer;
        /// <summary>
        /// Controls timing of animation
        /// </summary>
        private double animationTimer;

        private double explosionTimer;
        /// <summary>
        /// animation frame in image grid
        /// x component
        /// </summary>
        private short animationFrame = 0;//start at one to avoid dead bat frame...
        /// <summary>
        /// row of current animation 
        /// y component
        /// </summary>
        private short animationRow = 0;//like direction 

        //public Direction Direction;
        /// <summary>
        /// private backing variable for Position field
        /// FIXME decide where to set in controller or here
        /// </summary>
        private Vector2 position;
        /// <summary>
        /// Position of chopper
        /// </summary>
        public Vector2 Position => position;
        /// <summary>
        /// Determines if it is firing.
        /// TODO put chopper states into dedicated enum
        /// </summary>
        public bool Firing = false;
        private bool hit = false;
        /// <summary>
        /// Property to detect if missile has hit the chopper
        /// </summary>
        public bool Hit => hit;

        private bool isAlive = true;

        public bool IsAlive => isAlive;
        

        private bool isHit = false;

        private bool isExploding = false;

        private int boomAnimationFrame = 0;

        private int boomAnimationRow = 0;

        /// <summary>
        /// Bounding region for collision detection
        /// </summary>
        private BoundingRectangle bounds;
        public BoundingRectangle Bounds => bounds;

        /// <summary>
        /// length is 256 pixels, rad = 128 pixels
        /// in drawing method, chopper is scaled down by 1/2, so scaled rad = 64
        /// </summary>
        //TODO add bounding params
        public ChopperSprite()
        {
            missiles = new List<MissileSprite>();
            bounds = new BoundingRectangle(position.X, position.Y, 150, 30);
            setPosition();
            hitPoints = INITIAL_HP;
            
        }
        /// <summary>
        /// load content
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            //flyingTexture = content.Load<Texture2D>("Fly");//TODO how to switch to missile firing mid animation frame
            flyingTexture = content.Load<Texture2D>("Choppa_Sprite2");
            boundTexture = content.Load<Texture2D>(@"Debugging_Tools\Water32Frames8x4");
            explosionTexture = content.Load<Texture2D>(@"Explosion_Files\Circle_Boom");

        }
        /// <summary>
        /// Manually resets the chopper from controller
        /// </summary>
        /// <param name="alive"></param>
        public void ResetChopper(bool alive)
        {
            isAlive = alive;
            setPosition();
            hitPoints = INITIAL_HP;
            boomAnimationFrame = 0;
            boomAnimationRow = 0;
        }
        /// <summary>
        /// set chopper position on start or restart of game
        /// </summary>
        private void setPosition()
        {
            position = new Vector2(100, 200);
        }
        /// <summary>
        /// Updates chopper, most notably direction it is traveling
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (boomAnimationRow == 2 && boomAnimationFrame == 2)
            {
                isExploding = false;
                position.X = -600;
                position.Y = 200;
            }

            if (!isAlive)
            {
                isExploding = true;
                //position.X = -800;
                //position.Y = 200;
            }
            if (hitPoints <= 0)
            {
                isAlive = false;
                hitPoints = 0;
            }
            bounds.X = position.X + 16;
            bounds.Y = position.Y + 62;
            keyboardState = Keyboard.GetState();
            if (isAlive)
            {
                //TODO add acceleration to chopper as keys are held down
                if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A)) position += new Vector2((float)-3.5, 0);
                if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D)) position += new Vector2((float)3.5, 0);
                if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W)) position += new Vector2(0,(float) -3.5);
                if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S)) position += new Vector2(0, (float)3.5);
            }
            //FIXME change to only scroll forward on screen, otherwise chopper stops
            if (position.Y < 0) position.Y = 0;
            if (position.Y > Constants.GAME_HEIGHT) position.Y = Constants.GAME_HEIGHT;
            if (position.X < 0) position.X = 0;
            if (position.X > Constants.GAME_WIDTH) position.X = 0;
        }
        /// <summary>
        /// if chopper dies draw explosion
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        private void drawExplosion(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (isExploding)
            {
                explosionTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (explosionTimer > 0.1)//3x3 spritesheet
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
            var sourceRectangle = new Rectangle(boomAnimationFrame * 128, boomAnimationRow * 128, 128, 128);
            if (isExploding)
            {
                spriteBatch.Draw(explosionTexture, new Vector2(position.X+110, position.Y + 60), sourceRectangle, Color.White, 0f, new Vector2(64, 64), 2.5f, SpriteEffects.None, 0);
            }


        }
        /// <summary>
        /// draws chopper
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
            if (!isAlive && !(boomAnimationRow == 2 && boomAnimationFrame == 2)) drawExplosion(gameTime, spriteBatch);

            //3x2 image
            if (!hit)
            {
                //update timer based on elapsed time in game
                //elapsed time = elapsed time since last update
                animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (animationTimer > .05)
                {
                    animationFrame++;
                    //reached end of current row, reset to first pos in next row
                    if (animationFrame > 3)
                    {
                        animationFrame = 0;
                        animationRow++;
                        if (animationRow > 1) animationRow = 0;
                    }
                    animationTimer -= .05;
                }
            }
            //TODO add source rectangle for firing missile
            var sourceRectangle = new Rectangle(animationFrame * 850, animationRow * 381, 850, 381);
            //draw with upadted position and source rectangle
            //spriteBatch.Draw(texture, Position, sourceRectangle, Color.White);
            //TODO difference between position and origin?
            if(isAlive) spriteBatch.Draw(flyingTexture, position, sourceRectangle, Color.White, 0f, new Vector2(0, 0), .25f, SpriteEffects.None, 0);
            var boundRect = new Rectangle((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height);
            //if(isAlive) spriteBatch.Draw(boundTexture, boundRect, Color.White * .2f);

        }

        /// <summary>
        /// checks for collision with single target
        /// </summary>
        /// <param name="other"></param>
        /// <returns>number of times hit in frame</returns>
        public int CollisionChecker(BoundingRectangle other)
        {
            int hitCount = 0;
            if (bounds.CollidesWith(other))
            {
                hitCount++;
                
            }
            return hitCount;
        }
        /// <summary>
        /// detracts hitpoints based on muniton type and number of times hit
        /// </summary>
        /// <param name="hitCount"></param>
        /// <param name="munitionType"></param>
        public void DetractHitPoints(int hitCount, MunitionType munitionType)
        {
            hitPoints -= hitCount * (int)munitionType;
            if (hitPoints < 0) hitPoints = 0;
        }
    }
}
