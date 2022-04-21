using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using SeniorDesign.ButtonStates;
using Microsoft.Xna.Framework.Content;
using SeniorDesign.Bounding_Regions;

namespace SeniorDesign
{
    public static class FlameParticleSystem
    {
        private static Vector2 [] dragonPositions;
        private static Texture2D flameTexture;
        private static Texture2D boundTexture;
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
            flameTexture = content.Load<Texture2D>(@"Explosion_Files\PNG\Fire\Fire2");
            boundTexture = content.Load<Texture2D>(@"Debugging_Tools\Water32Frames8x4");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="originPos">position of randomly selected dragon position</param>
        public static void Update(GameTime gameTime, int dragonIndex)
        {
            if (IsFiring)
            {
                fireTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if(fireTimer >= 1.0)
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

        private static void SpawnFlame(int index)
        {
            //FIXME adjust later
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
        


    }
}
