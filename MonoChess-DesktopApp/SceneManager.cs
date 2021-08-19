using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoChess_DesktopApp
{
    internal static class SceneManager
    {
        private static IScene CurrentScene { get; set; }
        private static readonly List<IScene> Scenes = new List<IScene>();

        public static void AddScene(IScene scene)
        {
            Scenes.Add(scene);
        }

        public static void LoadScene<T>()
        {
            var scene = Scenes.Find(scene => scene is T);
            LoadScene(scene);
        }

        public static void LoadScene(int index)
        {
            var scene = Scenes.ElementAtOrDefault(index);
            LoadScene(scene);
        }

        public static void Update(GameTime gameTime)
        {
            CurrentScene.Update(gameTime);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            CurrentScene.Draw(spriteBatch);
        }

        private static void LoadScene(IScene scene)
        {
            if (scene is { })
            {
                CurrentScene?.Stop();
                CurrentScene = scene;
                CurrentScene.Start();
            }
        }
    }
}