using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace MonoChess_DesktopApp.Draughts
{
    abstract internal class SelectionState
    {
        protected readonly DraughtsBoardView _context;
        protected readonly BoardCursor _cursor;
        protected readonly Texture2D _frame;
        protected Point[] _activePositions;

        public SelectionState(DraughtsBoardView context)
        {
            _context = context;
            var content = context.Content;
            _frame = content.Load<Texture2D>("frame");
            _cursor = new BoardCursor(content, context.GetScreenRectangle());
            _cursor.OnSelect += OnSelect;
        }

        public void Update(GameTime gameTime)
        {
            _cursor.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawActivePositions(spriteBatch);
            _cursor.Draw(spriteBatch);
        }

        protected abstract void OnValidSelection(Point point);

        private void OnSelect(Point point)
        {
            if (_activePositions.Contains(point))
            {
                OnValidSelection(point);
            }
        }

        private void DrawActivePositions(SpriteBatch spriteBatch)
        {
            foreach (var point in _activePositions)
            {
                var screenPosition = _context.PointToScreenPosition(point);
                spriteBatch.Draw(_frame, screenPosition.ToVector2(), Color.White);
            }
        }
    }
}
