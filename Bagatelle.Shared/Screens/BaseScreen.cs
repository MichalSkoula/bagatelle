using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bagatelle.Shared.Screens
{
    public abstract class BaseScreen
    {
        protected Game Game { get; }
        protected GraphicsDevice GraphicsDevice => Game.GraphicsDevice;

        // Enable clicking after a short delay to avoid accidental inputs
        protected int LimitFrames = 30;

        protected BaseScreen(Game game)
        {
            Game = game;
        }

        public virtual void LoadContent() { }
        public virtual void UnloadContent() { }
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
