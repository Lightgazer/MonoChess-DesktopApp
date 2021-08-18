using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.Linq;

namespace MonoChess_DesktopApp.Draughts
{
    internal class PieceSelectionState : SelectionState, IDraughtsBoardState
    {
        public PieceSelectionState(ContentManager content, DraughtsBoardView context) : base(content, context) { }

        public void Init(DraughtsModel model)
        {
            _activePositions = CalculateActivePositions(model.GetActivePieces());
        }

        protected override void OnValidSelection(Point index)
        {
            var newState = new ActionSelectionState(_content, _context, index.GetIndex());
            _context.TransitionTo(newState);
        }

        private static Point[] CalculateActivePositions(IReadOnlyList<int> squares)
            => squares.Select(square => DraughtsBoardView.PointFromSquareNumber(square)).ToArray();
    }
}
