namespace MonoChess_DesktopApp.Draughts
{
    public static class DraughtsConstants
    {
        public const int BoardSize = 10;
        public const int NumberOfPositions = BoardSize * BoardSize / 2;
        public const int RowLength = NumberOfPositions / BoardSize;
        public const int RepeatsBeforeDraw = 2;
    }
}