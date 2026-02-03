using Bagatelle.Shared.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bagatelle.Shared.GameObjects
{
    public class Hole
    {
        public Vector2 Position { get; }
        public float Radius { get; }
        public int Points { get; }

        public Ball Occupant { get; set; }

        public Hole(Vector2 position, int points)
        {
            Position = position;
            Points = points;
            Radius = GameConstants.HoleRadius;
            Occupant = null;
        }

        public bool Contains(Ball ball)
        {
            float distance = Vector2.Distance(Position, ball.Position);
            // Relaxed condition: check with slightly larger radius for physics interactions
            return distance < Radius * 1.2f;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawHelper.DrawCircle(spriteBatch, Position, Radius, GameConstants.HoleColor);
            DrawHelper.DrawCenteredString(spriteBatch, Game1.FontSmall, Points.ToString(), Position - new Vector2(0, 30), Color.White);
        }
    }
}
