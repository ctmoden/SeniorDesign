using System;

namespace SeniorDesign
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new GameController())
                game.Run();
        }
    }
}
