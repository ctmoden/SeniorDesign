﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Threading;
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
        private static int missileLoad = 3;//FIXME make this public and reset as soon as 

        public static int MissileLoad => missileLoad;
        private const int FIRE_VELOCITY = 3;
        /// <summary>
        /// pixel speed of animation
        /// </summary>
        private const int PIXEL_SPEED = 150;
        /// <summary>
        /// Texture for missile
        /// </summary>
        private Texture2D texture;
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
        /// private backing variable for missile expended state
        /// </summary>
        private bool expended = false;
        /// <summary>
        /// Determines of missile is expended 
        /// </summary>
        public bool Expended => expended;
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
        /// list of positions for all missiles in flight 
        /// </summary>
        /// <summary>
        /// Constructor, sets starting position to that of chopper's
        /// </summary>
        /// <param name="chopperPos">starting position of chopper</param>
        public MissileSprite(Vector2 chopperPos)
        {
            startPosition = chopperPos;
        }
        //TODO what to do about constructor and chopper position? =>update method!
        /// <summary>
        /// Loads missile content
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Missile");
        }
        /// <summary>
        /// TODO add deconstructor for spent missile?
        /// nned an updated chopper position each update
        /// when space bar is hit, draw missile starting from chopper
        /// each subsequent update doesn not reset missile position to chopper
        /// FIXME might want to add offset params
        /// </summary>
        /// <param name="fired">determines if missile has been fired</param>
        /// <param name="origin">chopper position on screen to fire from</param>
        public void Update(bool fired, Vector2 origin)
        {            
            startPosition = origin;
            startPosition.X += 110;
            startPosition.Y += 80;
            this.fired = fired;
            if(!this.fired) position = startPosition;                            
        }

        public void Update(Vector2 origin)
        {
            startPosition = origin;
            startPosition.X += 110;
            startPosition.Y += 80;
            if (!fired) position = startPosition;
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
            if (fired && missileLoad > 0)
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
                spriteBatch.Draw(texture, position, sourceRectangle, Color.White, 0f, new Vector2(0, 0), .25f, SpriteEffects.None, 0);
            }
            
        }
        
    }
}
