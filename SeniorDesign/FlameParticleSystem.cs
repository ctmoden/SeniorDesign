using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using SeniorDesign.ButtonStates;
using Microsoft.Xna.Framework.Content;
using SeniorDesign.Bounding_Regions;
using System.Collections.Generic;

namespace SeniorDesign
{
    public static class FlameParticleSystem
    {

        private static List<Vector2> dragonPositions;
        private static Texture2D flameTexture;
        private static Texture2D boundTexture;
        private static byte animationRow = 0;
        private static byte animationFrame = 0;
        private static double animationTimer;
        private static double fireTimer;
        private static Color color;
        private static int firedFlames;
        private static int fireVelocity;
        private static int freeIndex;
        private static Vector2 targetPos;
        public static Particle[] Flames = new Particle[1000];
        public static bool IsFiring;
        private static BoundingRectangle bounds;
        /// <summary>
        /// Standard issue bounding region public getter for collision debugging
        /// </summary>
        public static BoundingRectangle Bounds => bounds;
        

        public static void LoadContent(ContentManager content)
        {
            //FIXME
            flameTexture = content.Load<Texture2D>(@"Explosion_Files\Flames");
            boundTexture = content.Load<Texture2D>(@"Debugging_Tools\Water32Frames8x4");
            dragonPositions = new List<Vector2>();
        }
        /// <summary>
        /// if index is -1,t then add new vector to the list
        /// </summary>
        /// <param name="index"></param>
        /// <param name="newPos"></param>
        public static void UpdateDragonPositions(int index, Vector2 newPos)
        {

        }
        /// <summary>
        /// Call from game controller
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="originPos">position of randomly selected dragon position</param>
        public static void Update(GameTime gameTime, int dragonIndex)
        {
            if (IsFiring)
            {
                fireTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if(fireTimer >= 2.0)
                {
                    SpawnFlame(dragonIndex);
                    fireTimer = 0.0;
                }
            }
            else
            {
                fireTimer = 1.0;
            }
            for(int i = 0; i < Flames.Length; i++)
            {
                if (!Flames[i].Alive) continue;
                Flames[i].Position += (float)gameTime.ElapsedGameTime.TotalSeconds * Flames[i].Velocity;
                Flames[i].UpdateBounds();//FIXME add params to particle class to 
            }
        }

        public static void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for(int i = 0; i < Flames.Length; i++)
            {
                animationTimer += gameTime.TotalGameTime.TotalSeconds;
                if (animationTimer > 1.0)
                {
                    animationFrame++;
                    if (animationFrame > 2)
                    {
                        animationFrame = 0;
                        animationRow++;
                        if (animationRow > 2) animationRow = 0;
                    }
                    animationTimer -= 1.0;
                }//128 * 128
                var sourceRectangle = new Rectangle(animationFrame * 128, animationRow * 128, 128, 128);
                //FIXME later include angle to shift flame towards target
                if(Flames[i].Alive)spriteBatch.Draw(flameTexture, Flames[i].Position, sourceRectangle, Color.White, 0f, new Vector2(64,64),0,SpriteEffects.None,0);
            }
            

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        private static void SpawnFlame(int index)
        {
            //FIXME adjust to accomodate for a list
            dragonPositions[index].X += 50;
            dragonPositions[index].Y += 50;
            for(int i = 0; i < Flames.Length; i++)
            {
                if (!Flames[i].Fired)
                {
                    Vector2 newPosition = new Vector2(dragonPositions[i].X, dragonPositions[i].Y);
                    Flames[i].Position = newPosition;
                    Flames[i].Fired = true;
                    Flames[i].Alive = true;
                    Flames[i].InitializeBounds(newPosition);
                    firedFlames++;
                    return;
                }
            }
        }
        /// <summary>
        /// Checks for collisions with chopper AND bullets(circle back, focus on the chopper first)
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public static int CollisionChecker(BoundingRectangle other)
        {
            int hitCount = 0;
            for(int i = 0; i < firedFlames; i++)
            {
                if(Flames[i].Position.X > Constants.GAME_WIDTH || Flames[i].Position.X < 0)
                {
                    Flames[i].Alive = false;
                    //TO DO check flames here maybe?  Gotta fix that soon... hehe
                    //Flames[i].Fired = false;
                    hitCount++;
                }
                
            }
            return 1;
        }
        /// <summary>
        /// FIXME implement SOMEWHERE AND GET IT TF WORKING
        /// </summary>
        private static void flameCheck()
        {
            for (int i = 0; i < firedFlames; i++)
            {
                if (Flames[i].Position.X > Constants.GAME_WIDTH || Flames[i].Position.X < 0)
                {
                    Flames[i].Alive = false;
                }
                if (!Flames[i].Alive)
                {
                    Flames[i] = Flames[firedFlames];
                    firedFlames--;
                }
            }

        }
        


    }
}
