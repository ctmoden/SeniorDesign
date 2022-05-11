using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SeniorDesign
{
    public static class HelperMethods
    {
        static Random rand = new Random();
        /// <summary>
        /// returns new random vector for positioning of sprites on game screen 
        /// </summary>
        /// <returns></returns>
        public static Vector2 RandomVectGenerator()
        {

            return new Vector2((float)rand.NextDouble() * Constants.GAME_WIDTH, (float)rand.NextDouble() * Constants.GAME_WIDTH - Constants.GAME_WIDTH);

        }
        /// <summary>
        /// Generates a vector with a randomly generated y component between min and max
        /// for modulating the y component velocity of a sprite
        /// </summary>
        /// <param name="min">min for random number gen</param>
        /// <param name="max">max for random number gen</param>
        /// <returns></returns>
        public static Vector2 RandomYVelGenerator(int min, int max)
        {
            //chooses random y velocity of sprite
            int randVel = rand.Next(min, max);
            return new Vector2(0, randVel);
        }
        /// <summary>
        /// generates random int from 0 to max-1
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Next(int max) => rand.Next(max);
        /// <summary>
        /// generates random int within range of min to max-1
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Next(int min, int max) => rand.Next(min, max);
        /// <summary>
        /// generates random double from 0.0 to 1.0
        /// </summary>
        /// <returns></returns>
        public static double NextDouble() => rand.NextDouble();

        
    }
}
