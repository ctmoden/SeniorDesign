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
        private int hitPoints = 100;
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
        /// <summary>
        /// Texture for helicopter firing missile
        /// </summary>
        private Texture2D fireTexture;
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
        private Vector2 position = new Vector2(100, 200);
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

        private bool IsAlive = true;

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
            
        }
        public void LoadContent(ContentManager content)
        {
            //flyingTexture = content.Load<Texture2D>("Fly");//TODO how to switch to missile firing mid animation frame
            fireTexture = content.Load<Texture2D>("Fire Missile");//FIXME still need to do this
            flyingTexture = content.Load<Texture2D>("Choppa_Sprite2");
            boundTexture = content.Load<Texture2D>(@"Debugging_Tools\Water32Frames8x4");

        }
        /// <summary>
        /// Updates chopper, most notably direction it is traveling
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            bounds.X = position.X + 16;
            bounds.Y = position.Y + 62;
            keyboardState = Keyboard.GetState();
            if (!hit)
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

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
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
            spriteBatch.Draw(flyingTexture, position, sourceRectangle, Color.White, 0f, new Vector2(0, 0), .25f, SpriteEffects.None, 0);
            var boundRect = new Rectangle((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height);
            spriteBatch.Draw(boundTexture, boundRect, Color.White * .2f);

        }

        /// <summary>
        /// checks for collision with single target
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CollisionChecker(BoundingRectangle other)
        {
            int hitCount = 0;
            if (bounds.CollidesWith(other))
            {
                hitCount++;
                IsAlive = false;
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
        }
    }
}
