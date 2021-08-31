using MonoChess_DesktopApp.Draughts.Enums;
using System.Collections.Generic;

namespace MonoChess_DesktopApp.Draughts.Model
{
    public class PvtMove : Move
    {
        public PvtMove(int position, PieceType[] pieces, bool endFlag) : base(position, pieces, endFlag) { }

        protected override void AddNextMovesOnDirection(List<Move> listMoves, Direction direction)
        {
            var neighbor = GetNeighbor(ToPosition, direction);
            if (neighbor == -1) return;
            if (Pieces[neighbor] == PieceType.None && CaptureCount == 0)
            {
                listMoves.Add(MovePiece(ToPosition, neighbor));
            }
            else
            {
                var secondNeighbor = GetNeighbor(neighbor, direction);
                if (secondNeighbor != -1 && Pieces[secondNeighbor] == PieceType.None && IsEnemy(neighbor))
                {
                    listMoves.Add(CapturePiece(ToPosition, secondNeighbor, neighbor));
                }
            }
        }

        protected override Move CreateNextMove(int position, PieceType[] pieces, bool endFlag)
            => new PvtMove(position, pieces, endFlag);
    }
}
