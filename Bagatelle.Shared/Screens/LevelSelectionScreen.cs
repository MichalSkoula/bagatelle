using Bagatelle.Shared.Controls;
using Bagatelle.Shared.GameObjects.Boards;
using Bagatelle.Shared.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bagatelle.Shared.Screens
{
    public class LevelSelectionScreen : BaseScreen
    {
        private List<LevelButton> _levelButtons;
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

            _levelButtons = new List<LevelButton>();

            // Find all non-abstract classes that inherit from Board
            var boardTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Board)))
                .ToList();

            var tempBoards = new List<(Board Instance, Type Type)>();
            foreach (var type in boardTypes)
            {
                tempBoards.Add(((Board)Activator.CreateInstance(type), type));
            }

            tempBoards = tempBoards.OrderBy(b => b.Instance.Index).ToList();

            int currentY = startY;
            foreach (var b in tempBoards)
            {
                _levelButtons.Add(new LevelButton
                {
                    Rect = new Rectangle(centerX, currentY, buttonWidth, buttonHeight),
                    Name = b.Instance.Name,
                    BoardType = b.Type
                });

                currentY += spacing;
            }

            // Place back button at fixed Y
            _backButton = new Rectangle(centerX, 1240, buttonWidth, buttonHeight);

            _frameCount = 0;
        }

        public override void Update(GameTime gameTime)
        {
            InputManager.Update(Game.IsActive);
            _frameCount++;

            if (_frameCount < LimitFrames)
                return;

            foreach (var btn in _levelButtons)
            {
                if (InputManager.IsButtonPressed(btn.Rect))
                {
                    Game1.Screens.SetScreen(new PlayingScreen(Game, _playerCount, btn.BoardType));
                    return;
                }
            }

            if (InputManager.IsButtonPressed(_backButton))
            {
                Game1.Screens.SetScreen(new MenuScreen(Game));
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawHelper.DrawRectangle(spriteBatch, new Rectangle(0, 0, GameConstants.ScreenWidth, GameConstants.ScreenHeight), GameConstants.BoardDarkColor);

            DrawHelper.DrawCenteredString(spriteBatch, Game1.FontLarge, "SELECT LEVEL", new Vector2(GameConstants.ScreenWidth / 2, 300), Color.White);

            foreach (var btn in _levelButtons)
            {
                DrawButton(spriteBatch, btn.Rect, btn.Name);
            }

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
