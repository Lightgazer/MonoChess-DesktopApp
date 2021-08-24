using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoChess_DesktopApp.Extensions;

namespace MonoChess_DesktopApp.Draughts
{
    internal class Piece
    {
        public Point BoardIndex;
        public Vector2 ScreenDisplacement;
        public PieceType Type;
    }

    public class DraughtsBoardView
    {
        internal List<Piece> Pieces { get; private set; }
        internal DraughtsModel Model { get; private set; }
        internal ContentManager Content { get; private set; }
        internal IDraughtsBoardState State { private get; set; }
        private const int Size = DraughtsConstants.BoardSize;
        private const int RowLength = DraughtsConstants.RowLength;
        private readonly Point _position;
        private readonly Texture2D _lightSquare;
        private readonly Texture2D _darkSquare;
        private readonly Texture2D _whitePiece;
        private readonly Texture2D _blackPiece;

        public static Point SquareNumberToPoint(int number)
        {
            int row = number / RowLength;
            int colInModel = number % RowLength;
            int col = colInModel * 2 + (row.IsEven() ? 1 : 0);
            return new Point(col, row);
        }

        public DraughtsBoardView(ContentManager content, DraughtsModel model, Point position)
        {
            _darkSquare = content.Load<Texture2D>("dark_square");
            _lightSquare = content.Load<Texture2D>("light_square");
            _blackPiece = content.Load<Texture2D>("black_piece");
            _whitePiece = content.Load<Texture2D>("white_piece");
            _position = position;
            Content = content;
            Model = model;
            StartNewTurn();
        }

        public Rectangle GetScreenRectangle()
            => new Rectangle(_position, new Point(Size * GameSettings.BlockSize));

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
            State = new PieceSelectionState(this);
        }

        public Point PointToScreenPosition(Point index) 
            => _position + GameSettings.BlockPoint * index;

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
                var position = blockIndex * GameSettings.BlockPoint + _position;
                var isRowEven = blockIndex.Y.IsEven();
                var texture = (i + (isRowEven ? 0 : 1)) % 2 == 1 ? _darkSquare : _lightSquare;
                spriteBatch.Draw(texture, position.ToVector2(), Color.White);
            }
        }

        private void DrawPieces(SpriteBatch spriteBatch)
        {
            Pieces.ForEach(piece => {
                var texture = piece.Type switch
                {
                    PieceType.BlackPvt => _blackPiece,
                    PieceType.WhitePvt => _whitePiece,
                    _ => null
                };
                if (texture is null) return;

                var position = PointToScreenPosition(piece.BoardIndex).ToVector2() + piece.ScreenDisplacement;
                spriteBatch.Draw(texture, position, Color.White);
            });
        }
    }
}