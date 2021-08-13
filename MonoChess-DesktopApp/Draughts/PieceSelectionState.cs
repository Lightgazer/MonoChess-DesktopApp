using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace MonoChess_DesktopApp.Draughts
{
    class PieceSelectionState : IDraughtsBoardState
    {
        private readonly DraughtsBoardView _context;
        private readonly BoardCursor _cursor;
        private readonly ContentManager _content;
        private readonly Texture2D _frame;
        private Point[] _activePositions;

        public PieceSelectionState(ContentManager content, DraughtsBoardView context)
        {
            _context = context;
            _content = content;
            _frame = content.Load<Texture2D>("frame");
            _cursor = new BoardCursor(content, context.GetScreenRectangle());
            _cursor.OnSelect += OnSelect;
        }

        public void Init(DraughtsModel model)
        {
            _activePositions = CalculateActivePositions(model.GetActivePieces());
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

        private static Point[] CalculateActivePositions(IReadOnlyList<int> squares)
            => squares.Select(square => DraughtsBoardView.SquareNumberToPoint(square)).ToArray();


        private void OnSelect(Point point)
        {
            if (DraughtsBoardView.PointToSquareNumber(point) is { } index)
            {
                var newState = new ActionSelectionState(_content, _context, index);
                _context.TransitionTo(newState);
            }
        }

        private void DrawActivePositions(SpriteBatch spriteBatch)
        {
            foreach(var point in _activePositions)
            {
                var screenPosition = _context.PointToScreenPosition(point);
                spriteBatch.Draw(_frame, screenPosition.ToVector2(), Color.White);
            }
        }
    }
}
