using Microsoft.Xna.Framework;
using MonoChess_DesktopApp.Draughts.Model;
using System.Collections.Generic;
using System.Linq;

namespace MonoChess_DesktopApp.Draughts.View
{
    public class ActionSelectionState : SelectionState
    {
        private List<Command> _commands;

        public ActionSelectionState(DraughtsBoardView context, int index) : base(context)
        {
            _cursor.OnCancel += OnCancel;
            _commands = context.Model.GetPossibleCommands(index);
            _activePositions = CalculateActivePositions(_commands);
        }

        protected override void OnValidSelection(Point point)
        {
            var command = _commands.Find(command => command.EndPosition == point.GetIndex());
            _context.State = new PieceMovementState(_context, command);
        }

        private static Point[] CalculateActivePositions(List<Command> commands)
            => commands.Select(command => DraughtsBoardView.SquareNumberToPoint(command.EndPosition)).ToArray();


        private void OnCancel()
        {
            _context.State = new PieceSelectionState(_context);
        }
    }
}
