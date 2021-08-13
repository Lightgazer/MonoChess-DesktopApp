using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoChess_DesktopApp.Draughts
{
    public interface IDraughtsBoardState
    {
        void Init(DraughtsModel model);
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}