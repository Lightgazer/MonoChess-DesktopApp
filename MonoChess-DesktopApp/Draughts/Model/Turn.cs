using MonoChess_DesktopApp.Draughts.Enums;
using System.Collections.Generic;
using System.Linq;

namespace MonoChess_DesktopApp.Draughts.Model
{
    public class Turn
    {
        public readonly PieceType[] Pieces;
        public readonly Side Side;
        public readonly List<Move> AllowedMoves;

        public Turn(PieceType[] pieces, Side side)
        {
            Side = side;
            Pieces = pieces;
            AllowedMoves = CalculateAllowedMoves();
        }

        private Move CreateMove(int index)
        {
            var type = Pieces[index];
            return type switch
            {
                PieceType.BlackPvt => new PvtMove(index, Pieces, false),
                PieceType.WhitePvt => new PvtMove(index, Pieces, false),
                PieceType.BlackKing => new KingMove(index, Pieces, false),
                PieceType.WhiteKing => new KingMove(index, Pieces, false),
                _ => null
            };
        }

        private List<Move> CalculateAllowedMoves()
        {
            var currentSidePieces = SelectPieceIndexes(Side);
            var movesList = currentSidePieces
                .Select(index => CreateMove(index))
                .ToList();

            if (movesList.Count == 0) return new List<Move>();

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