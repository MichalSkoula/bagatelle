using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bagatelle.Shared.UI
{
    public static class DrawHelper
    {
        private static Texture2D _pixel;

        public static void Initialize(GraphicsDevice device)
        {
            _pixel = new Texture2D(device, 1, 1);
            _pixel.SetData(new[] { Color.White });
        }

        public static Texture2D Pixel => _pixel;

        public static void DrawCircle(SpriteBatch sb, Vector2 center, float radius, Color color)
        {
            int diameter = (int)(radius * 2);
            for (int y = 0; y < diameter; y++)
            {
                for (int x = 0; x < diameter; x++)
                {
                    float dx = x - radius;
                    float dy = y - radius;
                    if (dx * dx + dy * dy <= radius * radius)
                    {
                        sb.Draw(_pixel, new Rectangle((int)(center.X - radius + x), (int)(center.Y - radius + y), 1, 1), color);
                    }
                }
            }
        }

        public static void DrawRectangle(SpriteBatch sb, Rectangle rect, Color color)
        {
            sb.Draw(_pixel, rect, color);
        }

        public static void DrawBorder(SpriteBatch sb, Rectangle rect, Color color, int thickness)
        {
            sb.Draw(_pixel, new Rectangle(rect.X, rect.Y, rect.Width, thickness), color);
            sb.Draw(_pixel, new Rectangle(rect.X, rect.Bottom - thickness, rect.Width, thickness), color);
            sb.Draw(_pixel, new Rectangle(rect.X, rect.Y, thickness, rect.Height), color);
            sb.Draw(_pixel, new Rectangle(rect.Right - thickness, rect.Y, thickness, rect.Height), color);
        }

        public static void DrawLine(SpriteBatch sb, Vector2 p1, Vector2 p2, int thickness, Color color)
        {
            Vector2 edge = p2 - p1;
            float angle = (float)System.Math.Atan2(edge.Y, edge.X);
            float length = edge.Length();

            sb.Draw(_pixel, p1, null, color, angle, new Vector2(0, 0.5f), new Vector2(length, thickness), SpriteEffects.None, 0);
        }

        public static void DrawCenteredString(SpriteBatch sb, SpriteFont font, string text, Vector2 position, Color color)
        {
            var size = font.MeasureString(text);
            sb.DrawString(font, text, position - size / 2, color);
        }
    }
}
