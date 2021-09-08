using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoChess_DesktopApp.Draughts.Model;
using System.Collections.Generic;
using System.Linq;

namespace MonoChess_DesktopApp.Draughts.View
{
    public class ActionSelectionState : SelectionState
    {
        private List<Command> _commands;
        private Texture2D _marker;

        public ActionSelectionState(DraughtsBoardView context, int index) : base(context)
        {
            _cursor.OnCancel += OnCancel;
            _commands = context.Model.GetPossibleCommands(index);
            _activePositions = CalculateActivePositions(_commands);
            _marker = context.Content.Load<Texture2D>("marker");
        }

        protected override void OnValidSelection(Point point)
        {
            var command = FindCommand(point);
            _context.State = new PieceMovementState(_context, command);
        }

        protected override void DrawOnHoverSelection(SpriteBatch spriteBatch, Point point)
        {
            var command = FindCommand(point);
            var moves = command.GetAllMoves();
            var from = moves.Pop().ToPosition;
            foreach(var move in moves)
            {
                var to = move.ToPosition;
                CalculateMarkerPositions(from, to)
                    .ForEach(point => spriteBatch.Draw(_marker, DraughtsBoardView.PointToScreenDisplacement(point) + _context.Position.ToVector2(), Color.White));
                from = to;
            }
        }

        private List<Point> CalculateMarkerPositions(int from, int to)
            => CalculateMarkerPositions(DraughtsBoardView.SquareNumberToPoint(from), DraughtsBoardView.SquareNumberToPoint(to));

        private List<Point> CalculateMarkerPositions(Point from, Point to)
        {
            var diff = to - from;
            var direction = new Point(diff.X > 0 ? 1 : -1, diff.Y > 0 ? 1 : -1); 
            var list = new List<Point>();
            Point next = to - direction;
            list.Add(from);
            while (next != from)
            {
                list.Add(next);
                next -= direction;
            }
            return list;
        }

        private static Point[] CalculateActivePositions(List<Command> commands)
            => commands.Select(command => DraughtsBoardView.SquareNumberToPoint(command.EndPosition)).ToArray();

        private void OnCancel()
            => _context.State = new PieceSelectionState(_context);
        
        private Command FindCommand(Point point) 
            => _commands.Find(command => command.EndPosition == point.GetIndex());
    }
}
