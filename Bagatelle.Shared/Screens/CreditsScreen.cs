using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bagatelle.Shared.Controls;
using Bagatelle.Shared.UI;

namespace Bagatelle.Shared.Screens
{
    public class CreditsScreen : BaseScreen
    {
        private int _frameCount;
        
        public CreditsScreen(Game game) : base(game) { }

        public override void LoadContent()
        {
            _frameCount = 0;
        }

        public override void Update(GameTime gameTime)
        {
            InputManager.Update(Game.IsActive);
            _frameCount++;

            // Ignore input for 60 frames to prevent click-through
            if (_frameCount < 60)
                return;

            if (InputManager.WasConfirmPressed())
                Game1.Screens.SetScreen(new MenuScreen(Game));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawHelper.DrawRectangle(spriteBatch, new Rectangle(0, 0, GameConstants.ScreenWidth, GameConstants.ScreenHeight), GameConstants.BoardColor);

            int centerX = GameConstants.ScreenWidth / 2;
            DrawHelper.DrawCenteredString(spriteBatch, Game1.Font, "CREDITS", new Vector2(centerX, 150), Color.White);
            DrawHelper.DrawCenteredString(spriteBatch, Game1.Font, "Bagatelle", new Vector2(centerX, 300), Color.Gray);
            DrawHelper.DrawCenteredString(spriteBatch, Game1.Font, "Inspired by", new Vector2(centerX, 380), Color.Gray);
            DrawHelper.DrawCenteredString(spriteBatch, Game1.Font, "Symbian game", new Vector2(centerX, 420), Color.Gray);
            DrawHelper.DrawCenteredString(spriteBatch, Game1.Font, "Tap to return", new Vector2(centerX, 550), Color.White);
        }
    }
}
