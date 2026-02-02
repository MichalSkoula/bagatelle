using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bagatelle.Shared.UI;

namespace Bagatelle.Shared.GameObjects
{
    public class Peg
    {
        public Vector2 Position { get; }
        public float Radius { get; }

        public Peg(Vector2 position)
        {
            Position = position;
            Radius = GameConstants.PegRadius;
        }

        public bool CollidesWith(Ball ball)
        {
            float distance = Vector2.Distance(Position, ball.Position);
            return distance < Radius + ball.Radius;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawHelper.DrawCircle(spriteBatch, Position, Radius, GameConstants.PegColor);
        }
    }
}
