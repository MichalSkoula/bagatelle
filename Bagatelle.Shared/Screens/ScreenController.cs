using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bagatelle.Shared.Screens
{
    public class ScreenController
    {
        private BaseScreen _currentScreen;
        private BaseScreen _nextScreen;
        private readonly Game _game;

        public ScreenController(Game game)
        {
            _game = game;
        }

        public void SetScreen(BaseScreen screen)
        {
            _nextScreen = screen;
        }

        public void Update(GameTime gameTime)
        {
            if (_nextScreen != null)
            {
                _currentScreen?.UnloadContent();
                _currentScreen = _nextScreen;
                _currentScreen.LoadContent();
                _nextScreen = null;
            }

            _currentScreen?.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _currentScreen?.Draw(gameTime, spriteBatch);
        }
    }
}
