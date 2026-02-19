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
            int y = 24;
            int padding = 20;

            // Player 1
            DrawPlayerInfo(spriteBatch, _gameManager.Players[0], padding, y, _gameManager.CurrentPlayerIndex == 0);

            // Player 2 (if exists)
            if (_gameManager.PlayerCount > 1)
            {
                DrawPlayerInfo(spriteBatch, _gameManager.Players[1],
                    GameConstants.ScreenWidth - 300 - padding, y, _gameManager.CurrentPlayerIndex == 1);
            }

            // Game over indicator
            if (_gameManager.State == GameState.GameOver)
            {
                var rect = new Rectangle(
                    GameConstants.ScreenWidth / 2 - 250,
                    GameConstants.ScreenHeight / 2 - 60,
                    500, 110);
                DrawHelper.DrawRectangle(spriteBatch, rect, Color.Black * 0.7f);
                DrawHelper.DrawCenteredString(spriteBatch, Game1.Font, "GAME OVER",
                    new Vector2(rect.Center.X, rect.Center.Y), Color.Beige);
            }
        }

        private void DrawPlayerInfo(SpriteBatch sb, Player player, int x, int y, bool isActive)
        {
            var box = new Rectangle(x, y, 300, 120);

            if (isActive)
            {
                DrawHelper.DrawBorder(sb, new Rectangle(box.X - 4, box.Y - 4, box.Width + 8, box.Height + 8), Color.Beige, 4);
            }

            DrawHelper.DrawRectangle(sb, box, Color.Black * 0.5f);
            DrawHelper.DrawRectangle(sb, new Rectangle(x + 10, y + 10, 40, 100), player.Color);

            // Player name with color
            //string playerName = $"P{player.Id}";
            //sb.DrawString(Game1.FontSmall, playerName, new Vector2(x + 70, y + 10), player.Color);

            // Score
            sb.DrawString(Game1.Font, player.Score.ToString(), new Vector2(x + 70, y), Color.Beige);

            // Balls remaining - draw sprites instead of circles
            for (int i = 0; i < player.BallsRemaining; i++)
            {
                float ballSize = 28f; // Diameter
                float scale = ballSize / player.Sprite.Width;
                sb.Draw(
                    player.Sprite,
                    new Vector2(x + 80 + i * 36, y + 92),
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
