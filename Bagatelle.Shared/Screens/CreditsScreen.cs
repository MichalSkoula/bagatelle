using Bagatelle.Shared.Controls;
using Bagatelle.Shared.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
#if ANDROID
using Xamarin.Essentials;
#endif

namespace Bagatelle.Shared.Screens
{
    public class CreditsScreen : BaseScreen
    {
        private int _frameCount;
        private Rectangle _linkBounds;

        public CreditsScreen(Game game) : base(game) { }

        public override void LoadContent()
        {
            _frameCount = 0;

            // Calculate link bounds
            string linkText = "skoula.cz/bagatelle";
            int centerX = GameConstants.ScreenWidth / 2;
            Vector2 linkPos = new Vector2(centerX, 560);
            Vector2 linkSize = Game1.Font.MeasureString(linkText);
            _linkBounds = new Rectangle(
                (int)(linkPos.X - linkSize.X / 2),
                (int)(linkPos.Y - linkSize.Y / 2),
                (int)linkSize.X,
                (int)linkSize.Y
            );
        }

        public override void Update(GameTime gameTime)
        {
            InputManager.Update(Game.IsActive);
            _frameCount++;

            // Ignore input for given frames to prevent click-through
            if (_frameCount < LimitFrames)
                return;

            // Check if link was clicked
            if (InputManager.IsButtonPressed(_linkBounds))
            {
                OpenLink("https://skoula.cz/bagatelle");
                return;
            }

            // Return to menu only if not clicking the link
            if (InputManager.WasConfirmPressed() && !InputManager.IsButtonHeld(_linkBounds))
                Game1.Screens.SetScreen(new MenuScreen(Game));
        }

        private void OpenLink(string url)
        {
            try
            {
#if ANDROID
                _ = Browser.OpenAsync(new Uri(url), BrowserLaunchMode.SystemPreferred);
#else
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
#endif
            }
            catch
            {
                // Silently ignore if browser can't be opened
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawHelper.DrawRectangle(spriteBatch, new Rectangle(0, 0, GameConstants.ScreenWidth, GameConstants.ScreenHeight), GameConstants.BoardDarkColor);

            int centerX = GameConstants.ScreenWidth / 2;
            DrawHelper.DrawCenteredString(spriteBatch, Game1.Font, "CREDITS", new Vector2(centerX, 150), Color.White);

            DrawHelper.DrawCenteredString(spriteBatch, Game1.Font, "Inspired by the legendary", new Vector2(centerX, 380), Color.Beige);
            DrawHelper.DrawCenteredString(spriteBatch, Game1.Font, "Symbian game", new Vector2(centerX, 420), Color.Beige);
            DrawHelper.DrawCenteredString(spriteBatch, Game1.Font, "Bagatelle Touch (2009)", new Vector2(centerX, 460), Color.Beige);

            DrawHelper.DrawCenteredString(spriteBatch, Game1.Font, "(c) Michal Skoula", new Vector2(centerX, 520), Color.Beige);

            // Draw clickable link
            DrawHelper.DrawCenteredString(spriteBatch, Game1.Font, "skoula.cz/bagatelle", new Vector2(centerX, 560), Color.LightBlue);

            DrawHelper.DrawCenteredString(spriteBatch, Game1.Font, "Tap to return", new Vector2(centerX, 660), Color.Beige);
        }
    }
}
