using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoChess_DesktopApp.Draughts
{
    //TODO: нужно как-то связать Turn и PieceType

    public class DraughtsModel
    {
        public Turn CurrentTurn { get; private set; } = Turn.White;

        //Passive model? 
        public event Action<DraughtsModel> OnUpdatePositions;

        private readonly PieceType[] _pieces = new PieceType[DraughtsConstants.NumberOfPositions];

        public DraughtsModel()
        {
            Array.Fill(_pieces, PieceType.BlackPvt, 0, 20);
            Array.Fill(_pieces, PieceType.WhitePvt, 30, 20);
        }

        public Command[] GetPossibleActions(int startPosition)
        {
            return null;
        }

        public void Execute(Command action)
        {
        }

        public GameState GetGameState()
        {
            return GameState.Ongoing;
        }

        public PieceType[] GetPiecePositions()
        {
            return _pieces;
        }

        public List<int> GetActivePieces()
        {
            var currentTurnPieces = CurrentTurn switch
            {
                Turn.White => SelectPieceIndexes(new[] {PieceType.WhitePvt, PieceType.WhiteKing}),
                Turn.Black => SelectPieceIndexes(new[] {PieceType.BlackPvt, PieceType.BlackKing}),
                _ => throw new ArgumentOutOfRangeException()
            };
            var movesList = currentTurnPieces
                .Select(index => new Move(index, _pieces, false))
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

            var allowedMoves = movesList
                .GroupBy(move => move.CaptureCount)
                .OrderBy(move => move.First().CaptureCount)
                .Last()
                .ToList();

            return allowedMoves
                .Select(GetStartOfMovement)
                .Distinct()
                .ToList();
        }

        private int GetStartOfMovement(Move move)
        {
            var ret = move;
            while (ret.Parent is { })
                ret = ret.Parent;
            return ret.Position;
        }

        private int[] SelectPieceIndexes(PieceType[] types) =>
            Enumerable.Range(0, _pieces.Length)
                .Where(i => types.Contains(_pieces[i]))
                .ToArray();
    }
}