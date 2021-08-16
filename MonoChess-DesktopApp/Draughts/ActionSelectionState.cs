using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace MonoChess_DesktopApp.Draughts
{
    class ActionSelectionState : IDraughtsBoardState
    {
        private readonly DraughtsBoardView _context;
        private readonly BoardCursor _cursor;
        private readonly ContentManager _content;
        private readonly Texture2D _frame;
        private Point[] _activePositions;

        private readonly int _index;

        public ActionSelectionState(ContentManager content, DraughtsBoardView context, int index)
        {
            _context = context;
            _content = content;
            _frame = content.Load<Texture2D>("frame");
            _cursor = new BoardCursor(content, context.GetScreenRectangle());
            _cursor.OnSelect += OnSelect;

            _cursor.OnCancel += OnCancel;
            _index = index;
        }

        public void Init(DraughtsModel model)
        {
            _activePositions = CalculateActivePositions(model.GetPossibleActions(_index));
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

        private static Point[] CalculateActivePositions(List<Command> commands)
            => commands.Select(command => DraughtsBoardView.SquareNumberToPoint(command.EndPosition)).ToArray();

        private void OnSelect(Point point)
        {

        }

        private void OnCancel() { }

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
