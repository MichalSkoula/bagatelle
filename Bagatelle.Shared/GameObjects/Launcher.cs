using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bagatelle.Shared.UI;

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
            DrawHelper.DrawRectangle(spriteBatch, ChargeArea, GameConstants.PegColor);

            if (IsCharging)
            {
                int chargeWidth = (int)(ChargeArea.Width * ChargePower);
                var chargeRect = new Rectangle(ChargeArea.X, ChargeArea.Y - 10, chargeWidth, 8);
                var chargeColor = Color.Lerp(Color.Green, Color.Red, ChargePower);
                DrawHelper.DrawRectangle(spriteBatch, chargeRect, chargeColor);
            }
        }
    }
}
