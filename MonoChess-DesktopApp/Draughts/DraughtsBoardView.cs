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
        private readonly Texture2D _frame;
        private readonly PieceType[,] _pieces;
        private readonly BoardCursor _cursor;
        private Point? _selectionIndex = null;
        private readonly DraughtsModel _model;

        public DraughtsBoardView(ContentManager content, DraughtsModel model, Point position)
        {
            _darkSquare = content.Load<Texture2D>("dark_square");
            _lightSquare = content.Load<Texture2D>("light_square");
            _blackPiece = content.Load<Texture2D>("black_piece");
            _whitePiece = content.Load<Texture2D>("white_piece");
            _frame = content.Load<Texture2D>("frame");
            _position = position;
            _pieces = ConvertPositions(model.GetPiecePositions());
            _model = model;
            var boardRect = new Rectangle(_position, new Point(Size * GameSettings.BlockSize));
            _cursor = new BoardCursor(content, boardRect);
            _cursor.OnSelect += OnPlayerSelect;
        }

        public void Update(GameTime gameTime, DraughtsModel model)
        {
            _cursor.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawCells(spriteBatch);
            DrawPieces(spriteBatch);
            _cursor.Draw(spriteBatch);
        }

        private static PieceType[,] ConvertPositions(IReadOnlyList<PieceType> positions)
        {
            var pieces = new PieceType[Size, Size];
            for (int index = 0; index < positions.Count; index++)
            {
                int row = index / RowLength;
                int colInModel = index % RowLength;
                int col = colInModel * 2 + (row.IsEven() ? 1 : 0);
                pieces[col, row] = positions[index];
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

                var position = IndexToPosition(index);
                spriteBatch.Draw(texture, position.ToVector2(), Color.White);
            });
        }

        private Point IndexToPosition(Point index)
            => _position + GameSettings.BlockPoint * index;

        private void ClearBoard()
        {
            _pieces.ForEach((x, y) => _pieces[x, y] = PieceType.None);
        }

        private void OnPlayerSelect(Point point)
        {
            if (PointToModelIndex(point) is { } index)
            {
                _selectionIndex = point;
                _model.GetPossibleActions(index);
            }
        }

        private int? PointToModelIndex(Point index)
        {
            var (x, y) = index;
            if (y.IsEven() && !x.IsEven() || !y.IsEven() && x.IsEven())
            {
                int col = (x.IsEven() ? x : x + 1) / 2;
                return y * DraughtsConstants.RowLength + col;
            }

            return null;
        }
    }
}