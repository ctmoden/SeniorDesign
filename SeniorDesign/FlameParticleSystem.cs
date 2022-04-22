using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using SeniorDesign.ButtonStates;
using Microsoft.Xna.Framework.Content;
using SeniorDesign.Bounding_Regions;
using System.Collections.Generic;
using System.Linq;

namespace SeniorDesign
{
    public static class FlameParticleSystem
    {

        private static List<Vector2> dragonPositions;
        private static Texture2D flameTexture;
        private static Texture2D boundTexture;
        private static int animationRow = 0;
        private static int animationFrame = 0;
        private static double animationTimer;
        private static double fireTimer;
        private static Color color;
        private static int firedFlames;
        private static int fireVelocity;
        private static int freeIndex;
        private static Vector2 targetPos;
        public static Particle[] Flames = new Particle[100];
        public static bool IsFiring;
        private static BoundingRectangle bounds;
        /// <summary>
        /// Standard issue bounding region public getter for collision debugging
        /// </summary>
        public static BoundingRectangle Bounds => bounds;
        
        /// <summary>
        /// Loads from controller just fine
        /// </summary>
        /// <param name="content"></param>
        public static void LoadContent(ContentManager content)
        {
            //FIXME
            flameTexture = content.Load<Texture2D>(@"Explosion_Files\Flames");
            boundTexture = content.Load<Texture2D>(@"Debugging_Tools\Water32Frames8x4");
            
        }
        public static void Initialize()
        {
            dragonPositions = new List<Vector2>();
            IsFiring = true;
        }
        /// <summary>
        /// Call from game controller
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="originPos">position of randomly selected dragon position</param>
        public static void Update(GameTime gameTime, Vector2 dragonPos)
        {
            #region firing based on state
            /*if (IsFiring)
            {
                fireTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if(fireTimer >= 2.0)
                {                   
                    SpawnFlame(dragonPositions.IndexOf(dragonPos));
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
            }*/
            #endregion

            fireTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if(fireTimer > 2.0)
            {
                SpawnFlame(dragonPositions.IndexOf(dragonPos));
                fireTimer = 0.0;
            }
            else
            {
                fireTimer = 2.0;

            }
            for (int i = 0; i < Flames.Length; i++)
            {
                if (!Flames[i].Alive) continue;
                Flames[i].Position -= (float)gameTime.ElapsedGameTime.TotalSeconds * Flames[i].Velocity;
                Flames[i].UpdateBounds();//FIXME add params to particle class to 
            }
        }

        public static void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            #region timer drawing
            /*
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
                if (Flames[i].Alive)
                {
                    spriteBatch.Draw(flameTexture, Flames[i].Position, sourceRectangle, Color.White, 0f, new Vector2(64, 64), 0, SpriteEffects.None, 0);
                }
            }*/
            #endregion
            animationTimer += gameTime.TotalGameTime.TotalSeconds;
            if (animationTimer > 3.0)
            {
                animationFrame++;
                if (animationFrame > 2)
                {
                    animationFrame = 0;
                    animationRow++;
                    if (animationRow > 1) animationRow = 0;
                }
                animationTimer -= 3.0;
            }//128 * 128
            var sourceRectangle = new Rectangle(animationFrame * 128, animationRow * 128, 128, 128);
            for (int i = 0; i < firedFlames; i++)
            {
                if (Flames[i].Alive)
                {
                    spriteBatch.Draw(flameTexture, Flames[i].Position, sourceRectangle, Color.White, 0f, new Vector2(64, 64), .25f, SpriteEffects.FlipHorizontally, 0);
                }
            }

        }
        /// <summary>
        /// Spawns flame at current position of certain dragon
        /// </summary>
        /// <param name="index"></param>
        private static void SpawnFlame(int index)
        {
            //LOL let's see if this bs works
            var tempArray = dragonPositions.ToArray();
            tempArray[index].X -= 30;
            tempArray[index].Y -= 12;
            dragonPositions = tempArray.ToList();

            for(int i = 0; i < Flames.Length; i++)
            {
                if (!Flames[i].Fired)
                {
                    //CHECK see if I am grabbing positions right
                    Vector2 newPosition = new Vector2(dragonPositions[index].X, dragonPositions[index].Y);
                    Flames[i].Position = newPosition;
                    //Flames[i].Fired = true;
                    //Flames[i].Alive = true;
                    Flames[i].Initialize();
                    Flames[i].InitializeBounds(newPosition);
                    firedFlames++;
                    return;
                }
            }
        }
        /// <summary>
        /// deletes certain dragon position
        /// </summary>
        /// <param name="index"></param>
        public static void DeleteDragonPos(int index)
        {

        }
        /// <summary>
        /// updates dragon position at certain index
        /// FIXME how will dragon update 
        /// </summary>
        /// <param name="newPos"></param>
        public static void UpdateDragonPos(Vector2 newPos, int targetIndex)
        {
            //dragonPositions[index] = newPos;
            //I can senese issues with this as positions are deleted when dragon dies...
            dragonPositions[targetIndex] = newPos;
        }
        /// <summary>
        /// adds new dragon position to list of dragon positions
        /// </summary>
        /// <param name="pos"></param>
        public static void AddNewDragonPos(Vector2 pos)
        {
            //dragonPositions.Append(pos);
            dragonPositions.Add(pos);
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
            return hitCount;
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
