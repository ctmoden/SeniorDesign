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
    {
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
        private Vector2 position = new Vector2(400, 400);
        /// <summary>
        /// Position of chopper
        /// </summary>
        public Vector2 Position => position;
        /// <summary>
        /// determines if missile has fired 
        /// </summary>
        private bool fired = false;
        /// <summary>
        /// Loads missile content
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Missile");
        }
        /// <summary>
        /// Updates game mistly based on missile state
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            //set fire variable to true
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                fired = true;
                position += new Vector2(5, 0);
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
                    
                  if (animationFrame > 2) animationRow = 0;
                    
                  animationTimer -= .5;
                }
                var sourceRectangle = new Rectangle(animationFrame * 178, 0, 178, 83);
                //draw with upadted position and source rectangle
                //spriteBatch.Draw(texture, Position, sourceRectangle, Color.White);
                spriteBatch.Draw(texture, position, sourceRectangle, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
            }
        }
    }
}
