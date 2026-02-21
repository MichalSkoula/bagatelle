using Bagatelle.Shared.Controls;
using Bagatelle.Shared.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
#if ANDROID
using Android.Content;
#endif

namespace Bagatelle.Shared.Screens
{
    public class CreditsScreen : BaseScreen
    {
        private int _frameCount;
        private Rectangle _linkBounds;
        private Rectangle _backButton;

        public CreditsScreen(Game game) : base(game) { }

        public override void LoadContent()
        {
            _frameCount = 0;

            // Calculate link bounds
            string linkText = "skoula.cz/bagatelle";
            int centerX = GameConstants.ScreenWidth / 2;
            Vector2 linkPos = new Vector2(centerX, 1120);
            Vector2 linkSize = Game1.Font.MeasureString(linkText);
            _linkBounds = new Rectangle(
                (int)(linkPos.X - linkSize.X / 2),
                (int)(linkPos.Y - linkSize.Y / 2),
                (int)linkSize.X,
                (int)linkSize.Y
            );

            int buttonWidth = 660;
            int buttonHeight = 120;
            int buttonCenterX = GameConstants.ScreenWidth / 2 - buttonWidth / 2;
            _backButton = new Rectangle(buttonCenterX, 1240, buttonWidth, buttonHeight);
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

            // Return to menu
            if (InputManager.IsButtonPressed(_backButton))
                Game1.Screens.SetScreen(new MenuScreen(Game));
        }

        private void OpenLink(string url)
        {
            try
            {
#if ANDROID
                var uri = global::Android.Net.Uri.Parse(url);
                var intent = new Intent(Intent.ActionView, uri);
                intent.AddFlags(ActivityFlags.NewTask);
                global::Android.App.Application.Context.StartActivity(intent);
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
            DrawHelper.DrawCenteredString(spriteBatch, Game1.FontLarge, "CREDITS", new Vector2(centerX, 300), Color.White);

            DrawHelper.DrawCenteredString(spriteBatch, Game1.Font, "Inspired by the legendary", new Vector2(centerX, 660), Color.Beige);
            DrawHelper.DrawCenteredString(spriteBatch, Game1.Font, "Symbian game", new Vector2(centerX, 840), Color.Beige);
            DrawHelper.DrawCenteredString(spriteBatch, Game1.Font, "Bagatelle Touch (2009)", new Vector2(centerX, 920), Color.Beige);

            DrawHelper.DrawCenteredString(spriteBatch, Game1.Font, "(c) Michal Skoula", new Vector2(centerX, 1040), Color.Beige);

            // Draw clickable link
            DrawHelper.DrawCenteredString(spriteBatch, Game1.Font, "skoula.cz/bagatelle", new Vector2(centerX, 1120), Color.LightBlue);

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
