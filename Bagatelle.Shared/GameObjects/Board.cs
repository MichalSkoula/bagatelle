using Bagatelle.Shared.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Bagatelle.Shared.GameObjects
{
    public class Board
    {
        // Board Geometry
        public Rectangle MainArea { get; }      // Rectangular part of the board
        public Rectangle LaunchChannel { get; } // The channel area (logic only)
        public Vector2 ArcCenter { get; }       // Center of the top semi-circle
        public float ArcRadius { get; }         // Radius of the top semi-circle
        public int ChannelWallX { get; }        // X coordinate of the wall separating channel and play area
        public float ChannelWallTopY { get; }   // Y coordinate where the channel wall ends

        public List<Hole> Holes { get; }
        public List<Peg> Pegs { get; }
        public Launcher Launcher { get; }

        private const int Margin = 20;
        private const int TopMargin = 80; // More space for UI - scores and menu button
        private const int ChannelWidth = 40;

        public Board()
        {
            int screenW = GameConstants.ScreenWidth;
            int screenH = GameConstants.ScreenHeight;

            // 1. Define the main outer shape
            // The board has a semi-circular top and a rectangular body.
            // Width of the board is ScreenWidth - 2*Margin.
            int boardWidth = screenW - 2 * Margin;
            int boardLeft = Margin;
            int boardRight = screenW - Margin;

            // Radius is half the width
            ArcRadius = boardWidth / 2f;

            // The semi-circle sits at the top. 
            // Center X is middle of board.
            // Center Y is TopMargin + ArcRadius.
            ArcCenter = new Vector2(screenW / 2f, TopMargin + ArcRadius);

            // The rectangular part starts at ArcCenter.Y and goes down.
            MainArea = new Rectangle(
                boardLeft,
                (int)ArcCenter.Y,
                boardWidth,
                screenH - (int)ArcCenter.Y - 60
            );

            // 2. Define the Launch Channel
            // It's on the right side.
            ChannelWallX = boardRight - ChannelWidth;

            // The channel wall shouldn't go all the way to the top of the arc.
            // It should stop to let the ball curve around.
            ChannelWallTopY = ArcCenter.Y - 40;

            // Logical rect for channel (used for input/logic mostly)
            LaunchChannel = new Rectangle(
                ChannelWallX,
                (int)ChannelWallTopY,
                ChannelWidth,
                MainArea.Bottom - (int)ChannelWallTopY
            );

            // 3. Create objects
            Holes = CreateHoles();
            Pegs = CreatePegs();

            Launcher = new Launcher(new Vector2(LaunchChannel.Center.X, MainArea.Bottom + 10));
        }

        private List<Hole> CreateHoles()
        {
            var holes = new List<Hole>();
            float centerX = (Margin + ChannelWallX) / 2f;
            float startY = ArcCenter.Y - 60;
            float rowSpacing = 110;

            // Row 1: 100 points
            holes.Add(new Hole(new Vector2(centerX, startY), 100));

            // Row 2: 75 points
            holes.Add(new Hole(new Vector2(centerX - 105, startY + rowSpacing), 75));
            holes.Add(new Hole(new Vector2(centerX + 105, startY + rowSpacing), 75));

            // Row 3: 50 points
            float row3Y = startY + rowSpacing * 2;
            holes.Add(new Hole(new Vector2(centerX - 140, row3Y), 50));
            holes.Add(new Hole(new Vector2(centerX, row3Y), 50));
            holes.Add(new Hole(new Vector2(centerX + 140, row3Y), 50));

            // Row 4: 25 points
            float row4Y = startY + rowSpacing * 3;
            holes.Add(new Hole(new Vector2(centerX - 105, row4Y), 25));
            holes.Add(new Hole(new Vector2(centerX + 105, row4Y), 25));

            // Row 5: 25-10-25
            float row5Y = startY + rowSpacing * 4;
            holes.Add(new Hole(new Vector2(centerX - 140, row5Y), 25));
            holes.Add(new Hole(new Vector2(centerX, row5Y), 10));
            holes.Add(new Hole(new Vector2(centerX + 140, row5Y), 25));

            return holes;
        }

        private List<Peg> CreatePegs()
        {
            var pegs = new List<Peg>();
            float centerX = (Margin + ChannelWallX) / 2f;

            // Add Peg at the top of the Channel Separator to smooth the corner
            pegs.Add(new Peg(new Vector2(ChannelWallX, ChannelWallTopY)));

            // Upper arc area pegs - spread wide in the semicircle
            float upperY = ArcCenter.Y - 60;
            pegs.Add(new Peg(new Vector2(centerX - 120, upperY)));
            pegs.Add(new Peg(new Vector2(centerX - 40, upperY)));
            pegs.Add(new Peg(new Vector2(centerX + 40, upperY)));
            pegs.Add(new Peg(new Vector2(centerX + 120, upperY)));

            // Second row in arc
            float upperY2 = ArcCenter.Y - 10;
            pegs.Add(new Peg(new Vector2(centerX - 80, upperY2)));
            pegs.Add(new Peg(new Vector2(centerX, upperY2)));
            pegs.Add(new Peg(new Vector2(centerX + 80, upperY2)));

            // Main peg grid (lower area)
            float startY = ArcCenter.Y + 50;
            for (int row = 0; row < 5; row++)
            {
                int pegCount = (row % 2 == 0) ? 4 : 3;
                float spacing = 80;
                float offsetX = (row % 2 == 0) ? -120f : -80f;

                for (int i = 0; i < pegCount; i++)
                {
                    float x = centerX + offsetX + i * spacing;
                    float y = startY + row * 75;
                    pegs.Add(new Peg(new Vector2(x, y)));
                }
            }
            return pegs;
        }

        public Vector2 GetBallStartPosition()
        {
            // Ball should sit on the bottom of the channel
            return new Vector2(LaunchChannel.Center.X, MainArea.Bottom - GameConstants.BallRadius);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // 1. Draw Board Background
            DrawHelper.DrawRectangle(spriteBatch, MainArea, GameConstants.BoardColor);
            DrawHelper.DrawCircle(spriteBatch, ArcCenter, ArcRadius, GameConstants.BoardColor);

            // 2. Draw Borders/Walls
            Color wallColor = Color.SaddleBrown;
            int thickness = 5;

            // Outer Left Wall
            DrawHelper.DrawRectangle(spriteBatch, new Rectangle(MainArea.Left, MainArea.Top, thickness, MainArea.Height), wallColor);

            // Outer Right Wall
            DrawHelper.DrawRectangle(spriteBatch, new Rectangle(MainArea.Right - thickness, MainArea.Top, thickness, MainArea.Height), wallColor);

            // Bottom Wall
            DrawHelper.DrawRectangle(spriteBatch, new Rectangle(MainArea.Left, MainArea.Bottom, MainArea.Width, thickness), wallColor);

            // Top Arc Wall
            // Draw upper semi-circle from PI (Left) to 2PI (Right)
            DrawArcWall(spriteBatch, ArcCenter, ArcRadius, thickness, wallColor);

            // Channel Separator Wall
            Rectangle channelWall = new Rectangle(
                ChannelWallX,
                (int)ChannelWallTopY,
                thickness,
                MainArea.Bottom - (int)ChannelWallTopY
            );
            DrawHelper.DrawRectangle(spriteBatch, channelWall, wallColor);

            // 3. Draw Game Objects
            foreach (var hole in Holes) hole.Draw(spriteBatch);
            foreach (var peg in Pegs) peg.Draw(spriteBatch);
            Launcher.Draw(spriteBatch);
        }

        private void DrawArcWall(SpriteBatch sb, Vector2 center, float radius, int thickness, Color color)
        {
            int segments = 64;
            // Arc from PI (Left) to 2PI (Right), going Up (negative Y relative to center)
            float startAngle = MathHelper.Pi;
            float totalAngle = MathHelper.Pi; // Semicircle
            float angleStep = totalAngle / segments;

            for (int i = 0; i < segments; i++)
            {
                float a1 = startAngle + (i * angleStep);
                float a2 = startAngle + ((i + 1) * angleStep);

                Vector2 p1 = center + new Vector2((float)Math.Cos(a1), (float)Math.Sin(a1)) * radius;
                Vector2 p2 = center + new Vector2((float)Math.Cos(a2), (float)Math.Sin(a2)) * radius;

                DrawHelper.DrawLine(sb, p1, p2, thickness, color);
            }
        }
    }
}
