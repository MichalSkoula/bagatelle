using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bagatelle.Shared.Controls;
using Bagatelle.Shared.UI;

namespace Bagatelle.Shared.Screens
{
    public class MenuScreen : BaseScreen
    {
        private Rectangle _onePlayerButton;
        private Rectangle _twoPlayerButton;
        private Rectangle _creditsButton;

        public MenuScreen(Game game) : base(game) { }

        public override void LoadContent()
        {
            int buttonWidth = 200;
            int buttonHeight = 60;
            int centerX = GameConstants.ScreenWidth / 2 - buttonWidth / 2;
            int startY = 300;
            int spacing = 80;

            _onePlayerButton = new Rectangle(centerX, startY, buttonWidth, buttonHeight);
            _twoPlayerButton = new Rectangle(centerX, startY + spacing, buttonWidth, buttonHeight);
            _creditsButton = new Rectangle(centerX, startY + spacing * 2, buttonWidth, buttonHeight);
        }

        public override void Update(GameTime gameTime)
        {
            InputManager.Update(Game.IsActive);

            if (InputManager.IsButtonPressed(_onePlayerButton))
                Game1.Screens.SetScreen(new PlayingScreen(Game, 1));
            else if (InputManager.IsButtonPressed(_twoPlayerButton))
                Game1.Screens.SetScreen(new PlayingScreen(Game, 2));
            else if (InputManager.IsButtonPressed(_creditsButton))
                Game1.Screens.SetScreen(new CreditsScreen(Game));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawHelper.DrawRectangle(spriteBatch, new Rectangle(0, 0, GameConstants.ScreenWidth, GameConstants.ScreenHeight), GameConstants.BoardColor);

            DrawHelper.DrawCenteredString(spriteBatch, Game1.Font, "BAGATELLE",
                new Vector2(GameConstants.ScreenWidth / 2, 150), Color.White);

            DrawButton(spriteBatch, _onePlayerButton, "1 PLAYER");
            DrawButton(spriteBatch, _twoPlayerButton, "2 PLAYERS");
            DrawButton(spriteBatch, _creditsButton, "CREDITS");
        }

        private void DrawButton(SpriteBatch spriteBatch, Rectangle rect, string text)
        {
            DrawHelper.DrawRectangle(spriteBatch, rect, Color.White * 0.2f);
            DrawHelper.DrawBorder(spriteBatch, rect, Color.White, 2);
            DrawHelper.DrawCenteredString(spriteBatch, Game1.Font, text,
                new Vector2(rect.Center.X, rect.Center.Y), Color.White);
        }
    }
}
