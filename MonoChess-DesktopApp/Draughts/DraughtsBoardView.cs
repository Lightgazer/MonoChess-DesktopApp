using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoChess_DesktopApp.Draughts
{
    public class DraughtsBoardView
    {
        public Point Position { set; get; }
        
        private readonly Texture2D _lightSquare;
        private readonly Texture2D _darkSquare;
        private readonly int _size;
        
        public DraughtsBoardView(Texture2D darkSquare, Texture2D lightSquare, int size)
        {
            _darkSquare = darkSquare;
            _lightSquare = lightSquare;
            _size = size;
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawCells(spriteBatch);
        }

        private void DrawCells(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 100; i++)
            {
                var blockIndex = new Point(i % _size, i / _size);
                var position = blockIndex * new Point(GameSettings.BlockSize) + Position;
                var isRowEven = blockIndex.Y % 2 == 0;
                var texture = (i + (isRowEven ? 0 : 1)) % 2 == 1 ? _darkSquare : _lightSquare;
                spriteBatch.Draw(texture, position.ToVector2(), Color.White);
            }
        }
    }
}