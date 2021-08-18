using Microsoft.Xna.Framework;
using MonoChess_DesktopApp.Extensions;

namespace MonoChess_DesktopApp.Draughts
{
    internal static class PointExtension
    {
        public static int GetIndex(this Point value)
        {
            var (x, y) = value;
            if (y.IsEven() && !x.IsEven() || !y.IsEven() && x.IsEven())
            {
                int col = (x.IsEven() ? x : x - 1) / 2;
                return y * DraughtsConstants.RowLength + col;
            }
            return -1;
        }
    }
}
