using MonoChess_DesktopApp.Draughts.Enums;
using System.Collections.Generic;

namespace MonoChess_DesktopApp.Draughts.Model
{
    public class KingMove : Move
    {
        public KingMove(int position, PieceType[] pieces, bool endFlag) : base(position, pieces, endFlag) { }

        protected override void AddNextMovesOnDirection(List<Move> listMoves, Direction direction)
        {
            if (CaptureCount == 0)
                AddMoves(listMoves, direction);
            AddCaptureMoves(listMoves, direction);
        }

        protected override Move CreateNextMove(int position, PieceType[] pieces, bool endFlag)
            => new KingMove(position, pieces, endFlag);

        private void AddMoves(List<Move> listMoves, Direction direction)
        {
            var neighbor = GetNeighbor(ToPosition, direction);
            while (neighbor != -1 && Pieces[neighbor] == PieceType.None)
            {
                listMoves.Add(MovePiece(ToPosition, neighbor));
            }
        }

        private void AddCaptureMoves(List<Move> listMoves, Direction direction)
        {
            int enemy = GetNearestEnemy(direction);
            int neighbor = GetNeighbor(enemy, direction);
            while (neighbor != -1 && Pieces[neighbor] == PieceType.None)
            {
                listMoves.Add(CapturePiece(ToPosition, neighbor, enemy));
                neighbor = GetNeighbor(neighbor, direction);
            }
        }

        private int GetNearestEnemy(Direction direction)
        {
            int neighbor = ToPosition;
            do neighbor = GetNeighbor(neighbor, direction); while (neighbor != -1 || !IsEnemy(neighbor));
            return neighbor;
        }
    }
}
