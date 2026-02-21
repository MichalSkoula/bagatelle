using Bagatelle.Shared.Controls;
using Bagatelle.Shared.Logic;
using Bagatelle.Shared.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bagatelle.Shared.Screens
{
    public class GameOverScreen : BaseScreen
    {
        private readonly GameManager _gameManager;
        private Rectangle _restartButton;
        private Rectangle _menuButton;
        private int _frameCount;

        public GameOverScreen(Game game, GameManager gameManager) : base(game)
        {
            _gameManager = gameManager;
        }

        public override void LoadContent()
        {
            int buttonWidth = 400;
            int buttonHeight = 120;
            int centerX = GameConstants.ScreenWidth / 2 - buttonWidth / 2;

            _restartButton = new Rectangle(centerX, 1080, buttonWidth, buttonHeight);
            _menuButton = new Rectangle(centerX, 1240, buttonWidth, buttonHeight);
            _frameCount = 0;
        }

        public override void Update(GameTime gameTime)
        {
            InputManager.Update(Game.IsActive);
            _frameCount++;

            // Ignore input for given frames to prevent click-through
            if (_frameCount < LimitFrames)
                return;

            if (InputManager.IsButtonPressed(_restartButton))
                Game1.Screens.SetScreen(new PlayingScreen(Game, _gameManager.PlayerCount, _gameManager.Board.GetType()));
            else if (InputManager.IsButtonPressed(_menuButton))
                Game1.Screens.SetScreen(new MenuScreen(Game));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawHelper.DrawRectangle(spriteBatch, new Rectangle(0, 0, GameConstants.ScreenWidth, GameConstants.ScreenHeight), GameConstants.BoardDarkColor);

            int centerX = GameConstants.ScreenWidth / 2;

            DrawHelper.DrawCenteredString(spriteBatch, Game1.FontLarge, "GAME OVER", new Vector2(centerX, 300), Color.White);

            for (int i = 0; i < _gameManager.PlayerCount; i++)
            {
                var player = _gameManager.Players[i];
                string text = $"Player {i + 1}: {player.Score}";
                DrawHelper.DrawCenteredString(spriteBatch, Game1.Font, text, new Vector2(centerX, 500 + i * 100), player.Color);
            }

            if (_gameManager.PlayerCount > 1)
            {
                var winner = _gameManager.GetWinner();
                string winText = _gameManager.Players[0].Score == _gameManager.Players[1].Score
                    ? "TIE!"
                    : $"Player {winner.Id} wins!";
                DrawHelper.DrawCenteredString(spriteBatch, Game1.Font, winText, new Vector2(centerX, 760), Color.Yellow);
            }

            DrawButton(spriteBatch, _restartButton, "RESTART");
            DrawButton(spriteBatch, _menuButton, "MENU");
        }

        private void DrawButton(SpriteBatch spriteBatch, Rectangle rect, string text)
        {
            DrawHelper.DrawRectangle(spriteBatch, rect, Color.White * 0.2f);
            DrawHelper.DrawBorder(spriteBatch, rect, Color.White, 4);
            DrawHelper.DrawCenteredString(spriteBatch, Game1.Font, text, new Vector2(rect.Center.X, rect.Center.Y), Color.White);
        }
    }
}
