using System.Collections.Generic;

namespace MonoChess_DesktopApp.Draughts
{
    public class Command
    {
        public int EndPosition { get { return _endMove.Position; } }
        private Move _endMove;

        public Command(Move endMove)
        {
            _endMove = endMove;
        }

        public List<Move> GetAllMoves()
        {
            return null;
        }
    }
}