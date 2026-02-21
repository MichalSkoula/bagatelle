using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bagatelle.Shared.GameObjects.Boards
{
    public class Velocity : Board
    {
        public override string Name => "VELOCITY";
        public override int Index => 2;

        public override Color BackgroundColor => new Color(30, 60, 40);
        public override Color WallColor => new Color(100, 120, 100);
        public override Texture2D CurrentHoleSprite => Game1.HoleSpriteGreen;

        public Velocity() : base()
        {
            CreateHoles();
            CreatePegs();
        }

        private void CreateHoles()
        {
            float centerX = (Margin + ChannelWallX) / 2f;
            float startY = ArcCenter.Y - 210;
            float rowSpacing = 140;

            // Row 1: 100 points
            Holes.Add(new Hole(new Vector2(centerX, startY), 100, CurrentHoleSprite));

            // Row 2: 75 points
            Holes.Add(new Hole(new Vector2(centerX - 220, startY + rowSpacing), 75, CurrentHoleSprite));
            Holes.Add(new Hole(new Vector2(centerX + 220, startY + rowSpacing), 75, CurrentHoleSprite));

            // Row 3: 75 points
            float row3Y = startY + rowSpacing * 2;
            Holes.Add(new Hole(new Vector2(centerX - 120, row3Y), 75, CurrentHoleSprite));
            Holes.Add(new Hole(new Vector2(centerX + 120, row3Y), 75, CurrentHoleSprite));

            // Row 4: 125 points
            float row4Y = startY + rowSpacing * 3;
            Holes.Add(new Hole(new Vector2(centerX, row4Y), 125, CurrentHoleSprite));

            // Row 5: 50 points
            float row5Y = startY + rowSpacing * 4;
            Holes.Add(new Hole(new Vector2(centerX - 260, row5Y), 50, CurrentHoleSprite));
            Holes.Add(new Hole(new Vector2(centerX + 260, row5Y), 50, CurrentHoleSprite));

            // Row 6: 75 points
            float row6Y = startY + rowSpacing * 5 - 40;
            Holes.Add(new Hole(new Vector2(centerX - 80, row6Y), 75, CurrentHoleSprite));
            Holes.Add(new Hole(new Vector2(centerX + 80, row6Y), 75, CurrentHoleSprite));

            // Row 7: 25 points
            float row7Y = startY + rowSpacing * 6;
            Holes.Add(new Hole(new Vector2(centerX - 200, row7Y), 25, CurrentHoleSprite));
            Holes.Add(new Hole(new Vector2(centerX + 200, row7Y), 25, CurrentHoleSprite));

            // Row 8: 10 points
            Holes.Add(new Hole(new Vector2(centerX, row7Y + 55), 10, CurrentHoleSprite));
        }

        private void CreatePegs()
        {
            float centerX = (Margin + ChannelWallX) / 2f;
            float holeStartY = ArcCenter.Y - 210;
            float rowSpacing = 140;

            float row1Y = holeStartY;
            float row2Y = holeStartY + rowSpacing;
            float row3Y = holeStartY + rowSpacing * 2;
            float row4Y = holeStartY + rowSpacing * 3;
            float row5Y = holeStartY + rowSpacing * 4;
            float row6Y = holeStartY + rowSpacing * 5 - 40;
            float row7Y = holeStartY + rowSpacing * 6;
            float row8Y = row7Y + 55;

            float pegRow1Y = (row1Y + row2Y) / 2f;
            float pegRow2Y = (row2Y + row3Y) / 2f;
            float pegRow3Y = (row3Y + row4Y) / 2f;
            float pegRow4Y = (row4Y + row5Y) / 2f;
            float pegRow5Y = (row5Y + row6Y) / 2f;
            float pegRow6Y = (row6Y + row7Y) / 2f;
            float pegRow7Y = (row7Y + row8Y) / 2f + 20;

            // Row 1
            Pegs.Add(new Peg(new Vector2(centerX - 140, pegRow1Y)));
            Pegs.Add(new Peg(new Vector2(centerX + 140, pegRow1Y)));

            // Row 2
            Pegs.Add(new Peg(new Vector2(centerX, pegRow2Y)));

            // Row 3
            Pegs.Add(new Peg(new Vector2(centerX - 220, pegRow3Y)));
            Pegs.Add(new Peg(new Vector2(centerX + 220, pegRow3Y)));

            // Row 4
            Pegs.Add(new Peg(new Vector2(centerX - 80, pegRow4Y)));
            Pegs.Add(new Peg(new Vector2(centerX + 80, pegRow4Y)));

            // Row 5
            Pegs.Add(new Peg(new Vector2(centerX - 260, pegRow5Y)));
            Pegs.Add(new Peg(new Vector2(centerX + 260, pegRow5Y)));

            // Row 6
            Pegs.Add(new Peg(new Vector2(centerX, pegRow6Y)));

            // Row 7
            Pegs.Add(new Peg(new Vector2(centerX - 200, pegRow7Y)));
            Pegs.Add(new Peg(new Vector2(centerX + 200, pegRow7Y)));
        }
    }
}
