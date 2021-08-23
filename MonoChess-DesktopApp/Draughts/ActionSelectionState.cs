using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.Linq;

namespace MonoChess_DesktopApp.Draughts
{
    internal class ActionSelectionState : SelectionState, IDraughtsBoardState
    {
        private List<Command> _commands;

        public ActionSelectionState(ContentManager content, DraughtsBoardView context, int index) : base(content, context)
        {
            _cursor.OnCancel += OnCancel;
            _commands = context.Model.GetPossibleCommands(index);
            _activePositions = CalculateActivePositions(_commands);
        }

        protected override void OnValidSelection(Point point)
        {
            var command = _commands.Find(command => command.EndPosition == point.GetIndex());
            _context.TransitionTo(new PieceMovementState(_content, _context, command));
        }

        private static Point[] CalculateActivePositions(List<Command> commands)
            => commands.Select(command => DraughtsBoardView.SquareNumberToPoint(command.EndPosition)).ToArray();


        private void OnCancel()
        {
            _context.TransitionTo(new PieceSelectionState(_content, _context));
        }
    }
}
