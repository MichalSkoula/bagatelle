using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bagatelle.Shared.Controls;
using Bagatelle.Shared.UI;

namespace Bagatelle.Shared.Screens
{
    public class IntroScreen : BaseScreen
    {
        public IntroScreen(Game game) : base(game) { }

        public override void Update(GameTime gameTime)
        {
            InputManager.Update(Game.IsActive);

            if (InputManager.WasConfirmPressed())
            {
                Game1.Screens.SetScreen(new MenuScreen(Game));
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Fill background with board color
            DrawHelper.DrawRectangle(spriteBatch, new Rectangle(0, 0, GameConstants.ScreenWidth, GameConstants.ScreenHeight), GameConstants.BoardColor);

            var center = new Vector2(GameConstants.ScreenWidth / 2, GameConstants.ScreenHeight / 2);
            DrawHelper.DrawCenteredString(spriteBatch, Game1.Font, "BAGATELLE", center, Color.White);
            DrawHelper.DrawCenteredString(spriteBatch, Game1.Font, "Tap to start", center + new Vector2(0, 50), Color.Beige);
        }
    }
}
