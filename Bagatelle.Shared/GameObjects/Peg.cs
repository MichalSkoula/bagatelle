using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
            Texture2D sprite = Game1.PegSprite;
            float scale = (Radius * 2) / sprite.Width;
            spriteBatch.Draw(
                sprite,
                Position,
                null,
                Color.White,
                0f,
                new Vector2(sprite.Width / 2f, sprite.Height / 2f),
                scale,
                SpriteEffects.None,
                0f
            );
        }
    }
}
