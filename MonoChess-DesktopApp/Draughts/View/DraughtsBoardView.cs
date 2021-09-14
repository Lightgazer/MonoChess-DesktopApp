using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoChess_DesktopApp.Draughts.Enums;
using MonoChess_DesktopApp.Draughts.Model;
using MonoChess_DesktopApp.Extensions;

namespace MonoChess_DesktopApp.Draughts.View
{
    public class Piece
    {
        public Point BoardIndex;
        public Vector2 ScreenDisplacement;
        public PieceType Type;
    }

    public class DraughtsBoardView
    {
        public List<Piece> Pieces { get; private set; }
        public IDraughtsBoardState State { private get; set; }
        public readonly DraughtsModel Model;
        public readonly ContentManager Content;
        public readonly Point Position;
        private const int Size = DraughtsConstants.BoardSize;
        private const int RowLength = DraughtsConstants.RowLength;
        private readonly Texture2D _lightSquare;
        private readonly Texture2D _darkSquare;
        private readonly Texture2D _whitePiece;
        private readonly Texture2D _blackPiece;
        private readonly Texture2D _blackPieceKing;
        private readonly Texture2D _whitePieceKing;

        public static Point SquareNumberToPoint(int number)
        {
            int row = number / RowLength;
            int colInModel = number % RowLength;
            int col = colInModel * 2 + (row.IsEven() ? 1 : 0);
            return new Point(col, row);
        }

        public static Vector2 PointToScreenDisplacement(Point index)
            => (GameSettings.BlockPoint * index).ToVector2();

        public static Vector2 SquareNumberToScreenDisplacement(int number)
            => PointToScreenDisplacement(SquareNumberToPoint(number));

        public DraughtsBoardView(ContentManager content, DraughtsModel model, Point position)
        {
            _darkSquare = content.Load<Texture2D>("dark_square");
            _lightSquare = content.Load<Texture2D>("light_square");
            _blackPiece = content.Load<Texture2D>("black_piece");
            _whitePiece = content.Load<Texture2D>("white_piece");
            _blackPieceKing = content.Load<Texture2D>("black_king");
            _whitePieceKing = content.Load<Texture2D>("white_king");
            Position = position;
            Content = content;
            Model = model;
            StartNewTurn();
        }

        public Rectangle GetScreenRectangle()
            => new Rectangle(Position, new Point(Size * GameSettings.BlockSize));

        public void Update(GameTime gameTime)
        {
            State.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawCells(spriteBatch);
            DrawPieces(spriteBatch);
            State.Draw(spriteBatch);
        }

        public void StartNewTurn()
        {
            Pieces = ConvertPositions(Model.GetPiecePositions());
            var gameState = Model.GetGameState();
            if (gameState == GameState.Ongoing)
            {
                State = new PieceSelectionState(this);
                return;
            }
            State = new EndState(this, gameState);
        }

        public void RemovePieceAt(Point point)
        {
            Pieces = Pieces.Where(piece => piece.BoardIndex != point).ToList();
        }

        public Point PointToScreenPosition(Point index)
            => Position + GameSettings.BlockPoint * index;

        private static List<Piece> ConvertPositions(IReadOnlyList<PieceType> positions)
        {
            var pieces = new List<Piece>();
            for (int index = 0; index < positions.Count; index++)
            {
                var type = positions[index];
                if (type == PieceType.None) continue;
                var piece = new Piece { BoardIndex = SquareNumberToPoint(index), Type = type };
                pieces.Add(piece);
            }

            return pieces;
        }

        private void DrawCells(SpriteBatch spriteBatch)
        {
            const int cellsCount = Size * Size;
            for (int i = 0; i < cellsCount; i++)
            {
                var blockIndex = new Point(i % Size, i / Size);
                var position = blockIndex * GameSettings.BlockPoint + Position;
                var isRowEven = blockIndex.Y.IsEven();
                var texture = (i + (isRowEven ? 0 : 1)) % 2 == 1 ? _darkSquare : _lightSquare;
                spriteBatch.Draw(texture, position.ToVector2(), Color.White);
            }
        }

        private void DrawPieces(SpriteBatch spriteBatch)
        {
            Pieces.ForEach(piece =>
            {
                var texture = piece.Type switch
                {
                    PieceType.BlackPvt => _blackPiece,
                    PieceType.WhitePvt => _whitePiece,
                    PieceType.WhiteKing => _whitePieceKing,
                    PieceType.BlackKing => _blackPieceKing,
                    _ => null
                };
                if (texture is null) return;

                var position = PointToScreenPosition(piece.BoardIndex).ToVector2() + piece.ScreenDisplacement;
                spriteBatch.Draw(texture, position, Color.White);
            });
        }
    }
}