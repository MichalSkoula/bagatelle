using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Bagatelle.Shared.UI
{
    public static class DrawHelper
    {
        private static Texture2D _pixel;
        private static Dictionary<int, Texture2D> _circleCache = new Dictionary<int, Texture2D>();
        private static GraphicsDevice _device;

        public static void Initialize(GraphicsDevice device)
        {
            _device = device;
            _pixel = new Texture2D(device, 1, 1);
            _pixel.SetData(new[] { Color.White });
        }

        public static Texture2D Pixel => _pixel;

        public static void DrawCircle(SpriteBatch sb, Vector2 center, float radius, Color color)
        {
            int radiusKey = (int)radius;
            if (!_circleCache.ContainsKey(radiusKey))
            {
                _circleCache[radiusKey] = CreateCircleTexture(radiusKey);
            }

            var texture = _circleCache[radiusKey];
            sb.Draw(texture, new Vector2(center.X - radius, center.Y - radius), color);
        }

        private static Texture2D CreateCircleTexture(int radius)
        {
            int diameter = radius * 2;
            Texture2D texture = new Texture2D(_device, diameter, diameter);
            Color[] colorData = new Color[diameter * diameter];

            float radiusSquared = radius * radius;
            for (int y = 0; y < diameter; y++)
            {
                for (int x = 0; x < diameter; x++)
                {
                    float dx = x - radius;
                    float dy = y - radius;
                    int index = y * diameter + x;
                    colorData[index] = (dx * dx + dy * dy <= radiusSquared) ? Color.White : Color.Transparent;
                }
            }

            texture.SetData(colorData);
            return texture;
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
