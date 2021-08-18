using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.Linq;

namespace MonoChess_DesktopApp.Draughts
{
    internal class ActionSelectionState : SelectionState, IDraughtsBoardState
    {
        private readonly int _index;
        private List<Command> _commands;

        public ActionSelectionState(ContentManager content, DraughtsBoardView context, int index) : base (content, context)
        {
            _cursor.OnCancel += OnCancel;
            _index = index;
        }

        public void Init(DraughtsModel model)
        {
            _commands = model.GetPossibleCommands(_index);
            _activePositions = CalculateActivePositions(_commands);
        }

        protected override void OnValidSelection(Point index)
        {
            var command = _commands.Find(command => command.EndPosition == index.GetIndex());
        }

        private static Point[] CalculateActivePositions(List<Command> commands)
            => commands.Select(command => DraughtsBoardView.PointFromSquareNumber(command.EndPosition)).ToArray();


        private void OnCancel() 
        {
            _context.TransitionTo(new PieceSelectionState(_content, _context));
        }
    }
}
