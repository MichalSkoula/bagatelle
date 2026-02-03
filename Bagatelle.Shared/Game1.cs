using Bagatelle.Shared.Controls;
using Bagatelle.Shared.Screens;
using Bagatelle.Shared.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Bagatelle.Shared
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static ScreenController Screens { get; private set; }
        public static SpriteFont Font { get; private set; }
        public static SpriteFont FontSmall { get; private set; }

        public static SpriteFont FontLarge { get; private set; }

        public GraphicsDeviceManager GetGraphicsDeviceManager() => _graphics;

        // Resolution handling
        private Matrix _scaleMatrix;
        private Vector2 _screenOffset;
        private float _scale;

#if WINDOWS
        private int _windowedWidth = GameConstants.ScreenWidth;
        private int _windowedHeight = GameConstants.ScreenHeight;
#endif

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Allow resizing on desktop to test responsiveness
            Window.AllowUserResizing = false;
            Window.ClientSizeChanged += OnResize;
        }

        protected override void Initialize()
        {
#if WINDOWS
            _graphics.PreferredBackBufferWidth = GameConstants.ScreenWidth;
            _graphics.PreferredBackBufferHeight = GameConstants.ScreenHeight;
            _graphics.ApplyChanges();
#else
            // On Android, let it use the device resolution
            _graphics.IsFullScreen = true;
            _graphics.SupportedOrientations = DisplayOrientation.Portrait;
            _graphics.ApplyChanges();
#endif
            CalculateScale();

            base.Initialize();
        }

        private void OnResize(object sender, System.EventArgs e)
        {
#if WINDOWS
            // Save windowed size when not in fullscreen
            if (!_graphics.IsFullScreen)
            {
                _windowedWidth = Window.ClientBounds.Width;
                _windowedHeight = Window.ClientBounds.Height;
            }
#endif
            CalculateScale();
        }

#if WINDOWS
        public void ToggleFullscreen()
        {
            if (_graphics.IsFullScreen)
            {
                // Switch to windowed
                _graphics.IsFullScreen = false;
                _graphics.PreferredBackBufferWidth = _windowedWidth;
                _graphics.PreferredBackBufferHeight = _windowedHeight;
            }
            else
            {
                // Switch to fullscreen at desktop resolution
                _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                _graphics.IsFullScreen = true;
            }
            _graphics.ApplyChanges();
        }
#endif

        private void CalculateScale()
        {
            float screenWidth = GraphicsDevice.Viewport.Width;
            float screenHeight = GraphicsDevice.Viewport.Height;

            // Calculate scale to fit while maintaining aspect ratio
            float scaleX = screenWidth / GameConstants.ScreenWidth;
            float scaleY = screenHeight / GameConstants.ScreenHeight;
            _scale = System.Math.Min(scaleX, scaleY);

            // Calculate centering offset
            float targetWidth = GameConstants.ScreenWidth * _scale;
            float targetHeight = GameConstants.ScreenHeight * _scale;

            _screenOffset = new Vector2(
                (screenWidth - targetWidth) / 2f,
                (screenHeight - targetHeight) / 2f
            );

            _scaleMatrix = Matrix.CreateScale(_scale) *
                           Matrix.CreateTranslation(new Vector3(_screenOffset, 0));

            // Update InputManager with the new scale/offset
            InputManager.SetScale(_scale, _screenOffset);

            // Ensure TouchPanel knows the actual screen size (critical for Android)
            TouchPanel.DisplayWidth = (int)screenWidth;
            TouchPanel.DisplayHeight = (int)screenHeight;
            TouchPanel.EnableMouseTouchPoint = true;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            DrawHelper.Initialize(GraphicsDevice);
            Font = Content.Load<SpriteFont>("GameFont");
            FontSmall = Content.Load<SpriteFont>("GameFontSmall");
            FontLarge = Content.Load<SpriteFont>("GameFontLarge");

            Screens = new ScreenController(this);
            Screens.SetScreen(new IntroScreen(this));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            Screens.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Clear to BoardColor to match the game board (hides letterboxing)
            GraphicsDevice.Clear(GameConstants.BoardDarkColor);

            // Draw screens with the scale matrix
            _spriteBatch.Begin(transformMatrix: _scaleMatrix);
            Screens.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
