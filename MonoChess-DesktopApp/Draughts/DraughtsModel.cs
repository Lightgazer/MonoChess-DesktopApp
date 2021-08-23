using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoChess_DesktopApp.Draughts
{
    //TODO: нужно как-то связать енамы Turn и PieceType
    //TODO: проверка на конец игры, ничью

    public class DraughtsModel
    {
        private Turn _currentTurn;

        public DraughtsModel()
        {
            var pieces = new PieceType[DraughtsConstants.NumberOfPositions];
            Array.Fill(pieces, PieceType.BlackPvt, 0, 20);
            Array.Fill(pieces, PieceType.WhitePvt, 30, 20);
            _currentTurn = new Turn(pieces, TurnSide.White);
        }

        public List<Command> GetPossibleCommands(int startPosition)
        {
            return _currentTurn.GetAllowedMoves()
                .FindAll(move => move.GetStartOfMovement() == startPosition)
                .Select(move => new Command(move))
                .ToList();
        }

        public void Execute(Command command)
        {
            var nextSide = _currentTurn.Side == TurnSide.White ? TurnSide.Black : TurnSide.White;
            _currentTurn = new Turn(command.EndPieces, nextSide);
        }

        public GameState GetGameState()
        {
            return GameState.Ongoing;
        }

        public IReadOnlyList<PieceType> GetPiecePositions()
        {
            return _currentTurn.Pieces;
        }

        public IReadOnlyList<int> GetActivePieces()
        {
            return _currentTurn.GetAllowedMoves()
                .Select(move => move.GetStartOfMovement())
                .Distinct()
                .ToList();
        }
    }
}