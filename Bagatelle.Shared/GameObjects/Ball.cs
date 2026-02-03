using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bagatelle.Shared.UI;

namespace Bagatelle.Shared.GameObjects
{
    public class Ball
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Color Color { get; }
        public float Radius { get; }
        public bool IsActive { get; set; }
        public bool IsInHole { get; set; }

        public Ball(Vector2 position, Color color)
        {
            Position = position;
            Color = color;
            Radius = GameConstants.BallRadius;
            Velocity = Vector2.Zero;
            IsActive = false;
            IsInHole = false;
        }

        public void Update(float deltaTime)
        {
            if (!IsActive) return;

            // Always apply gravity
            Velocity += new Vector2(0, GameConstants.Gravity * deltaTime);
            Position += Velocity * deltaTime;
        }

        public void Launch(float power)
        {
            IsActive = true;
            Velocity = new Vector2(0, -power);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawHelper.DrawCircle(spriteBatch, Position, Radius, Color);
        }
    }
}
