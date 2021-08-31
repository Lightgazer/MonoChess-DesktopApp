using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoChess_DesktopApp.Draughts.View
{
    public interface IDraughtsBoardState
    {
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}