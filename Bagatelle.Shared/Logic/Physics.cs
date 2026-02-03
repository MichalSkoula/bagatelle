using Microsoft.Xna.Framework;
using Bagatelle.Shared.GameObjects;
using System;
using System.Collections.Generic;

namespace Bagatelle.Shared.Logic
{
    public static class Physics
    {
        public static void HandleBoardCollision(Ball ball, Board board)
        {
            // If ball is very low, let it fall out (below launcher area)
            if (ball.Position.Y > board.MainArea.Bottom + 50) return;

            // 1. Check if inside Launch Channel (before it enters the arc)
            if (IsInLaunchChannel(ball, board))
            {
                HandleChannelCollision(ball, board.LaunchChannel);
                return;
            }

            // 2. Check Arc Collision (Top Semi-circle)
            // This applies if the ball is generally in the upper part of the board
            // and NOT in the channel.
            if (ball.Position.Y < board.ArcCenter.Y + 20)
            {
                if (HandleArcCollision(ball, board.ArcCenter, board.ArcRadius))
                {
                    // If we hit the arc, we don't hit the side walls (as arc covers sides at top)
                    // But we might be transiting from channel
                    return;
                }
            }

            // 3. Handle Side and Bottom Walls (Rectangular part)
            // Only if below the arc center level (where straight walls start)
            if (ball.Position.Y >= board.ArcCenter.Y)
            {
                HandleMainAreaWalls(ball, board.MainArea, board.ChannelWallX);
            }
        }

        public static bool IsInLaunchChannel(Ball ball, Board board)
        {
            // Ball is in channel if it's to the right of the separator wall
            // And below the top of the separator wall (where it opens to the arc)
            // Use >= for X to catch balls touching the wall
            return ball.Position.X >= board.ChannelWallX && 
                   ball.Position.Y > board.ChannelWallTopY;
        }

        private static void HandleChannelCollision(Ball ball, Rectangle channel)
        {
            // Left wall of channel (Separator)
            if (ball.Position.X - ball.Radius < channel.Left)
            {
                ball.Position = new Vector2(channel.Left + ball.Radius, ball.Position.Y);
                ball.Velocity = new Vector2(Math.Abs(ball.Velocity.X) * GameConstants.BounceRestitution, ball.Velocity.Y);
            }

            // Right wall of channel (Outer Board Wall)
            if (ball.Position.X + ball.Radius > channel.Right)
            {
                ball.Position = new Vector2(channel.Right - ball.Radius, ball.Position.Y);
                ball.Velocity = new Vector2(-Math.Abs(ball.Velocity.X) * GameConstants.BounceRestitution, ball.Velocity.Y);
            }

            // Bottom of channel (Launcher face)
            // Ball starts at Bottom - BallRadius, so floor should be slightly below that
            float launcherFloorY = channel.Bottom;
            
            if (ball.Position.Y + ball.Radius > launcherFloorY)
            {
                ball.Position = new Vector2(ball.Position.X, launcherFloorY - ball.Radius);
                // Damping bounce on the launcher
                ball.Velocity = new Vector2(ball.Velocity.X * 0.9f, -ball.Velocity.Y * 0.4f);
            }
        }

        private static bool HandleArcCollision(Ball ball, Vector2 arcCenter, float arcRadius)
        {
            Vector2 toBall = ball.Position - arcCenter;
            float distance = toBall.Length();
            float maxDist = arcRadius - ball.Radius;

            // Check if ball is outside the inner circle (hitting the wall)
            if (distance > maxDist)
            {
                // Only consider collision if ball is somewhat above/near the arc part
                // (Though with gravity it should be fine)
                
                if (distance > 0)
                {
                    toBall.Normalize();
                    
                    // Push ball inside
                    ball.Position = arcCenter + toBall * maxDist;

                    // Normal points IN to center (-toBall)
                    Vector2 normal = -toBall; 
                    
                    // Reflect velocity if moving OUT (against normal)
                    // normal points IN. Velocity OUT means Dot(v, n) < 0.
                    if (Vector2.Dot(ball.Velocity, normal) < 0) 
                    {
                        // Reflect
                        ball.Velocity = Vector2.Reflect(ball.Velocity, normal) * GameConstants.BounceRestitution;
                        return true;
                    }
                }
            }
            return false;
        }

