using MonoChess_DesktopApp.Draughts.Enums;
using MonoChess_DesktopApp.Draughts.Model;
using System.Collections.Generic;

namespace MonoChess_DesktopApp.Draughts
{
    public class Command
    {
        public int EndPosition => _endMove.ToPosition;
        public PieceType[] EndPieces => _endMove.Pieces;
        private readonly Move _endMove;

        public Command(Move endMove)
        {
            _endMove = endMove;
        }

        public Stack<Move> GetAllMoves()
        {
            var stack = new Stack<Move>();
            var move = _endMove;
            while (move is { })
            {
                stack.Push(move);
                move = move.Parent;
            }
            return stack;
        }
    }
}