using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bagatelle.Shared.Controls;
using Bagatelle.Shared.GameObjects;
using Bagatelle.Shared.Logic;
using Bagatelle.Shared.UI;

namespace Bagatelle.Shared.Screens
{
    public class PlayingScreen : BaseScreen
    {
        private Board _board;
        private GameManager _gameManager;
        private Hud _hud;
        private readonly int _playerCount;
        private Rectangle _menuButton;

        public PlayingScreen(Game game, int playerCount) : base(game)
        {
            _playerCount = playerCount;
        }

        public override void LoadContent()
        {
            _board = new Board();
            _gameManager = new GameManager(_playerCount, _board);
            _hud = new Hud(_gameManager);
            
            // Menu button at top, centered
            _menuButton = new Rectangle(GameConstants.ScreenWidth / 2 - 35, 10, 70, 30);
        }

        public override void Update(GameTime gameTime)
        {
            InputManager.Update(Game.IsActive);
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Check menu button
            if (InputManager.IsButtonPressed(_menuButton))
            {
                Game1.Screens.SetScreen(new MenuScreen(Game));
                return;
            }

            if (_gameManager.State == GameState.GameOver)
            {
                // Don't auto-transition, just wait for user to tap to continue
                if (InputManager.WasConfirmPressed())
                    Game1.Screens.SetScreen(new GameOverScreen(Game, _gameManager));
                return;
            }

            HandleInput(deltaTime);
            _gameManager.Update(deltaTime);
        }

        private void HandleInput(float deltaTime)
        {
            if (_gameManager.State != GameState.WaitingToLaunch) return;

            if (InputManager.IsCharging())
            {
                if (!_board.Launcher.IsCharging)
                    _board.Launcher.StartCharging();
                _board.Launcher.UpdateCharge(deltaTime);
            }
            else if (InputManager.WasChargingReleased() && _board.Launcher.IsCharging)
            {
                float power = _board.Launcher.ReleaseCharge();
                _gameManager.LaunchBall(power);
            }


        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawHelper.DrawRectangle(spriteBatch, new Rectangle(0, 0, GameConstants.ScreenWidth, GameConstants.ScreenHeight), GameConstants.BoardDarkColor);

            _board.Draw(spriteBatch);
            
            // Draw all active balls
            foreach (var ball in _gameManager.BallsOnBoard)
            {
                ball.Draw(spriteBatch);
            }
            
            // Draw current ball if not in list (e.g. waiting to launch)
            if (_gameManager.State == GameState.WaitingToLaunch && _gameManager.CurrentBall != null)
            {
                 _gameManager.CurrentBall.Draw(spriteBatch);
            }

            _hud.Draw(spriteBatch);
            
            // Draw menu button
            DrawHelper.DrawRectangle(spriteBatch, _menuButton, Color.Black * 0.5f);
            DrawHelper.DrawBorder(spriteBatch, _menuButton, Color.White, 1);
            DrawHelper.DrawCenteredString(spriteBatch, Game1.FontSmall, "MENU", new Vector2(_menuButton.Center.X, _menuButton.Center.Y), Color.White);
        }
    }
}
