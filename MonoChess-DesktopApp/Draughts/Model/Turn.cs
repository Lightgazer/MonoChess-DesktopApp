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
            var currentSidePieces = SelectPieceIndexes(Side);
            var movesList = currentSidePieces
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

        private int[] SelectPieceIndexes(Side side) =>
            Enumerable.Range(0, Pieces.Length)
                .Where(i => Pieces[i].GetSide() == side)
                .ToArray();
    }
}