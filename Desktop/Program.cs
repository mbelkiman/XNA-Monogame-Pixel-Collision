using System;
using XNA_Monogame_Pixel_Collision;

namespace Desktop
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            using var game = new GameTester();
            game.Run();
        }
    }
}
