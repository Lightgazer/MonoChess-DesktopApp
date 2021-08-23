using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.Linq;

namespace MonoChess_DesktopApp.Draughts
{
    internal class PieceSelectionState : SelectionState, IDraughtsBoardState
    {
        public PieceSelectionState(ContentManager content, DraughtsBoardView context) : base(content, context)
        {
            _activePositions = CalculateActivePositions(context.Model.GetActivePieces());
        }

        protected override void OnValidSelection(Point point)
        {
            var newState = new ActionSelectionState(_content, _context, point.GetIndex());
            _context.State = newState;
        }

        private static Point[] CalculateActivePositions(IReadOnlyList<int> squares)
            => squares.Select(square => DraughtsBoardView.SquareNumberToPoint(square)).ToArray();
    }
}
