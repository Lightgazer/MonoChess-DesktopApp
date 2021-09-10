using MonoChess_DesktopApp.Draughts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoChess_DesktopApp.Draughts.Model
{
    //TODO: проверка на конец игры, ничью

    public class DraughtsModel
    {
        private Turn _currentTurn;

        public DraughtsModel()
        {
            var pieces = new PieceType[DraughtsConstants.NumberOfPositions];
            Array.Fill(pieces, PieceType.BlackPvt, 0, 20);
            Array.Fill(pieces, PieceType.WhitePvt, 30, 20);

            //pieces[10] = PieceType.WhitePvt;
            //pieces[1] = PieceType.None;
            //pieces[15] = PieceType.None;

            _currentTurn = new Turn(pieces, Side.White);
        }

        public DraughtsModel(Turn turn)
        {
            _currentTurn = turn;
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
            var nextSide = _currentTurn.Side == Side.White ? Side.Black : Side.White;
            var nextPieces = command.GetResultPieces();
            _currentTurn = new Turn(nextPieces, nextSide);
        }

        public GameState GetGameState()
        {
            if(_currentTurn.GetAllowedMoves().Count == 0)
            {
                return _currentTurn.Side switch
                {
                    Side.White => GameState.BlackWin,
                    Side.Black => GameState.WhiteWin,
                    _ => GameState.None
                };
            }
            //The game is considered a draw when the same position repeats itself for the third time (not necessarily consecutive), with the same player having the move each time.
            //A king-versus-king endgame is automatically declared a draw, as is any other position proven to be a draw.
            return GameState.Ongoing;
        }

        public IReadOnlyList<PieceType> GetPiecePositions()
        {
            return _currentTurn.Pieces;
        }

        public List<int> GetActivePieces()
        {
            return _currentTurn.GetAllowedMoves()
                .Select(move => move.GetStartOfMovement())
                .Distinct()
                .ToList();
        }
    }
}