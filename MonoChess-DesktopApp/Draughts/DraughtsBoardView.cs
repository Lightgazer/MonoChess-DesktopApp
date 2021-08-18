using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoChess_DesktopApp.Extensions;

namespace MonoChess_DesktopApp.Draughts
{
    public class DraughtsBoardView
    {
        private const int Size = DraughtsConstants.BoardSize;
        private const int RowLength = DraughtsConstants.RowLength;
        private readonly Point _position;
        private readonly Texture2D _lightSquare;
        private readonly Texture2D _darkSquare;
        private readonly Texture2D _whitePiece;
        private readonly Texture2D _blackPiece;
        private readonly PieceType[,] _pieces;
        private readonly DraughtsModel _model;
        private IDraughtsBoardState _state;

        public static Point PointFromSquareNumber(int number)
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
            _pieces = ConvertPositions(model.GetPiecePositions());
            _model = model;
            _state = new PieceSelectionState(content, this);
            _state.Init(_model);
        }

        public Rectangle GetScreenRectangle()
            => new Rectangle(_position, new Point(Size * GameSettings.BlockSize));

        public void Update(GameTime gameTime)
        {
            _state.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawCells(spriteBatch);
            DrawPieces(spriteBatch);
            _state.Draw(spriteBatch);
        }

        public void TransitionTo(IDraughtsBoardState state)
        {
            _state = state;
            _state.Init(_model);
        }

        public Point PointToScreenPosition(Point index) 
            => _position + GameSettings.BlockPoint * index;

        private static PieceType[,] ConvertPositions(IReadOnlyList<PieceType> positions)
        {
            var pieces = new PieceType[Size, Size];
            for (int index = 0; index < positions.Count; index++)
            {
                var (x, y) = PointFromSquareNumber(index);
                pieces[x, y] = positions[index];
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
            _pieces.ForEach((type, index) =>
            {
                var texture = type switch
                {
                    PieceType.BlackPvt => _blackPiece,
                    PieceType.WhitePvt => _whitePiece,
                    _ => null
                };
                if (texture is null) return;

                var position = PointToScreenPosition(index);
                spriteBatch.Draw(texture, position.ToVector2(), Color.White);
            });
        }

        private void ClearBoard()
        {
            _pieces.ForEach((x, y) => _pieces[x, y] = PieceType.None);
        }
    }
}