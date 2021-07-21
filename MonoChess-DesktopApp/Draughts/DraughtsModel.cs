using System;
using System.Linq;

namespace MonoChess_DesktopApp.Draughts
{
    public enum PieceType
    {
        None,
        WhitePvt,
        BlackPvt,
        WhiteKing,
        BlackKing
    }
    
    public enum GameState
    {
        Ongoing,
        Draw,
        WhiteWin,
        BlackWin
    }

    public enum Turn
    {
        White,
        Black
    }
    
    public class DraughtsModel
    {
        public Turn CurrentTurn { get; } = Turn.White;
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
        
        public void Execute(Command action) {}

        public GameState GetGameState()
        {
            return GameState.Ongoing;
        }

        public PieceType[] GetPiecePositions()
        {
            return _pieces;
        }
    }
}