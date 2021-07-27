using System;
using System.Collections.Generic;
using System.Linq;
using MonoChess_DesktopApp.Extensions;

namespace MonoChess_DesktopApp.Draughts
{
    enum Direction
    {
        LeftUp,
        LeftDown,
        RightUp,
        RightDown
    }
    
    internal class Move
    {
        public int Position { get; }
        public int Capture { get; set; }
        public PieceType[] Pieces { get; }
        public Move Parent { get; set; }
        public int CaptureCount;
        private readonly bool _endFlag;

        public Move(int position, PieceType[] pieces, bool endFlag)
        {
            Position = position;
            Pieces = pieces;
            _endFlag = endFlag;
        }

        public List<Move> GetNextMoves()
        {
            var listMoves = new List<Move>();
            if (_endFlag) return listMoves;
            
            foreach (var direction in (Direction[]) Enum.GetValues(typeof(Direction)))
            {
                var neighbor = GetNeighbor(Position, direction);
                if (neighbor == -1) continue;
                if (Pieces[neighbor] == PieceType.None && CaptureCount == 0)
                {
                    listMoves.Add(MovePiece(Position, neighbor));
                }
                else
                {
                    var secondNeighbor = GetNeighbor(neighbor, Direction.LeftUp);
                    if (secondNeighbor != -1 && Pieces[secondNeighbor] == PieceType.None && IsEnemy(neighbor))
                    {
                        listMoves.Add(CapturePiece(Position, secondNeighbor, neighbor));
                    }
                }
            }

            return listMoves;
        }

        private static int GetNeighbor(int index, Direction direction)
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

        private Move MovePiece(int from, int to)
        {
            var nextPieces = (PieceType[]) Pieces.Clone();
            var piece = nextPieces[from];
            nextPieces[from] = PieceType.None;
            nextPieces[to] = piece;
            return new Move(to, nextPieces, true) {Parent = this};
        }

        private Move CapturePiece(int from, int to, int target)
        {
            var nextPieces = (PieceType[]) Pieces.Clone();
            var piece = nextPieces[from];
            nextPieces[from] = PieceType.None;
            nextPieces[target] = PieceType.None;
            nextPieces[to] = piece;
            return new Move(to, nextPieces, false)
            {
                Parent = this, CaptureCount = CaptureCount + 1, Capture = target
            };
        }

        private bool IsEnemy(int index)
        {
            var current = Pieces[Position];
            var target = Pieces[index];
            var blackSide = new[] {PieceType.WhitePvt, PieceType.WhiteKing};
            var whiteSide = new[] {PieceType.BlackPvt, PieceType.BlackKing};
            
            return blackSide.Contains(current) && whiteSide.Contains(target) || 
                   whiteSide.Contains(current) && blackSide.Contains(target);
        }
    }
}