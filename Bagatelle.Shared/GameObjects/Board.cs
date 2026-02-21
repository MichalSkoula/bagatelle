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

        private const int Margin = 40;
        private const int TopMargin = 160; // More space for UI - scores and menu button
        private const int ChannelWidth = 80;

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
                screenH - (int)ArcCenter.Y - 120
            );

            // 2. Define the Launch Channel
            // It's on the right side.
            ChannelWallX = boardRight - ChannelWidth;

            // The channel wall shouldn't go all the way to the top of the arc.
            // It should stop to let the ball curve around.
            ChannelWallTopY = ArcCenter.Y - 80;

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

            Launcher = new Launcher(new Vector2(LaunchChannel.Center.X, MainArea.Bottom + 20));
        }

        private List<Hole> CreateHoles()
        {
            var holes = new List<Hole>();
            float centerX = (Margin + ChannelWallX) / 2f;
            float startY = ArcCenter.Y - 210;
            float rowSpacing = 150;

            // Row 1: 75 points
            holes.Add(new Hole(new Vector2(centerX, startY), 75));

            // Row 2: 50 points
            holes.Add(new Hole(new Vector2(centerX - 210, startY + rowSpacing), 50));
            holes.Add(new Hole(new Vector2(centerX + 210, startY + rowSpacing), 50));

            // Row 3: 100 points
            float row3Y = startY + rowSpacing * 2;
            holes.Add(new Hole(new Vector2(centerX, row3Y), 100));

            // Row 4: 50 points
            float row4Y = startY + rowSpacing * 3;
            holes.Add(new Hole(new Vector2(centerX - 280, row4Y), 50));
            holes.Add(new Hole(new Vector2(centerX + 280, row4Y), 50));

            // Row 5: 50 points
            float row5Y = startY + rowSpacing * 4;
            holes.Add(new Hole(new Vector2(centerX, row5Y), 50));

            // Row 6: 25 points
            float row6Y = startY + rowSpacing * 5 - 50;
            holes.Add(new Hole(new Vector2(centerX - 180, row6Y), 25));
            holes.Add(new Hole(new Vector2(centerX + 180, row6Y), 25));

            // Row 7: 25 points
            float row7Y = startY + rowSpacing * 6;
            holes.Add(new Hole(new Vector2(centerX - 220, row7Y), 25));
            holes.Add(new Hole(new Vector2(centerX + 220, row7Y), 25));

            // Row 8: 10 points
            holes.Add(new Hole(new Vector2(centerX, row7Y + 50), 10));

            return holes;
        }

        private List<Peg> CreatePegs()
        {
            var pegs = new List<Peg>();
            float centerX = (Margin + ChannelWallX) / 2f;
            float startY = ArcCenter.Y - 260;

            // Add Peg at the top of the Channel Separator to smooth the corner
            pegs.Add(new Peg(new Vector2(ChannelWallX + 5, ChannelWallTopY)));

            // Row 1
            pegs.Add(new Peg(new Vector2(centerX - 100, startY)));
            pegs.Add(new Peg(new Vector2(centerX + 100, startY)));

            // Row 2
            pegs.Add(new Peg(new Vector2(centerX - 70, startY + 140)));
            pegs.Add(new Peg(new Vector2(centerX + 70, startY + 140)));

            // Row 3
            pegs.Add(new Peg(new Vector2(centerX - 210, startY + 250)));
            pegs.Add(new Peg(new Vector2(centerX + 210, startY + 250)));

            // Row 4
            pegs.Add(new Peg(new Vector2(centerX - 130, startY + 390)));
            pegs.Add(new Peg(new Vector2(centerX - 40, startY + 430)));
            pegs.Add(new Peg(new Vector2(centerX + 40, startY + 430)));
            pegs.Add(new Peg(new Vector2(centerX + 130, startY + 390)));

            // Row 5
            pegs.Add(new Peg(new Vector2(centerX - 280, startY + 550)));
            pegs.Add(new Peg(new Vector2(centerX + 280, startY + 550)));

            // Row 6
            pegs.Add(new Peg(new Vector2(centerX, startY + 700)));

            // Row 7
            pegs.Add(new Peg(new Vector2(centerX - 180, startY + 800)));
            pegs.Add(new Peg(new Vector2(centerX + 180, startY + 800)));

            // Row 8
            pegs.Add(new Peg(new Vector2(centerX - 220, startY + 1000)));
            pegs.Add(new Peg(new Vector2(centerX + 220, startY + 1000)));

            // Row 9
            pegs.Add(new Peg(new Vector2(centerX - 280, startY + 1050)));
            pegs.Add(new Peg(new Vector2(centerX - 160, startY + 1050)));
            pegs.Add(new Peg(new Vector2(centerX, startY + 1050)));
            pegs.Add(new Peg(new Vector2(centerX + 160, startY + 1050)));
            pegs.Add(new Peg(new Vector2(centerX + 280, startY + 1050)));

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
            int thickness = 10;

            // Outer Left Wall
            DrawHelper.DrawRectangle(spriteBatch, new Rectangle(MainArea.Left, MainArea.Top, thickness, MainArea.Height), wallColor);

            // Outer Right Wall
            DrawHelper.DrawRectangle(spriteBatch, new Rectangle(MainArea.Right - thickness, MainArea.Top, thickness, MainArea.Height), wallColor);

            // Bottom Wall
            DrawHelper.DrawRectangle(spriteBatch, new Rectangle(MainArea.Left, MainArea.Bottom, MainArea.Width, thickness), wallColor);

            // Top Arc Wall
            // Draw upper semi-circle from PI (Left) to 2PI (Right)
            DrawArcWall(spriteBatch, ArcCenter, ArcRadius - 4, thickness, wallColor);

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
            int segments = 128;
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
