using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoChess_DesktopApp.Draughts
{
    internal class Turn
    {
        public readonly PieceType[] Pieces;
        public readonly Side Side;
        private List<Move> _allowedMoves;

        public Turn(PieceType[] pieces, Side side)
        {
            Side = side;
            Pieces = pieces;
        }

        public List<Move> GetAllowedMoves()
        {
            if (_allowedMoves is { }) return _allowedMoves;
            return _allowedMoves = CalculateAllowedMoves();
        }

        private List<Move> CalculateAllowedMoves()
        {
            var currentTurnPieces = Side switch
            {
                Side.White => SelectPieceIndexes(new[] { PieceType.WhitePvt, PieceType.WhiteKing }),
                Side.Black => SelectPieceIndexes(new[] { PieceType.BlackPvt, PieceType.BlackKing }),
                _ => throw new ArgumentOutOfRangeException()
            };
            var movesList = currentTurnPieces
                .Select(index => new Move(index, Pieces, false))
                .ToList();

            while (true)
            {
                var newList = movesList
                    .Select(move => move.GetNextMoves())
                    .SelectMany(list => list)
                    .ToList();
                if (newList.Count == 0) break;
                movesList = newList;
            }

            return movesList
                .GroupBy(move => move.CaptureCount)
                .OrderBy(move => move.First().CaptureCount)
                .Last()
                .ToList();
        }

        private int[] SelectPieceIndexes(PieceType[] types) =>
            Enumerable.Range(0, Pieces.Length)
                .Where(i => types.Contains(Pieces[i]))
                .ToArray();
    }
}