using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using SeniorDesign.Bounding_Regions;

namespace SeniorDesign
{
    /// <summary>
    /// 28 Feb
    /// have a parent class for missiles?  Can share content, animation
    /// with vector list idea, how can I draw the same sprite at different positions at the same time?
    /// Should fire control belong to the chopper class?  But then I have to somehow deal with game controller
    /// objects for already created missiles (unless I put all that in the chopper sprite class... thats an idea)
    /// For machine gun rounds, would putting a thread to sleep work to make it look like multiple rounds are being fired?)
    /// instead of what looks like a massive "laser" being engaged?
    /// make a separate sprite batch draw
    /// collissions => draw at collision
    /// have a list of things to remove
    /// Each missile needs to have it's own position after launch( and bounding region for collisions)
    /// </summary>
    public class MissileSprite
    {
        /// <summary>
        /// Counts number of missiles supplied to chopper 
        /// </summary>
        private static int missileLoad;//FIXME make this public and reset as soon as 

        private const int FIRE_VELOCITY = 20;//30
        /// <summary>
        /// pixel speed of animation
        /// </summary>
        private const int PIXEL_SPEED = 150;
        /// <summary>
        /// Texture for missile
        /// </summary>
        private Texture2D texture;
        /// <summary>
        /// Texture for bounding regions
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
        private short animationFrame = 0;
        
        private short animationRow = 0;//like direction 

        //public Direction Direction;
        /// <summary>
        /// private backing variable for Position field
        /// FIXME change position generation later 
        /// </summary>
        private Vector2 position;
        /// <summary>
        /// Starting position of the missile
        /// </summary>
        private Vector2 startPosition;
        
        /// <summary>
        /// Position of chopper
        /// </summary>
        //public Vector2 Position => position;
        /// <summary>
        /// determines if missile has fired 
        /// TODO is there a better way to do this with states?
        /// </summary>
        private bool fired = false;
        
        /// <summary>
        /// Public property for fired variable
        /// </summary>
        public bool Fired => fired;
        /// <summary>
        /// sets missile position to most update chopper position before firing
        /// </summary>
        private bool spinUp = false;
        /// <summary>
        /// Bounding region for colissions
        /// </summary>
        private BoundingRectangle bounds;
        public BoundingRectangle Bounds => bounds;

        public bool IsAlive = false;
        /// <summary>
        /// list of positions for all missiles in flight 
        /// </summary>
        /// <summary>
        /// Constructor, sets starting position to that of chopper's
        /// </summary>
        /// <param name="chopperPos">starting position of chopper</param>
        public MissileSprite(Vector2 chopperPos)
        {
            startPosition = chopperPos;
            bounds = new BoundingRectangle(position.X, position.Y, 20, 10);
        }
        public static void SetMissileLoad(int load)
        {
            missileLoad = load;
        }
        //TODO what to do about constructor and chopper position? =>update method!
        /// <summary>
        /// Loads missile content
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Missile");
            boundTexture = content.Load<Texture2D>(@"Debugging_Tools\Water32Frames8x4");

        }
        /// <summary>
        /// Update call for a recently fired missile, is marked as alive
        /// </summary>
        /// <param name="fired">determines if missile has been fired</param>
        /// <param name="origin">chopper position on screen to fire from</param>
        public void Update(bool fired, Vector2 origin)
        {
            resetBounds();
            startPosition = origin;
            startPosition.X += 110;
            startPosition.Y += 80;
            this.fired = fired;
            IsAlive = true;
            if(!this.fired) position = startPosition;                            
        }
        /// <summary>
        /// Update call for missile not just recently fired but is still in the air
        /// </summary>
        /// <param name="origin"></param>
        public void Update(Vector2 origin)
        {
            resetBounds();
            startPosition = origin;
            startPosition.X += 110;
            startPosition.Y += 80;
            if (!fired) position = startPosition;
        }
        /// <summary>
        /// FIXME put logic of 
        /// </summary>
        private void resetBounds()
        {
            bounds.X = position.X + 20;
            bounds.Y = position.Y + 5;
        }
        /// <summary>
        /// Fires missile and moves missile across screen
        /// </summary>
        public void FireControl()
        {
            if (position.X < Constants.GAME_WIDTH && fired)
            {
                position += new Vector2(FIRE_VELOCITY, 0);
            }
            if (position.X >= Constants.GAME_WIDTH && fired)
            {
                position += new Vector2(0, 0);
                missileLoad--;
                //reset missile in case of replenishing
                fired = false;
                IsAlive = false;
                position = startPosition;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (fired && missileLoad > 0 && IsAlive)
            {
                //update timer based on elapsed time in game
                //elapsed time = elapsed time since last update
                animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (animationTimer > .5)
                {
                    animationFrame++;
                    //reached end of current row, reset to first pos in next row
                  if (animationFrame > 2) animationFrame = 0;
                  animationTimer -= .5;
                }
                var sourceRectangle = new Rectangle(animationFrame * 178, 0, 178, 83);
                //draw with upadted position and source rectangle
                //spriteBatch.Draw(texture, Position, sourceRectangle, Color.White);
                spriteBatch.Draw(texture, position, sourceRectangle, Color.White, 0f, new Vector2(0, 0), .32f, SpriteEffects.None, 0);
                var boundRect = new Rectangle((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height);
                //spriteBatch.Draw(boundTexture, boundRect, Color.White);

            }

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
                bounds.X = -2000;
                bounds.Y = -2000;
                position.X = -2000;
                position.Y = -2000;
            }
            return hitCount;
        }
        
    }
}
