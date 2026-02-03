using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Bagatelle.Shared.Controls
{
    public static class InputManager
    {
        private static MouseState _previousMouse;
        private static MouseState _currentMouse;
        
        // Resolution scaling
        private static float _scale = 1.0f;
        private static Vector2 _offset = Vector2.Zero;
        private static bool _isActive;

        public static void SetScale(float scale, Vector2 offset)
        {
            _scale = scale;
            _offset = offset;
            TouchInput.SetScale(scale, offset);
        }

        public static void Update(bool isActive)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();
            _isActive = isActive;

            TouchInput.Update(isActive);
        }

        // Helper to transform screen position to game position
        private static Vector2 TransformPosition(Point screenPoint)
        {
            return (new Vector2(screenPoint.X, screenPoint.Y) - _offset) / _scale;
        }
        
        private static bool IsPointInArea(Point screenPoint, Rectangle gameArea)
        {
            if (!_isActive)
                return false;
                
            Vector2 gamePos = TransformPosition(screenPoint);
            return gameArea.Contains(gamePos);
        }
        
        private static bool IsMouseInGameBounds(Point mousePos)
        {
            if (!_isActive)
                return false;
                
            Vector2 gamePos = TransformPosition(mousePos);
            Rectangle gameBounds = new Rectangle(0, 0, GameConstants.ScreenWidth, GameConstants.ScreenHeight);
            return gameBounds.Contains(gamePos);
        }

        // Charging (hold to charge power)
        public static bool IsCharging()
        {
            return IsMouseLeftHeld() || TouchInput.IsPressedAnywhere();
        }

        public static bool WasChargingReleased()
        {
            return WasMouseLeftReleased() || TouchInput.WasReleasedAnywhere();
        }

        // Button interactions
        public static bool IsButtonPressed(Rectangle hitbox)
        {
            return IsMouseClickedInArea(hitbox) || TouchInput.WasReleased(hitbox);
        }

        public static bool IsButtonHeld(Rectangle hitbox)
        {
            return IsMouseLeftHeldInArea(hitbox) || TouchInput.IsPressed(hitbox);
        }

        // Mouse
        public static bool IsMouseLeftHeld() =>
            _isActive && IsMouseInGameBounds(_currentMouse.Position) &&
            _currentMouse.LeftButton == ButtonState.Pressed;

        public static bool WasMouseLeftPressed() =>
            _isActive && IsMouseInGameBounds(_currentMouse.Position) &&
            _currentMouse.LeftButton == ButtonState.Pressed &&
            _previousMouse.LeftButton == ButtonState.Released;

        public static bool WasMouseLeftReleased() =>
            _isActive && IsMouseInGameBounds(_currentMouse.Position) &&
            _currentMouse.LeftButton == ButtonState.Released &&
            _previousMouse.LeftButton == ButtonState.Pressed;

        public static bool IsMouseClickedInArea(Rectangle area) =>
            WasMouseLeftReleased() && IsPointInArea(_currentMouse.Position, area);

        public static bool IsMouseLeftHeldInArea(Rectangle area) =>
            IsMouseLeftHeld() && IsPointInArea(_currentMouse.Position, area);

        public static Point MousePosition => _currentMouse.Position;

        // Navigation
        public static bool WasConfirmPressed() =>
            WasMouseLeftPressed() || TouchInput.WasReleasedAnywhere();

        public static bool WasBackPressed() => false;
    }
}
