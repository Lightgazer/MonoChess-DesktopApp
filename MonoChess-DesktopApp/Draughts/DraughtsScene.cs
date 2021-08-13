using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoChess_DesktopApp.Draughts
{
    //https://en.wikipedia.org/wiki/International_draughts
    
    internal class DraughtsScene : IScene
    {
        private readonly ContentManager _content;

        private DraughtsBoardView _boardView;
        private DraughtsModel _model;

        public DraughtsScene(ContentManager content)
        {
            _content = content;
        }

        public void Start()
        {
            const int boardPx = DraughtsConstants.BoardSize * GameSettings.BlockSize;
            _model = new DraughtsModel();
            var position = new Point((GameSettings.Width - boardPx) / 2, (GameSettings.Height - boardPx) / 2);
            _boardView = new DraughtsBoardView(_content, _model, position);
        }
        
        public void Stop() {}
        
        public void Update(GameTime gameTime)
        {
            _boardView.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _boardView.Draw(spriteBatch);
        }
    }
}