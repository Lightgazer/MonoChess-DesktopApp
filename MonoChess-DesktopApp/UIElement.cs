using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MonoChess_DesktopApp
{
    public class UIElement
    {
        public Action OnClick;

        private readonly Texture2D _texture;
        private Rectangle _rectangle;
        private MouseState _lastMouseState;

        public UIElement(Texture2D texture, Vector2 position)
        {
            this._texture = texture;
            var size = new Vector2(texture.Width, texture.Height);
            var vector = position - size / 2;
            _rectangle = new Rectangle(vector.ToPoint(), size.ToPoint());
            _lastMouseState = Mouse.GetState();
        }

        public void Update(GameTime gameTime)
        {
            if (OnClick != null && CheckClick()) OnClick();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _rectangle, Color.White);
        }

        private bool CheckClick()
        {
            var mouseState = Mouse.GetState();
            if (_lastMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
            {
                if (_rectangle.Contains(mouseState.Position))
                {
                    return true;
                }
            }

            _lastMouseState = mouseState;
            return false;
        }
    }
}
