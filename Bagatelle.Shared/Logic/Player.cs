using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bagatelle.Shared.Logic
{
    public class Player
    {
        public int Id { get; }
        public Color Color { get; }
        public Texture2D Sprite { get; }
        public int Score { get; set; }
        public int BallsRemaining { get; private set; }

        public Player(int id, Color color, Texture2D sprite)
        {
            Id = id;
            Color = color;
            Sprite = sprite;
            Score = 0;
            BallsRemaining = GameConstants.BallsPerPlayer;
        }

        public void AddScore(int points)
        {
            Score += points;
        }

        public void UseBall()
        {
            if (BallsRemaining > 0)
                BallsRemaining--;
        }

        public void ReturnBall()
        {
            BallsRemaining++;
        }

        public bool HasBalls => BallsRemaining > 0;

        public void Reset()
        {
            Score = 0;
            BallsRemaining = GameConstants.BallsPerPlayer;
        }
    }
}
