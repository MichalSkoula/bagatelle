using Bagatelle.Shared.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bagatelle.Shared.GameObjects
{
    public class Launcher
    {
        public Vector2 Position { get; }
        public Rectangle ChargeArea { get; }
        public float ChargeTime { get; private set; }
        public bool IsCharging { get; private set; }

        private const int Width = 80;
        private const int Height = 40;

        public Launcher(Vector2 position)
        {
            Position = position;
            ChargeArea = new Rectangle(
                (int)(position.X - Width / 2),
                (int)position.Y,
                Width,
                Height
            );
        }

        public void StartCharging()
        {
            IsCharging = true;
            ChargeTime = 0;
        }

        public void UpdateCharge(float deltaTime)
        {
            if (!IsCharging) return;
            ChargeTime = MathHelper.Min(ChargeTime + deltaTime, GameConstants.MaxChargeTime);
        }

        public float ReleaseCharge()
        {
            IsCharging = false;
            float power = (ChargeTime / GameConstants.MaxChargeTime) * GameConstants.MaxLaunchPower;
            ChargeTime = 0;
            return power;
        }

        public void Reset()
        {
            IsCharging = false;
            ChargeTime = 0;
        }

        public float ChargePower => ChargeTime / GameConstants.MaxChargeTime;

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw full-width charge indicator at bottom
            if (IsCharging)
            {
                int margin = 10;
                int barHeight = 20;
                int barY = GameConstants.ScreenHeight - barHeight - margin;

                // Background bar
                var bgRect = new Rectangle(margin, barY, GameConstants.ScreenWidth - margin * 2, barHeight);
                DrawHelper.DrawRectangle(spriteBatch, bgRect, Color.Black * 0.5f);
                DrawHelper.DrawBorder(spriteBatch, bgRect, Color.White, 2);

                // Charge fill
                int chargeWidth = (int)((bgRect.Width - 4) * ChargePower);
                if (chargeWidth > 0)
                {
                    var chargeRect = new Rectangle(bgRect.X + 2, bgRect.Y + 2, chargeWidth, barHeight - 4);
                    var chargeColor = Color.Lerp(Color.Green, Color.Red, ChargePower);
                    DrawHelper.DrawRectangle(spriteBatch, chargeRect, chargeColor);
                }
            }
        }
    }
}
