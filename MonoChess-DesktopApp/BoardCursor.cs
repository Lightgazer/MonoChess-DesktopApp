using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MonoChess_DesktopApp
{
    public class BoardCursor
    {
        public Action<Point> OnSelect;
        private readonly Texture2D _texture;
        private Rectangle _target;
        private ButtonState _prevButtonState;
        private Point? _hoverIndex;

        public BoardCursor(ContentManager content, Rectangle target)
        {
            _texture = content.Load<Texture2D>("cursor");
            _target = target;
        }

        public void Update()
        {
            var mouseState = Mouse.GetState();
            _hoverIndex = PositionToBoardIndex(mouseState.Position);

            var leftButton = mouseState.LeftButton;
            if (_hoverIndex is {} selected 
                && _prevButtonState == ButtonState.Released && leftButton == ButtonState.Pressed)
            {
                OnSelect?.Invoke(selected);
            }

            _prevButtonState = leftButton;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_hoverIndex is { } index)
            {
                var blockSize = GameSettings.BlockPoint;
                var position = index * blockSize + _target.Location;
                spriteBatch.Draw(_texture, position.ToVector2(), Color.White);
            }
        }

        private Point? PositionToBoardIndex(Point position)
        {
            if (!_target.Contains(position))
                return null;
            position -= _target.Location;
            return position / new Point(GameSettings.BlockSize);
        }
    }
}