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

        public static int Next(int max) => rand.Next(max);

        public static int Next(int min, int max) => rand.Next(min, max);

        public static double NextDouble() => rand.NextDouble();

        
    }
}
