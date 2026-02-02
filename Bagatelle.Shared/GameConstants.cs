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

        // Peg
        public const float PegRadius = 6f;

        // Hole
        public const float HoleRadius = 20f;

        // Launcher
        public const float MaxChargeTime = 2f; // seconds

        // Colors
        public static readonly Color Player1Color = new Color(76, 175, 80);   // Green
        public static readonly Color Player2Color = new Color(33, 150, 243);  // Blue
        public static readonly Color BoardColor = new Color(139, 90, 43);     // Wood brown
        public static readonly Color PegColor = new Color(192, 192, 192);     // Silver
        public static readonly Color HoleColor = new Color(40, 40, 40);       // Dark
    }
}
