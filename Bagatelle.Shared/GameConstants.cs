using Microsoft.Xna.Framework;

namespace Bagatelle.Shared
{
    public static class GameConstants
    {
        // Screen
        public const int ScreenWidth = 480;
        public const int ScreenHeight = 800;

        // Physics
        public const float Gravity = 400f;
        public const float MaxLaunchPower = 1300f;
        public const float BounceRestitution = 0.7f;

        // Game rules
        public const int BallsPerPlayer = 5;

        // Ball
        public const float BallRadius = 10f;
        public const float BallLowSpeedThreshold = 50f; // For time tracking
        public const float BallStoppedTimeThreshold = 0.5f; // Seconds ball must be slow to be "stopped"

        // Peg
        public const float PegRadius = 6f;

        // Hole
        public const float HoleRadius = 20f;
        public const float HoleAttractionRadius = 1.15f; // Multiplier of HoleRadius - smaller = harder
        public const float HoleEscapeSpeed = 500f; // Balls faster than this can escape - lower = harder
        public const float HoleInsidePullStrength = 4500f; // Pull force when inside hole
        public const float HoleOutsidePullStrengthBase = 1500f; // Base pull when approaching
        public const float HoleOutsidePullStrengthBonus = 1000f; // Bonus for slow balls
        public const float HoleInsideFriction = 0.80f; // Damping when in hole - higher = less damping
        public const float HoleOutsideFriction = 0.92f; // Damping when approaching
        public const float HoleSnapSpeed = 120f; // Speed threshold for snapping to center

        // Launcher
        public const float MaxChargeTime = 2f; // seconds

        // Colors
        public static readonly Color Player1Color = new Color(76, 175, 80);   // Green
        public static readonly Color Player2Color = new Color(33, 150, 243);  // Blue
        public static readonly Color BoardColor = new Color(139, 90, 43);     // Wood brown
        public static readonly Color BoardDarkColor = new Color(60, 40, 30);  // Darker wood
        public static readonly Color PegColor = new Color(192, 192, 192);     // Silver
        public static readonly Color HoleColor = new Color(40, 40, 40);       // Dark
    }
}
