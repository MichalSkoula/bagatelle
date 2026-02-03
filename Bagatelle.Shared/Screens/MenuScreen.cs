using Bagatelle.Shared.Controls;
using Bagatelle.Shared.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bagatelle.Shared.Screens
{
    public class MenuScreen : BaseScreen
    {
        private Rectangle _onePlayerButton;
        private Rectangle _twoPlayerButton;
        private Rectangle _creditsButton;
        private Rectangle _fullscreenButton;
        private Rectangle _exitButton;
        private int _frameCount;

        public MenuScreen(Game game) : base(game) { }

        public override void LoadContent()
        {
            int buttonWidth = 240;
            int buttonHeight = 60;
            int centerX = GameConstants.ScreenWidth / 2 - buttonWidth / 2;
            int startY = 300;
            int spacing = 80;

            _onePlayerButton = new Rectangle(centerX, startY, buttonWidth, buttonHeight);
            _twoPlayerButton = new Rectangle(centerX, startY + spacing, buttonWidth, buttonHeight);
            _creditsButton = new Rectangle(centerX, startY + spacing * 2, buttonWidth, buttonHeight);

#if WINDOWS
            _fullscreenButton = new Rectangle(centerX, startY + spacing * 3, buttonWidth, buttonHeight);
            _exitButton = new Rectangle(centerX, startY + spacing * 4, buttonWidth, buttonHeight);
#else
            _exitButton = new Rectangle(centerX, startY + spacing * 3, buttonWidth, buttonHeight);
#endif
            _frameCount = 0;
        }

        public override void Update(GameTime gameTime)
        {
            InputManager.Update(Game.IsActive);
            _frameCount++;

            // Ignore input for given frames to prevent click-through
            if (_frameCount < LimitFrames)
                return;

            if (InputManager.IsButtonPressed(_onePlayerButton))
                Game1.Screens.SetScreen(new PlayingScreen(Game, 1));
            else if (InputManager.IsButtonPressed(_twoPlayerButton))
                Game1.Screens.SetScreen(new PlayingScreen(Game, 2));
            else if (InputManager.IsButtonPressed(_creditsButton))
                Game1.Screens.SetScreen(new CreditsScreen(Game));
#if WINDOWS
            else if (InputManager.IsButtonPressed(_fullscreenButton))
            {
                ((Game1)Game).ToggleFullscreen();
            }
#endif
            else if (InputManager.IsButtonPressed(_exitButton))
            {
                Game.Exit();
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawHelper.DrawRectangle(spriteBatch, new Rectangle(0, 0, GameConstants.ScreenWidth, GameConstants.ScreenHeight), GameConstants.BoardDarkColor);

            DrawHelper.DrawCenteredString(spriteBatch, Game1.FontLarge, "BAGATELLE",
                new Vector2(GameConstants.ScreenWidth / 2, 150), Color.White);

            DrawButton(spriteBatch, _onePlayerButton, "1 PLAYER");
            DrawButton(spriteBatch, _twoPlayerButton, "2 PLAYERS");
            DrawButton(spriteBatch, _creditsButton, "CREDITS");

#if WINDOWS
            var graphics = ((Game1)Game).GetGraphicsDeviceManager();
            string fsText = graphics.IsFullScreen ? "WINDOWED" : "FULLSCREEN";
            DrawButton(spriteBatch, _fullscreenButton, fsText);
#endif
            DrawButton(spriteBatch, _exitButton, "EXIT");
        }

        private void DrawButton(SpriteBatch spriteBatch, Rectangle rect, string text)
        {
            DrawHelper.DrawRectangle(spriteBatch, rect, Color.Beige * 0.2f);
            DrawHelper.DrawBorder(spriteBatch, rect, Color.Beige, 2);
            DrawHelper.DrawCenteredString(spriteBatch, Game1.Font, text,
                new Vector2(rect.Center.X, rect.Center.Y), Color.Beige);
        }
    }
}
