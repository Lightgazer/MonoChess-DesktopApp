using System;
using System.Collections.Generic;
using MonoChess_DesktopApp.Draughts.Enums;
using MonoChess_DesktopApp.Extensions;

namespace MonoChess_DesktopApp.Draughts.Model
{
    public abstract class Move
    {
        public int ToPosition { get; }
        public PieceType[] Pieces { get; }
        public Move Parent { get; private set; }
        public int CaptureCount;
        protected readonly bool _endFlag;

        public Move(int position, PieceType[] pieces, bool endFlag)
        {
            ToPosition = position;
            Pieces = pieces;
            _endFlag = endFlag;
        }

        public List<Move> GetNextMoves()
        {
            var listMoves = new List<Move>();
            if (_endFlag) return listMoves;

            foreach (var direction in (Direction[])Enum.GetValues(typeof(Direction)))
            {
                AddNextMovesOnDirection(listMoves, direction);
            }

            return listMoves;
        }

        public int GetStartOfMovement()
            => Parent?.GetStartOfMovement() ?? ToPosition;

        protected static int GetNeighbor(int index, Direction direction)
        {
            var row = index / DraughtsConstants.RowLength;
            var col = index % DraughtsConstants.RowLength;
            if ((direction == Direction.LeftUp || direction == Direction.RightUp) && row == 0)
                return -1;
            if ((direction == Direction.LeftDown || direction == Direction.RightDown)
                && row == DraughtsConstants.BoardSize - 1)
                return -1;
            if ((direction == Direction.LeftUp || direction == Direction.LeftDown) && !row.IsEven() && col == 0)
                return -1;
            if ((direction == Direction.RightUp || direction == Direction.RightDown)
                && row.IsEven() && col == DraughtsConstants.RowLength - 1)
                return -1;

            return direction switch
            {
                Direction.LeftUp => row.IsEven() ? index - 5 : index - 6,
                Direction.LeftDown => row.IsEven() ? index + 5 : index + 4,
                Direction.RightUp => row.IsEven() ? index - 4 : index - 5,
                Direction.RightDown => row.IsEven() ? index + 6 : index + 5,
                _ => -1
            };
        }

        protected abstract void AddNextMovesOnDirection(List<Move> listMoves, Direction direction);

        protected Move MovePiece(int from, int to)
        {
            var nextPieces = (PieceType[])Pieces.Clone();
            nextPieces[to] = nextPieces[from];
            nextPieces[from] = PieceType.None;
            var nextMove = CreateNextMove(to, nextPieces, true);
            nextMove.Parent = this;
            return nextMove;
        }

        protected Move CapturePiece(int from, int to, int target)
        {
            var nextPieces = (PieceType[])Pieces.Clone();
            nextPieces[to] = nextPieces[from];
            nextPieces[from] = PieceType.None;
            nextPieces[target] = PieceType.Captured;
            var nextMove = CreateNextMove(to, nextPieces, false);
            nextMove.Parent = this;
            nextMove.CaptureCount = CaptureCount + 1;
            return nextMove;
        }

        protected bool IsEnemy(int index)
        {
            var current = Pieces[ToPosition];
            var target = Pieces[index];
            return target != PieceType.None && target != PieceType.Captured && current.GetSide() != target.GetSide();
        }

        protected abstract Move CreateNextMove(int position, PieceType[] pieces, bool endFlag);
    }
}