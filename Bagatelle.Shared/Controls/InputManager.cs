using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Bagatelle.Shared.Controls
{
    public static class InputManager
    {
        private static KeyboardState _previousKeyboard;
        private static KeyboardState _currentKeyboard;
        private static MouseState _previousMouse;
        private static MouseState _currentMouse;
        
        // Resolution scaling
        private static float _scale = 1.0f;
        private static Vector2 _offset = Vector2.Zero;

        public static void SetScale(float scale, Vector2 offset)
        {
            _scale = scale;
            _offset = offset;
            TouchInput.SetScale(scale, offset);
        }

        public static void Update(bool isActive)
        {
            _previousKeyboard = _currentKeyboard;
            _previousMouse = _currentMouse;

            _currentKeyboard = Keyboard.GetState();
            _currentMouse = Mouse.GetState();

            TouchInput.Update(isActive);
        }

        // Helper to transform screen position to game position
        private static Vector2 TransformPosition(Point screenPoint)
        {
            return (new Vector2(screenPoint.X, screenPoint.Y) - _offset) / _scale;
        }
        
        private static bool IsPointInArea(Point screenPoint, Rectangle gameArea)
        {
            Vector2 gamePos = TransformPosition(screenPoint);
            return gameArea.Contains(gamePos);
        }

        // Charging (hold to charge power)
        public static bool IsCharging()
        {
            return IsKeyHeld(Keys.Space) ||
                   IsMouseLeftHeld() ||
                   TouchInput.IsPressedAnywhere();
        }

        public static bool WasChargingReleased()
        {
            return WasKeyReleased(Keys.Space) ||
                   WasMouseLeftReleased() ||
                   TouchInput.WasReleasedAnywhere();
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

        // Keyboard
        public static bool IsKeyHeld(Keys key) => _currentKeyboard.IsKeyDown(key);

        public static bool WasKeyPressed(Keys key) =>
            _currentKeyboard.IsKeyDown(key) && !_previousKeyboard.IsKeyDown(key);

        public static bool WasKeyReleased(Keys key) =>
            !_currentKeyboard.IsKeyDown(key) && _previousKeyboard.IsKeyDown(key);

        // Mouse
        public static bool IsMouseLeftHeld() =>
            _currentMouse.LeftButton == ButtonState.Pressed;

        public static bool WasMouseLeftPressed() =>
            _currentMouse.LeftButton == ButtonState.Pressed &&
            _previousMouse.LeftButton == ButtonState.Released;

        public static bool WasMouseLeftReleased() =>
            _currentMouse.LeftButton == ButtonState.Released &&
            _previousMouse.LeftButton == ButtonState.Pressed;

        public static bool IsMouseClickedInArea(Rectangle area) =>
            WasMouseLeftReleased() && IsPointInArea(_currentMouse.Position, area);

        public static bool IsMouseLeftHeldInArea(Rectangle area) =>
            IsMouseLeftHeld() && IsPointInArea(_currentMouse.Position, area);

        public static Point MousePosition => _currentMouse.Position; // Raw screen position

        // Navigation
        public static bool WasConfirmPressed() =>
            WasKeyPressed(Keys.Enter) || WasKeyPressed(Keys.Space) ||
            WasMouseLeftPressed() || TouchInput.WasReleasedAnywhere();

        public static bool WasBackPressed() =>
            WasKeyPressed(Keys.Escape) || WasKeyPressed(Keys.Back);
    }
}
