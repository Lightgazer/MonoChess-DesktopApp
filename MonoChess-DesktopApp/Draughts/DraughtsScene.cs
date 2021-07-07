using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoChess_DesktopApp.Draughts
{
    //https://en.wikipedia.org/wiki/International_draughts
    
    internal class DraughtsScene : IScene
    {
        private const int BoardSize = 10;
        
        private readonly Texture2D _blackPieceTexture;
        private readonly Texture2D _whitePieceTexture;
        private readonly Texture2D _lightSquareTexture;
        private readonly Texture2D _darkSquareTexture;
        private readonly Texture2D _frameTexture;

        private DraughtsBoard _board;

        public DraughtsScene(ContentManager content)
        {
            _blackPieceTexture = content.Load<Texture2D>("black_piece");
            _whitePieceTexture = content.Load<Texture2D>("white_piece");
            _lightSquareTexture = content.Load<Texture2D>("light_square");
            _darkSquareTexture = content.Load<Texture2D>("dark_square");
            _frameTexture = content.Load<Texture2D>("frame");
        }

        public void Start()
        {
            const int boardPx = BoardSize * GameSettings.BlockSize;
            _board = new DraughtsBoard(_darkSquareTexture, _lightSquareTexture, BoardSize)
            {
                Position = new Point((GameSettings.Width - boardPx) / 2, (GameSettings.Height - boardPx) / 2)
            };
        }
        
        public void Stop() {}
        
        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _board.Draw(spriteBatch);
        }
    }
}