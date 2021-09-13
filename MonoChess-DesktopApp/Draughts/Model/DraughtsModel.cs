using MonoChess_DesktopApp.Draughts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoChess_DesktopApp.Draughts.Model
{
    //TODO: проверка на конец игры, ничью

    public class DraughtsModel
    {
        private readonly List<Turn> _turnHistory = new List<Turn>();
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
            return _currentTurn.AllowedMoves
                .FindAll(move => move.GetStartOfMovement() == startPosition)
                .Select(move => new Command(move))
                .ToList();
        }

        public void Execute(Command command)
        {
            var nextSide = _currentTurn.Side == Side.White ? Side.Black : Side.White;
            var nextPieces = command.GetResultPieces();
            _turnHistory.Add(_currentTurn);
            _currentTurn = new Turn(nextPieces, nextSide);
        }

        public GameState GetGameState()
        {
            if(_currentTurn.AllowedMoves.Count == 0)
            {
                return _currentTurn.Side switch
                {
                    Side.White => GameState.BlackWin,
                    Side.Black => GameState.WhiteWin,
                    _ => GameState.None
                };
            }
            // The game is considered a draw when the same position repeats itself for the third time (not necessarily consecutive),
            // with the same player having the move each time.
            var repeatCount = _turnHistory
                .FindAll(turn => _currentTurn.Side == turn.Side)
                .FindAll(turn => _currentTurn.Pieces.SequenceEqual(turn.Pieces))
                .Count();
            if (repeatCount > DraughtsConstants.RepeatsBeforeDraw)
                return GameState.Draw;

            if (_currentTurn.Pieces.Where(piece => piece == PieceType.WhiteKing).Count() == 1 &&
                _currentTurn.Pieces.Where(piece => piece == PieceType.BlackKing).Count() == 1)
                return GameState.Draw;
            return GameState.Ongoing;
        }

        public IReadOnlyList<PieceType> GetPiecePositions()
        {
            return _currentTurn.Pieces;
        }

        public List<int> GetActivePieces()
        {
            return _currentTurn.AllowedMoves
                .Select(move => move.GetStartOfMovement())
                .Distinct()
                .ToList();
        }
    }
}