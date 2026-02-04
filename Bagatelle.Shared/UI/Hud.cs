using Bagatelle.Shared.Logic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bagatelle.Shared.UI
{
    public class Hud
    {
        private readonly GameManager _gameManager;

        public Hud(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int y = 12; // Moved down to make room for menu button
            int padding = 10;

            // Player 1
            DrawPlayerInfo(spriteBatch, _gameManager.Players[0], padding, y, _gameManager.CurrentPlayerIndex == 0);

            // Player 2 (if exists)
            if (_gameManager.PlayerCount > 1)
            {
                DrawPlayerInfo(spriteBatch, _gameManager.Players[1],
                    GameConstants.ScreenWidth - 150 - padding, y, _gameManager.CurrentPlayerIndex == 1);
            }

            // Game over indicator
            if (_gameManager.State == GameState.GameOver)
            {
                var rect = new Rectangle(
                    GameConstants.ScreenWidth / 2 - 110,
                    GameConstants.ScreenHeight / 2 - 30,
                    220, 55);
                DrawHelper.DrawRectangle(spriteBatch, rect, Color.Black * 0.8f);
                DrawHelper.DrawCenteredString(spriteBatch, Game1.Font, "GAME OVER",
                    new Vector2(rect.Center.X, rect.Center.Y), Color.Beige);
            }
        }

        private void DrawPlayerInfo(SpriteBatch sb, Player player, int x, int y, bool isActive)
        {
            var box = new Rectangle(x, y, 150, 60);

            if (isActive)
            {
                DrawHelper.DrawBorder(sb, new Rectangle(box.X - 2, box.Y - 2, box.Width + 4, box.Height + 4), Color.Beige, 2);
            }

            DrawHelper.DrawRectangle(sb, box, Color.Black * 0.5f);
            DrawHelper.DrawRectangle(sb, new Rectangle(x + 5, y + 5, 20, 50), player.Color);

            // Player name with color
            //string playerName = $"P{player.Id}";
            //sb.DrawString(Game1.FontSmall, playerName, new Vector2(x + 35, y + 5), player.Color);

            // Score
            sb.DrawString(Game1.Font, player.Score.ToString(), new Vector2(x + 35, y + 8), Color.White);

            // Balls remaining - draw sprites instead of circles
            for (int i = 0; i < player.BallsRemaining; i++)
            {
                float ballSize = 14f; // Diameter
                float scale = ballSize / player.Sprite.Width;
                sb.Draw(
                    player.Sprite,
                    new Vector2(x + 40 + i * 18, y + 46),
                    null,
                    Color.White,
                    0f,
                    new Vector2(player.Sprite.Width / 2f, player.Sprite.Height / 2f),
                    scale,
                    SpriteEffects.None,
                    0f
                );
            }
        }
    }
}
