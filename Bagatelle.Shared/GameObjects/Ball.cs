using Bagatelle.Shared.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        public float TimeAtLowSpeed { get; set; }

        public Ball(Vector2 position, Color color)
        {
            Position = position;
            Color = color;
            Radius = GameConstants.BallRadius;
            Velocity = Vector2.Zero;
            IsActive = false;
            IsInHole = false;
            TimeAtLowSpeed = 0f;
        }

        public void Update(float deltaTime)
        {
            if (!IsActive) return;

            // Apply gravity only when NOT locked in hole
            if (!IsInHole)
            {
                Velocity += new Vector2(0, GameConstants.Gravity * deltaTime);
            }

            Position += Velocity * deltaTime;

            // Track how long ball has been moving slowly
            if (Velocity.Length() < GameConstants.BallLowSpeedThreshold)
                TimeAtLowSpeed += deltaTime;
            else
                TimeAtLowSpeed = 0f;
        }

        public void Launch(float power)
        {
            IsActive = true;
            IsInHole = false; // Ensure ball is not marked as in hole when launched
            TimeAtLowSpeed = 0f;
            Velocity = new Vector2(0, -power);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawHelper.DrawCircle(spriteBatch, Position, Radius, Color);
        }
    }
}
