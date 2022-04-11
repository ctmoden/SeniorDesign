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

        private double animationTimer;

        private double directionTimer;

        private int animationRow;//y component

        private short animationFrame;//x component

        private Vector2 position;

        private const int PIXEL_SPEED = 150;

        //FIXME need a public position?

        private int hitCount;

        private bool resetTimer;

        private double flyTime;

        private double velocityTimer;

        public bool Alive;
        /// <summary>
        /// when hit by a missile, subtract 50-60
        /// when hit by a bullet, subtract 2-3
        /// </summary>
        private int hitPoints = 100;
        public int HitPoints => hitPoints;
        private int x_pos;

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
            position = new Vector2(HelperMethods.Next(500, 700), HelperMethods.Next(200,500));
            this.animationRow = animationRow;
            direction = (Direction)(HelperMethods.Next(2, 3+1));
            resetTimer = false;
            x_pos = 700;
            bounds = new BoundingRectangle(new Vector2(position.X, position.Y), 60, 20);

        }
        public void LoadContent(ContentManager content)
        {
            dragonTexture = content.Load<Texture2D>(@"Dragon_Files\PNG\144x128\flying_dragon-red");//FIXME will this cause issues outside my machine?
            boundingTexture = content.Load<Texture2D>(@"Debugging_Tools\Water32Frames8X4");
            //FIXME load fire texture
        }
        public void Update(GameTime gameTime)
        {
            bounds.X = position.X-35;
            bounds.Y = position.Y-10;
            /*
             for a random period of time,
            move at random velocity either up on down
            imediately switch direction of it starts to go out of bounds (how to pair with direction timer? reset it)
            NEXT STEP: make it oscillate randomly in x coordinate
             */
            directionTimer += gameTime.ElapsedGameTime.TotalSeconds;
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
                        //position += HelperMethods.RandomYVelGenerator(1, 3) * PIXEL_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        //position = new Vector2(x_pos, 10);//reconsider this after second timer
                        break;
                    case Direction.Up:
                        direction = Direction.Down;
                        //position += HelperMethods.RandomYVelGenerator(-2, 0) * PIXEL_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        //position = new Vector2(x_pos, Constants.GAME_HEIGHT - 10);
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
                    case Direction.Down:
                        position += HelperMethods.RandomYVelGenerator(-2, 0) * PIXEL_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        break;
                    case Direction.Up:
                        position += HelperMethods.RandomYVelGenerator(1, 3) * PIXEL_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        break;
                }
                velocityTimer -= 0.02;
            }
            
            if(position.Y < 0)
            {
                //position = new Vector2(x_pos, 10);//move to case statement
                direction = Direction.Down;
            }
            if(position.Y > Constants.GAME_HEIGHT)
            {
                //position = new Vector2(x_pos, Constants.GAME_HEIGHT - 10);
                direction = Direction.Up;
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
            spriteBatch.Draw(dragonTexture, position, sourceRectangle, Color.White, 0f, new Vector2(72, 64), .75f, SpriteEffects.None,0);
            var debugRect = new Rectangle((int)bounds.X, (int)bounds.Y, (int)bounds.Height, (int)bounds.Width);
            spriteBatch.Draw(boundingTexture, debugRect, Color.White);

        }
    }
}
