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
            // Fix: Stop ball at the start position height to prevent "drop and teleport" glitch.
            // Start position is at Bottom - 50 (center). 
            // So we want the ball center to not go below that.
            float launcherFloorY = channel.Bottom - 50 + ball.Radius;
            
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
            // Allow ball to bounce off bottom
            if (ball.Position.Y + ball.Radius > mainArea.Bottom)
            {
                ball.Position = new Vector2(ball.Position.X, mainArea.Bottom - ball.Radius);
                // Lower restitution for floor to reduce bouncing
                // Increase Y restitution to 0.5f (was 0.2f) so it bounces a bit more
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
             if (hole.Occupant != null && hole.Occupant != ball && Physics.IsBallStopped(hole.Occupant))
             {
                 return;
             }

             Vector2 toCenter = hole.Position - ball.Position;
             float dist = toCenter.Length();
             float speed = ball.Velocity.Length();
             
             // Very fast balls fly over - no effect
             if (speed > 800f)
             {
                 return;
             }
             
             // Only affect balls that are close to the hole
             if (dist < hole.Radius * 1.2f)
             {
                 Vector2 pull = toCenter;
                 if (dist > 0) pull.Normalize();
                 
                 // Pull strength - stronger for slower balls
                 float pullStrength = 2000f * (1f - speed / 800f);
                 ball.Velocity += pull * pullStrength * 0.016f; 
                 
                 // Friction when near hole
                 ball.Velocity *= 0.90f; 

                 // Snap to center when very slow and close
                 if (dist < hole.Radius && speed < 80f)
                 {
                      ball.Velocity = Vector2.Zero;
                      ball.Position = hole.Position;
                      ball.IsInHole = true;
                      if (hole.Occupant == null) hole.Occupant = ball;
                 }
                 else if (dist < hole.Radius)
                 {
                     ball.IsInHole = true;
                 }
             }
             else
             {
                 if (ball.IsInHole && dist > hole.Radius)
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
            // Increased threshold to prevent endless turns
            // If in hole, we are very generous to allow turn to end
            if (ball.IsInHole && ball.Velocity.Length() < 100f) return true;
            return ball.Velocity.Length() < 20f;
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
