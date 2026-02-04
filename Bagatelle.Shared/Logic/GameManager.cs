using Bagatelle.Shared.GameObjects;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Bagatelle.Shared.Logic
{
    public enum GameState
    {
        WaitingToLaunch,
        BallInPlay,
        GameOver
    }

    public class GameManager
    {
        public Player[] Players { get; }
        public int PlayerCount => Players.Length;
        public int CurrentPlayerIndex { get; private set; }
        public Player CurrentPlayer => Players[CurrentPlayerIndex];
        public GameState State { get; private set; }
        public Ball CurrentBall { get; private set; }
        public int LastScore { get; private set; }

        public List<Ball> BallsOnBoard { get; } = new List<Ball>();

        private readonly Board _board;
        private bool _ballEnteredPlayArea;

        public GameManager(int playerCount, Board board)
        {
            _board = board;

            Players = new Player[playerCount];
            Players[0] = new Player(1, GameConstants.Player1Color, Game1.BlueBallSprite);
            if (playerCount > 1)
                Players[1] = new Player(2, GameConstants.Player2Color, Game1.RedBallSprite);

            CurrentPlayerIndex = 0;
            State = GameState.WaitingToLaunch;
            PrepareBall();
        }

        private void PrepareBall()
        {
            var startPos = _board.GetBallStartPosition();
            CurrentBall = new Ball(startPos, CurrentPlayer.Color, CurrentPlayer.Sprite);
        }

        public void LaunchBall(float power)
        {
            if (State != GameState.WaitingToLaunch) return;

            CurrentBall.Launch(power);
            CurrentPlayer.UseBall();
            State = GameState.BallInPlay;
            BallsOnBoard.Add(CurrentBall);
            _ballEnteredPlayArea = false;
        }

        public void Update(float deltaTime)
        {
            if (State != GameState.BallInPlay && State != GameState.WaitingToLaunch) return;

            // Update all active balls
            // We need to iterate backwards in case we remove balls, but for now we only add
            foreach (var ball in BallsOnBoard)
            {
                ball.Update(deltaTime);

                // Ball-Board Collisions
                Physics.HandleBoardCollision(ball, _board);

                // Ball-Peg Collisions
                foreach (var peg in _board.Pegs)
                {
                    Physics.HandlePegCollision(ball, peg);
                }

                // Ball-Ball Collisions
                foreach (var other in BallsOnBoard)
                {
                    if (ball == other) continue;
                    Physics.HandleBallCollision(ball, other);

                    // If collision happened, we might have knocked a ball out of a hole
                    // We need to update hole occupancy
                    // We can do this by checking distance to holes in next frame or here
                }
            }

            // Cleanup Hole Occupancy
            foreach (var hole in _board.Holes)
            {
                if (hole.Occupant != null)
                {
                    // If occupant moved away or is moving, it's no longer occupying
                    float dist = Vector2.Distance(hole.Occupant.Position, hole.Position);
                    if (dist > 3f || hole.Occupant.Velocity.Length() > 50f || !hole.Occupant.IsInHole)
                    {
                        hole.Occupant = null;
                    }
                }
            }

            // Special handling for the Current Ball logic (Turn ending)
            if (State == GameState.BallInPlay)
            {
                // Track if ball has truly entered the main play area
                // Must be well into play area (not just touching edge) AND below arc level
                bool inPlayAreaX = CurrentBall.Position.X < _board.ChannelWallX - 30;
                bool belowArc = CurrentBall.Position.Y > _board.ArcCenter.Y;
                if (inPlayAreaX && belowArc)
                {
                    _ballEnteredPlayArea = true;
                }

                // If ball never properly entered play area, don't end turn
                // Just wait for ball to return to launcher or enter play area
                if (!_ballEnteredPlayArea)
                {
                    // Check if ball stopped in launcher
                    if (Physics.IsBallInLauncher(CurrentBall, _board) && Physics.IsBallStopped(CurrentBall))
                    {
                        BallsOnBoard.Remove(CurrentBall);

                        CurrentBall.Position = _board.GetBallStartPosition();
                        CurrentBall.Velocity = Vector2.Zero;
                        CurrentBall.IsInHole = false;
                        CurrentBall.IsActive = false;

                        CurrentPlayer.ReturnBall();
                        State = GameState.WaitingToLaunch;
                    }
                    return;
                }

                // Interaction with holes
                // Balls settle in holes physically now.

                bool allStopped = true;
                foreach (var ball in BallsOnBoard)
                {
                    if (!Physics.IsBallStopped(ball))
                    {
                        allStopped = false;
                    }

                    // Check if ball is in hole area
                    var hole = Physics.GetHoleContaining(ball, _board.Holes);
                    if (hole != null)
                    {
                        // Apply hole physics (trap)
                        Physics.ApplyHoleTrap(ball, hole);
                    }
                    else
                    {
                        ball.IsInHole = false;
                    }
                }

                if (allStopped)
                {
                    // If Current Ball is stopped (and not in launcher, handled above)
                    // End Turn.
                    // Calculate Score for new settlements?
                    // Let's assume points are awarded when the ball settles in a hole.
                    // To avoid double counting, we could calculate score at end of turn based on where it is.

                    // Wait, if next ball knocks it out, do we lose points?
                    // Usually yes. Score is based on final position.
                    // So we update score continuously or at end of all balls?
                    // Standard: Score is sum of balls in holes.

                    UpdateScores();
                    EndTurn();
                }
            }
        }

        private void UpdateScores()
        {
            // Reset scores and recalculate based on current board state
            // Or just add delta?
            // "Další kulička ji může vystrnadit" -> implied dynamic score.

            foreach (var p in Players) p.Score = 0;

            foreach (var ball in BallsOnBoard)
            {
                var hole = Physics.GetHoleContaining(ball, _board.Holes);
                if (hole != null && Physics.IsBallStopped(ball))
                {
                    // Find owner
                    foreach (var p in Players)
                    {
                        if (p.Color == ball.Color)
                        {
                            p.Score += hole.Points;
                        }
                    }
                }
            }
        }

        private void EndTurn()
        {
            // CurrentBall is already in BallsOnBoard list.

            // Check game over
            bool anyPlayerHasBalls = false;
            foreach (var player in Players)
            {
                if (player.HasBalls)
                {
                    anyPlayerHasBalls = true;
                    break;
                }
            }

            if (!anyPlayerHasBalls)
            {
                State = GameState.GameOver;
                UpdateScores(); // Final check
                return;
            }

            // Next player
            NextPlayer();
            PrepareBall();
            State = GameState.WaitingToLaunch;
        }

        private void NextPlayer()
        {
            int startIndex = CurrentPlayerIndex;
            do
            {
                CurrentPlayerIndex = (CurrentPlayerIndex + 1) % Players.Length;
            }
            while (!CurrentPlayer.HasBalls && CurrentPlayerIndex != startIndex);
        }

        public Player GetWinner()
        {
            if (Players.Length == 1) return Players[0];

            return Players[0].Score >= Players[1].Score ? Players[0] : Players[1];
        }
    }
}