        private static void HandleMainAreaWalls(Ball ball, Rectangle mainArea, int channelWallX)
        {
            // Left Outer Wall
            if (ball.Position.X - ball.Radius < mainArea.Left)
            {
                ball.Position = new Vector2(mainArea.Left + ball.Radius, ball.Position.Y);
                ball.Velocity = new Vector2(Math.Abs(ball.Velocity.X) * GameConstants.BounceRestitution, ball.Velocity.Y);
            }

            // Right Wall of Play Area (which is the Channel Separator)
            // But only if we are to the left of it (in play area)
            // If we are in channel, IsInLaunchChannel handles it.
            // If we are "above" the channel top, HandleArcCollision handles it (as it's open there).
            
            // So we only check collision with Channel Separator if X < ChannelWallX
            if (ball.Position.X < channelWallX)
            {
                if (ball.Position.X + ball.Radius > channelWallX)
                {
                    ball.Position = new Vector2(channelWallX - ball.Radius, ball.Position.Y);
                    ball.Velocity = new Vector2(-Math.Abs(ball.Velocity.X) * GameConstants.BounceRestitution, ball.Velocity.Y);
                }
            }
            // Note: Right Outer Wall for the channel is handled in HandleChannelCollision

            // Bottom Wall (Gutter)
            // Only apply in main play area, NOT in launcher channel
            // If ball is in channel (X >= channelWallX), let HandleChannelCollision manage bottom
            if (ball.Position.X < channelWallX && ball.Position.Y + ball.Radius > mainArea.Bottom)
            {
                ball.Position = new Vector2(ball.Position.X, mainArea.Bottom - ball.Radius);
                float xFriction = 0.8f;
                ball.Velocity = new Vector2(ball.Velocity.X * xFriction, -Math.Abs(ball.Velocity.Y) * 0.5f);
            }
        }

        public static void HandleBallCollision(Ball b1, Ball b2)
        {
            Vector2 direction = b1.Position - b2.Position;
            float distance = direction.Length();
            float minDistance = b1.Radius + b2.Radius;

            if (distance < minDistance && distance > 0)
            {
                direction.Normalize();
                
                // Separate
                float overlap = minDistance - distance;
                b1.Position += direction * overlap * 0.5f;
                b2.Position -= direction * overlap * 0.5f;

                // Wake up if sleeping
                b1.IsInHole = false;
                b2.IsInHole = false;
                
                // Clear hole occupation if they were settled
                // We need to check all holes to clear occupancy? 
                // Or let Update loop handle it.
                // Best is to let the Update loop clear it if they move out.

                // Exchange momentum (simplified elastic)
                // v1' = v1 - 2*m2/(m1+m2) * dot(v1-v2, dir) * dir
                // Assuming equal mass
                
                Vector2 v1 = b1.Velocity;
                Vector2 v2 = b2.Velocity;

                float p = Vector2.Dot(v1 - v2, direction); // (v1-v2).n

                b1.Velocity = v1 - p * direction;
                b2.Velocity = v2 + p * direction;
                
                // Add restitution
                b1.Velocity *= 0.9f;
                b2.Velocity *= 0.9f;
            }
        }

        public static Hole GetHoleContaining(Ball ball, System.Collections.Generic.List<Hole> holes)
        {
             return CheckHoleCollision(ball, holes);
        }

