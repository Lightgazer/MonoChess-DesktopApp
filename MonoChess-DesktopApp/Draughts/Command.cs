using System.Collections.Generic;

namespace MonoChess_DesktopApp.Draughts
{
    public class Command
    {
        public int EndPosition { get { return _endMove.ToPosition; } }
        private Move _endMove;

        public Command(Move endMove)
        {
            _endMove = endMove;
        }

        public Stack<Move> GetAllMoves()
        {
            var list = new Stack<Move>();
            var move = _endMove;
            while (move is { })
            {
                list.Push(move);
                move = move.Parent;
            }
            return list;
        }
    }
}