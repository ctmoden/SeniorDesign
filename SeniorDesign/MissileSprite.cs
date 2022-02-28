using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace SeniorDesign
{
    public class MissileSprite
    {//add list of vectors for missile positions
        private KeyboardState keyboardState;

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
        /// Position of chopper
        /// </summary>
        //public Vector2 Position => position;
        /// <summary>
        /// determines if missile has fired 
        /// TODO is there a better way to do this with states?
        /// </summary>
        private bool fired = false;
        /// <summary>
        /// sets missile position to most update chopper position before firing
        /// </summary>
        private bool spinUp = false;
        /// <summary>
        /// 
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
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="chopperPos"></param>
        public void Update(GameTime gameTime, Vector2 chopperPos)
        {
            startPosition = chopperPos;
            keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Space)&& !fired)//ways of detecting a single click and waiting until a certain condition?
            {
                fired = true;
                spinUp = true;//it's like I need to lock a thread (well same concept)
            }
            if (spinUp)
            {
                if (position.X < startPosition.X)
                    position = startPosition;

                    spinUp = false;
            }
            if (position.X < Constants.GAME_WIDTH && fired)
            {
                position += new Vector2(5, 0);
            }
            if (position.X >= Constants.GAME_WIDTH)
            {
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
            if (fired)
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
            /*
             AI needs to be aware of the world.  Besides 
            heat source? 
             */
        }
    }
}
