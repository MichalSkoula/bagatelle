using Bagatelle.Shared.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Bagatelle.Shared.GameObjects.Boards
{
    public abstract class Board
    {
        public abstract string Name { get; }
        public abstract int Index { get; }

        public virtual Color BackgroundColor => GameConstants.BoardColor;
        public virtual Color WallColor => Color.SaddleBrown;

        public virtual Texture2D CurrentHoleSprite => Game1.HoleSprite;

        // Board Geometry
        public Rectangle MainArea { get; }      // Rectangular part of the board
        public Rectangle LaunchChannel { get; } // The channel area (logic only)
        public Vector2 ArcCenter { get; }       // Center of the top semi-circle
        public float ArcRadius { get; }         // Radius of the top semi-circle
        public int ChannelWallX { get; }        // X coordinate of the wall separating channel and play area
        public float ChannelWallTopY { get; }   // Y coordinate where the channel wall ends

        public List<Hole> Holes { get; } = new List<Hole>();
        public List<Peg> Pegs { get; } = new List<Peg>();
        public Launcher Launcher { get; }

        protected const int Margin = 40;
        protected const int TopMargin = 160; // More space for UI - scores and menu button
        protected const int ChannelWidth = 62;

        protected Board()
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
            ChannelWallTopY = ArcCenter.Y;

            // Logical rect for channel (used for input/logic mostly)
            LaunchChannel = new Rectangle(
                ChannelWallX,
                (int)ChannelWallTopY,
                ChannelWidth,
                MainArea.Bottom - (int)ChannelWallTopY
            );

            Launcher = new Launcher(new Vector2(LaunchChannel.Center.X, MainArea.Bottom + 20));
        }

        public Vector2 GetBallStartPosition()
        {
            // Ball should sit on the bottom of the channel
            return new Vector2(LaunchChannel.Center.X, MainArea.Bottom - GameConstants.BallRadius);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 shadowOffset = new Vector2(5.3f, 5.3f);
            Color shadowColor = Color.Black * 0.4f;

            // 0. Draw Board Drop Shadow
            Rectangle boardShadowRect = new Rectangle(
                MainArea.X + (int)shadowOffset.X,
                MainArea.Y + (int)shadowOffset.Y,
                MainArea.Width,
                MainArea.Height);
            DrawHelper.DrawRectangle(spriteBatch, boardShadowRect, shadowColor);
            DrawHelper.DrawCircle(spriteBatch, ArcCenter + shadowOffset, ArcRadius, shadowColor);

            // 1. Draw Board Background
            DrawHelper.DrawRectangle(spriteBatch, MainArea, BackgroundColor);
            DrawHelper.DrawCircle(spriteBatch, ArcCenter, ArcRadius, BackgroundColor);

            // 1.5 Draw Wall Shadows (Light from NW)
            int thickness = 10;

            // Outer Left Wall Shadow
            DrawHelper.DrawRectangle(spriteBatch, new Rectangle(MainArea.Left + (int)shadowOffset.X, MainArea.Top + (int)shadowOffset.Y, thickness, MainArea.Height), shadowColor);

            // Top Arc Wall Shadow
            DrawArcWall(spriteBatch, ArcCenter + shadowOffset, ArcRadius - 5, thickness, shadowColor);

            // Channel Separator Wall Shadow
            Rectangle channelWallShadow = new Rectangle(
                ChannelWallX + (int)shadowOffset.X,
                (int)ChannelWallTopY + (int)shadowOffset.Y,
                thickness,
                MainArea.Bottom - (int)ChannelWallTopY
            );
            DrawHelper.DrawRectangle(spriteBatch, channelWallShadow, shadowColor);

            // Bottom Wall Shadow
            DrawHelper.DrawRectangle(spriteBatch, new Rectangle(MainArea.Left + (int)shadowOffset.X, MainArea.Bottom + (int)shadowOffset.Y, MainArea.Width, thickness), shadowColor);

            // 2. Draw Borders/Walls
            Color wallColor = WallColor;

            // Outer Left Wall
            DrawHelper.DrawRectangle(spriteBatch, new Rectangle(MainArea.Left, MainArea.Top, thickness, MainArea.Height), wallColor);

            // Outer Right Wall
            DrawHelper.DrawRectangle(spriteBatch, new Rectangle(MainArea.Right - thickness, MainArea.Top, thickness, MainArea.Height), wallColor);

            // Bottom Wall
            DrawHelper.DrawRectangle(spriteBatch, new Rectangle(MainArea.Left, MainArea.Bottom, MainArea.Width, thickness), wallColor);

            // Top Arc Wall
            // Draw upper semi-circle from PI (Left) to 2PI (Right)
            DrawArcWall(spriteBatch, ArcCenter, ArcRadius - 5, thickness, wallColor);

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
