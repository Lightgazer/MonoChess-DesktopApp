using Microsoft.Xna.Framework;

namespace MonoChess_DesktopApp
{
    internal  static class GameSettings
    {
        public const int Width = 900;
        public const int Height = 700;
        public const int BlockSize = 64;
        public static Point BlockPoint = new Point(BlockSize);
    }
}