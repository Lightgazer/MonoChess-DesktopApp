using MonoChess_DesktopApp.Draughts.Enums;
using System.Collections.Generic;
using System.Linq;

namespace MonoChess_DesktopApp.Draughts.Model
{
    public class Command
    {
        public int EndPosition => _endMove.ToPosition;
        private readonly Move _endMove;

        public Command(Move endMove)
        {
            _endMove = endMove;
        }

        public PieceType[] GetResultPieces()
        {
            var pieces = _endMove.Pieces
                .Select(piece => piece == PieceType.Captured ? PieceType.None : piece)
                .ToArray();

            TryCrown(pieces);
            return pieces;
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

        private void TryCrown(PieceType[] pieces)
        {
            var position = _endMove.ToPosition;
            var pretender = pieces[position];
            var side = pretender.GetSide();
            if (side == Side.White && position < DraughtsConstants.RowLength)
            {
                pieces[position] = PieceType.WhiteKing;

            } else if(side == Side.Black && DraughtsConstants.NumberOfPositions - DraughtsConstants.RowLength < position)
            {
                pieces[position] = PieceType.BlackKing;
            }
        }
    }
}