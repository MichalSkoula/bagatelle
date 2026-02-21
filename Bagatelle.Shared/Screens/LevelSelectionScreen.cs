using Bagatelle.Shared.Controls;
using Bagatelle.Shared.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bagatelle.Shared.Screens
{
    public class LevelSelectionScreen : BaseScreen
    {
        private Rectangle _level1Button;
        private Rectangle _backButton;
        private int _frameCount;
        private readonly int _playerCount;

        public LevelSelectionScreen(Game game, int playerCount) : base(game)
        {
            _playerCount = playerCount;
        }

        public override void LoadContent()
        {
            int buttonWidth = 660;
            int buttonHeight = 120;
            int centerX = GameConstants.ScreenWidth / 2 - buttonWidth / 2;
            int startY = 600;
            int spacing = 160;

            _level1Button = new Rectangle(centerX, startY, buttonWidth, buttonHeight);
            _backButton = new Rectangle(centerX, startY + spacing * 4, buttonWidth, buttonHeight);
            
            _frameCount = 0;
        }

        public override void Update(GameTime gameTime)
        {
            InputManager.Update(Game.IsActive);
            _frameCount++;

            if (_frameCount < LimitFrames)
                return;

            if (InputManager.IsButtonPressed(_level1Button))
            {
                Game1.Screens.SetScreen(new PlayingScreen(Game, _playerCount));
            }
            else if (InputManager.IsButtonPressed(_backButton))
            {
                Game1.Screens.SetScreen(new MenuScreen(Game));
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawHelper.DrawRectangle(spriteBatch, new Rectangle(0, 0, GameConstants.ScreenWidth, GameConstants.ScreenHeight), GameConstants.BoardDarkColor);

            DrawHelper.DrawCenteredString(spriteBatch, Game1.Font, "SELECT LEVEL",
                new Vector2(GameConstants.ScreenWidth / 2, 300), Color.White);

            DrawButton(spriteBatch, _level1Button, "HERITAGE");
            DrawButton(spriteBatch, _backButton, "BACK TO MENU");
        }

        private void DrawButton(SpriteBatch spriteBatch, Rectangle rect, string text)
        {
            DrawHelper.DrawRectangle(spriteBatch, rect, Color.Beige * 0.2f);
            DrawHelper.DrawBorder(spriteBatch, rect, Color.Beige, 4);
            DrawHelper.DrawCenteredString(spriteBatch, Game1.Font, text,
                new Vector2(rect.Center.X, rect.Center.Y), Color.Beige);
        }
    }
}
