using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace SeniorDesign
{
    
    /// <summary>
    /// Reads in highscore data for the game from a txt file
    /// </summary>
    public static class FileReader
    {
        private static string data;
        private static string fileName;
        private static string[] line;
        private static double bestTime;
        private static int dragonsKilled;
        
        /// <summary>
        /// Sets filename to be read from
        /// </summary>
        /// <param name="name"></param>
        public static void SetFileName(string name)
        {
            fileName = name;
        }
        public static double GetBestTime()
        {
            return bestTime;
        }
        /// <summary>
        /// Gets dragons killed
        /// </summary>
        /// <returns></returns>
        public static int GetDragonsKilled()
        {
            return dragonsKilled;
        }
        /// <summary>
        /// Reads in high score data
        /// gameTime,dragonsKilled
        /// </summary>
        public static void ReadFile(ContentManager content)
        {
            data = File.ReadAllText(Path.Join(content.RootDirectory, fileName));
            line = data.Split(',');
            bestTime = Double.Parse(line[0]);
            dragonsKilled = Int32.Parse(line[1]);
        }
        /// <summary>
        /// Writes new high score data to the file
        /// </summary>
        /// <param name="newHighScore"></param>
        /// <param name="newGameTime"></param>
        public static void WriteHighScoreInfo(ContentManager content, double newBestTime, double newDragonKillCount)
        {
            string writeText = newBestTime.ToString().Trim()+ ","+newDragonKillCount.ToString().Trim();
            File.WriteAllText(Path.Join(content.RootDirectory, fileName), writeText);           
        }

    }
}
