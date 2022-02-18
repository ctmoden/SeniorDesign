﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace SeniorDesign
{
    public enum Direction
    {
        Right = 0,
        Left = 1,
        Up = 2,
        Down = 3
    }
    public class ChopperSprite
    {
        private KeyboardState keyboardState;

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
        private Vector2 position = new Vector2(400, 400);
        /// <summary>
        /// Position of chopper
        /// </summary>
        public Vector2 Position => position;
        /// <summary>
        /// Determines if it is firing.
        /// TODO put chopper states into dedicated enum
        /// </summary>
        public bool Firing = false;
        private bool hit => Hit;
        /// <summary>
        /// Property to detect if missile has hit the chopper
        /// </summary>
        public bool Hit = false;
        /// <summary>
        /// length is 256 pixels, rad = 128 pixels
        /// in drawing method, chopper is scaled down by 1/2, so scaled rad = 64
        /// </summary>
        //TODO add bounding params

        public void LoadContent(ContentManager content)
        {
            //flyingTexture = content.Load<Texture2D>("Fly");//TODO how to switch to missile firing mid animation frame
            fireTexture = content.Load<Texture2D>("Fire Missile");
            flyingTexture = content.Load<Texture2D>("Choppa_Sprite2");
        }
        /// <summary>
        /// Updates chopper, most notably direction it is traveling
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            if (!hit)
            {
                //TODO add acceleration to chopper sa keys are held down
                if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A)) position += new Vector2((float)-3.5, 0);
                if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D)) position += new Vector2((float)3.5, 0);
                if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W)) position += new Vector2(0,(float) -3.5);
                if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S)) position += new Vector2(0, (float)3.5);
            }
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
            spriteBatch.Draw(flyingTexture, position, sourceRectangle, Color.White, 0f, new Vector2(0, 0), .25f, SpriteEffects.None, 0);
        }
    }
}
