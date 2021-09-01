namespace MonoChess_DesktopApp.Draughts.Enums
{
    public enum PieceType : byte
    {
        None,
        WhitePvt,
        BlackPvt,
        WhiteKing,
        BlackKing,
        Captured
    }

    public static class SideHelper
    {
        public static Side GetSide(this PieceType self) => self switch
        {
            PieceType.BlackPvt => Side.Black,   // 'or' only works in C# 9 :(
            PieceType.BlackKing => Side.Black,
            PieceType.WhitePvt => Side.White,
            PieceType.WhiteKing => Side.White,
            _ => Side.None
        };
    }
}