        public static void ApplyHoleTrap(Ball ball, Hole hole)
        {
             // Check if hole has an occupant
             if (hole.Occupant != null && hole.Occupant != ball)
             {
                 // Hole is occupied - don't let another ball settle here
                 // Fast balls should hit and potentially eject the occupant
                 return;
             }

             Vector2 toCenter = hole.Position - ball.Position;
             float dist = toCenter.Length();
             float speed = ball.Velocity.Length();
             
             // Fast balls can fly over
             if (speed > GameConstants.HoleEscapeSpeed && !ball.IsInHole)
             {
                 return;
             }
             
             // Check if ball is inside the hole
             if (dist < GameConstants.HoleRadius)
             {
                 // Ball is INSIDE the hole - lock it in!
                 ball.IsInHole = true;
                 
                 Vector2 pull = toCenter;
                 if (dist > 0) pull.Normalize();
                 
                 // Strong pull to center - gravity is disabled for balls in holes
                 ball.Velocity += pull * GameConstants.HoleInsidePullStrength * 0.016f;
                 ball.Velocity *= GameConstants.HoleInsideFriction;
                 
                 // Snap to center when slow
                 if (speed < GameConstants.HoleSnapSpeed || dist < GameConstants.HoleRadius * 0.3f)
                 {
                     ball.Velocity = Vector2.Zero;
                     ball.Position = hole.Position;
                     if (hole.Occupant == null) hole.Occupant = ball;
                 }
             }
             // Ball is approaching the hole
             else if (dist < GameConstants.HoleRadius * GameConstants.HoleAttractionRadius)
             {
                 Vector2 pull = toCenter;
                 if (dist > 0) pull.Normalize();
                 
                 // Normal attraction to pull ball in - weaker for faster balls
                 float speedFactor = System.Math.Max(0f, 1f - speed / GameConstants.HoleEscapeSpeed);
                 float pullStrength = GameConstants.HoleOutsidePullStrengthBase + 
                                     (GameConstants.HoleOutsidePullStrengthBonus * speedFactor);
                 
                 ball.Velocity += pull * pullStrength * 0.016f; 
                 ball.Velocity *= GameConstants.HoleOutsideFriction;
             }
             // Ball is far from hole
             else
             {
                 if (ball.IsInHole && dist > GameConstants.HoleRadius * 1.5f)
                 {
                     ball.IsInHole = false;
                     if (hole.Occupant == ball) hole.Occupant = null;
                 }
             }
        }

        public static void HandlePegCollision(Ball ball, Peg peg)
        {
            if (!peg.CollidesWith(ball)) return;

            Vector2 direction = ball.Position - peg.Position;
            float distance = direction.Length();

            if (distance == 0) return;

            direction.Normalize();

            float overlap = (ball.Radius + peg.Radius) - distance;
            ball.Position += direction * overlap;

            float dot = Vector2.Dot(ball.Velocity, direction);
            // Reflect only if moving towards peg
            if (dot < 0)
            {
                ball.Velocity -= 2 * dot * direction;
                ball.Velocity *= GameConstants.BounceRestitution;
            }
        }

        public static Hole CheckHoleCollision(Ball ball, System.Collections.Generic.List<Hole> holes)
        {
            foreach (var hole in holes)
            {
                if (hole.Contains(ball))
                    return hole;
            }
            return null;
        }

        public static bool IsBallOutOfBounds(Ball ball, Rectangle bounds)
        {
            return ball.Position.Y > bounds.Bottom + 50;
        }

        public static bool IsBallStopped(Ball ball)
        {
            // Ball must be slow AND have been slow for a while
            // This prevents false positives during bounces
            if (ball.IsInHole && ball.Velocity.Length() < 100f && ball.TimeAtLowSpeed > 0.3f) 
                return true;
            
            return ball.Velocity.Length() < 20f && ball.TimeAtLowSpeed > GameConstants.BallStoppedTimeThreshold;
        }

        public static bool IsBallInLauncher(Ball ball, Board board)
        {
            // Check if ball is strictly in the channel column
            bool inChannelX = ball.Position.X >= board.ChannelWallX;
            
            // Check if below the top of the channel wall (where it enters the board)
            bool inChannelY = ball.Position.Y > board.ChannelWallTopY;

            return inChannelX && inChannelY;
        }
    }
}
