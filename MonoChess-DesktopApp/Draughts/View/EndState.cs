using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoChess_DesktopApp.Draughts.Enums;
using System;

namespace MonoChess_DesktopApp.Draughts.View
{
    class EndState : IDraughtsBoardState
    {
        private readonly UIElement _button;
        private readonly UIElement _label;

        public EndState(DraughtsBoardView context, GameState gameState)
        {
            var center = new Vector2(GameSettings.Width / 2, GameSettings.Width /2);
            var someSpace = new Vector2(0, center.Y / 4);
            var buttonTexture = context.Content.Load<Texture2D>("labels/ok");
            _button = new UIElement(buttonTexture, center + someSpace) { OnClick = ButtonOnClick };
            var labelName = gameState switch
            {
                GameState.BlackWin => "labels/black_win",
                GameState.WhiteWin => "labels/white_win",
                GameState.Draw => "labels/draw",
                _ => throw new NotImplementedException(),
            };
            _label = new UIElement(context.Content.Load<Texture2D>(labelName), center - someSpace);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _button.Draw(spriteBatch);
            _label.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            _button.Update(gameTime);
            _label.Update(gameTime);
        }

        public void ButtonOnClick()
        {
            SceneManager.LoadScene<DraughtsScene>();
        }
    }
}
