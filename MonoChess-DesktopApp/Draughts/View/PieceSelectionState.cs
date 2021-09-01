﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace MonoChess_DesktopApp.Draughts.View
{
    public class PieceSelectionState : SelectionState, IDraughtsBoardState
    {
        public PieceSelectionState(DraughtsBoardView context) : base(context)
        {
            _activePositions = CalculateActivePositions(context.Model.GetActivePieces());
        }

        protected override void OnValidSelection(Point point)
        {
            var newState = new ActionSelectionState(_context, point.GetIndex());
            _context.State = newState;
        }

        private static Point[] CalculateActivePositions(IReadOnlyList<int> squares)
            => squares.Select(square => DraughtsBoardView.SquareNumberToPoint(square)).ToArray();
    }
}