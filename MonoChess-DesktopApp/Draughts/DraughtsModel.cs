using System;
using System.Linq;

namespace MonoChess_DesktopApp.Draughts
{
    public enum PieceTypes
    {
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
    
    public class DraughtsModel
    {
        private readonly PieceTypes[] _pieces = new PieceTypes[50];

        public DraughtsModel()
        {
            Array.Fill(_pieces, PieceTypes.BlackPvt, 0, 20);
            Array.Fill(_pieces, PieceTypes.WhitePvt, 30, 20);
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
    }
}