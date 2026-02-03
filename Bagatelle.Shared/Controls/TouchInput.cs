using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bagatelle.Shared.Controls
{
    public static class TouchInput
    {
        private static List<TouchLocation> _previousTouches = new();
        private static readonly List<TouchLocation> _currentTouches = new();

        // Resolution scaling
        private static float _scale = 1.0f;
        private static Vector2 _offset = Vector2.Zero;

        public static bool Available { get; private set; }
        private static bool _capabilityProbed;

        public static void SetScale(float scale, Vector2 offset)
        {
            _scale = scale;
            _offset = offset;
        }

        public static void Update(bool isActive)
        {
            _previousTouches = new List<TouchLocation>(_currentTouches);
            _currentTouches.Clear();

            if (!isActive) return;

            ProbeCapability();

            foreach (var touch in TouchPanel.GetState())
            {
                if (touch.State != TouchLocationState.Invalid)
                    _currentTouches.Add(touch);
            }

            if (_currentTouches.Any(t => t.State != TouchLocationState.Invalid))
                Available = true;
        }

        private static void ProbeCapability()
        {
            if (_capabilityProbed) return;
            _capabilityProbed = true;

            try
            {
                var getCaps = typeof(TouchPanel).GetMethod("GetCapabilities", BindingFlags.Public | BindingFlags.Static);
                if (getCaps == null) return;

                var caps = getCaps.Invoke(null, null);
                if (caps == null) return;

                var prop = caps.GetType().GetProperty("IsConnected")
                    ?? caps.GetType().GetProperty("IsAvailable")
                    ?? caps.GetType().GetProperty("TouchPresent");

                if (prop?.GetValue(caps) is bool b && b)
                    Available = true;
            }
            catch { /* Ignore probing errors */ }
        }

        private static Vector2 TransformPosition(Vector2 pos)
        {
            return (pos - _offset) / _scale;
        }

        public static bool IsPressed(Rectangle hitbox)
        {
            if (!Available) return false;

            return _currentTouches.Any(t =>
                hitbox.Contains(TransformPosition(t.Position).ToPoint()) &&
                (t.State == TouchLocationState.Pressed || t.State == TouchLocationState.Moved));
        }

        public static bool WasReleased(Rectangle hitbox)
        {
            if (!Available) return false;

            // Check if it was pressed inside before
            var wasPressed = _previousTouches.Any(t =>
                hitbox.Contains(TransformPosition(t.Position).ToPoint()) &&
                (t.State == TouchLocationState.Pressed || t.State == TouchLocationState.Moved));

            // Check if it's released now (anywhere, but logic implies continuity)
            // Or if a released event happened inside
            var isReleased = _currentTouches.Any(t =>
                hitbox.Contains(TransformPosition(t.Position).ToPoint()) &&
                t.State == TouchLocationState.Released);

            return isReleased;
        }

        public static bool IsPressedAnywhere()
        {
            if (!Available) return false;
            return _currentTouches.Any(t =>
                t.State == TouchLocationState.Pressed || t.State == TouchLocationState.Moved);
        }

        public static bool WasReleasedAnywhere()
        {
            if (!Available) return false;
            return _currentTouches.Any(t => t.State == TouchLocationState.Released);
        }

        public static Vector2? GetTouchPosition()
        {
            var touch = _currentTouches.FirstOrDefault(t =>
                t.State == TouchLocationState.Pressed || t.State == TouchLocationState.Moved);

            return touch.State != TouchLocationState.Invalid ? TransformPosition(touch.Position) : null;
        }
    }
}
