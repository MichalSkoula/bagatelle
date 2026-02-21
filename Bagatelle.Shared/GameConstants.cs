using Microsoft.Xna.Framework;

namespace Bagatelle.Shared
{
    public static class GameConstants
    {
        // Screen (2x for sharp rendering on mobile)
        public const int ScreenWidth = 960;
        public const int ScreenHeight = 1600;

        // Physics (scaled 2x for new resolution)
        public const float Gravity = 800f;
        public const float MaxLaunchPower = 2600f;
        public const float BounceRestitution = 0.7f;

        // Game rules
        public const int BallsPerPlayer = 5;

        // Ball
        public const float BallRadius = 20f;
        public const float BallLowSpeedThreshold = 100f; // For time tracking
        public const float BallStoppedTimeThreshold = 0.5f; // Seconds ball must be slow to be "stopped"

        // Peg
        public const float PegRadius = 10f;

        // Hole
        public const float HoleRadius = 24f;
        public const float HoleAttractionRadius = 1.15f; // Multiplier of HoleRadius - smaller = harder
        public const float HoleEscapeSpeed = 1000f; // Balls faster than this can escape - lower = harder
        public const float HoleInsidePullStrength = 9000f; // Pull force when inside hole
        public const float HoleOutsidePullStrengthBase = 3000f; // Base pull when approaching
        public const float HoleOutsidePullStrengthBonus = 2000f; // Bonus for slow balls
        public const float HoleInsideFriction = 0.80f; // Damping when in hole - higher = less damping
        public const float HoleOutsideFriction = 0.92f; // Damping when approaching
        public const float HoleSnapSpeed = 240f; // Speed threshold for snapping to center

        // Launcher
        public const float MaxChargeTime = 2f; // seconds

        // Colors
        public static readonly Color Player1Color = Color.RoyalBlue;               // Player 1 = Blue
        public static readonly Color Player2Color = Color.Firebrick;          // Player 2 = Red
        public static readonly Color BoardColor = new Color(139, 90, 43);     // Wood brown
        public static readonly Color BoardDarkColor = new Color(60, 40, 30);  // Darker wood
        public static readonly Color PegColor = new Color(192, 192, 192);     // Silver
        public static readonly Color HoleColor = new Color(40, 40, 40);       // Dark
    }
}
