using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonoChess_DesktopApp.Draughts
{
    internal class PieceMovementState : IDraughtsBoardState
    {
        private readonly DraughtsBoardView _context;
        private readonly Stack<Move> _moves;
        private Piece _piece;
        private Move _from;

        public PieceMovementState(DraughtsBoardView context, Command command)
        {
            _context = context;
            _moves = command.GetAllMoves();
            context.Model.Execute(command);

            _from = _moves.Pop();
            var position = DraughtsBoardView.SquareNumberToPoint(_from.ToPosition);
            _piece = context.Pieces.Find(p => p.BoardIndex == position);
        }

        public void Update(GameTime gameTime)
        {
            if (_piece.ScreenDisplacement == Vector2.Zero)
                NextDestination();

            var delta = GameSettings.MovementSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            _piece.ScreenDisplacement = MyMath.MoveTowards(_piece.ScreenDisplacement, Vector2.Zero, delta);

        }

        public void Draw(SpriteBatch spriteBatch) { }

        private void NextDestination()
        {
            if (_moves.Count > 0)
            {
                var move = _moves.Pop();
                var startPosition = DraughtsBoardView.SquareNumberToPoint(_from.ToPosition);
                var targetPosition = DraughtsBoardView.SquareNumberToPoint(move.ToPosition);
                var indexDisplacement = startPosition - targetPosition;

                _piece.BoardIndex = targetPosition;
                _piece.ScreenDisplacement = indexToScreenDisplacement(indexDisplacement);
                _from = move;
            }
            else
            {
                _context.StartNewTurn();
            }
        }

        private Vector2 indexToScreenDisplacement(Point index)
            => (GameSettings.BlockPoint * index).ToVector2();
    }
}
