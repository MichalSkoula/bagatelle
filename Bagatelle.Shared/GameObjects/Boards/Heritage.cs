using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bagatelle.Shared.GameObjects.Boards
{
    public class Heritage : Board
    {
        public override string Name => "HERITAGE";
        public override int Index => 1;
        public override Texture2D CurrentHoleSprite => Game1.HoleSprite;

        public Heritage() : base()
        {
            CreateHoles();
            CreatePegs();
        }

        private void CreateHoles()
        {
            float centerX = (Margin + ChannelWallX) / 2f;
            float startY = ArcCenter.Y - 210;
            float rowSpacing = 150;

            // Row 1: 75 points
            Holes.Add(new Hole(new Vector2(centerX, startY), 75, CurrentHoleSprite));

            // Row 2: 50 points
            Holes.Add(new Hole(new Vector2(centerX - 210, startY + rowSpacing), 50, CurrentHoleSprite));
            Holes.Add(new Hole(new Vector2(centerX + 210, startY + rowSpacing), 50, CurrentHoleSprite));

            // Row 3: 100 points
            float row3Y = startY + rowSpacing * 2;
            Holes.Add(new Hole(new Vector2(centerX, row3Y), 100, CurrentHoleSprite));

            // Row 4: 50 points
            float row4Y = startY + rowSpacing * 3;
            Holes.Add(new Hole(new Vector2(centerX - 280, row4Y), 50, CurrentHoleSprite));
            Holes.Add(new Hole(new Vector2(centerX + 280, row4Y), 50, CurrentHoleSprite));

            // Row 5: 50 points
            float row5Y = startY + rowSpacing * 4;
            Holes.Add(new Hole(new Vector2(centerX, row5Y), 50, CurrentHoleSprite));

            // Row 6: 25 points
            float row6Y = startY + rowSpacing * 5 - 50;
            Holes.Add(new Hole(new Vector2(centerX - 180, row6Y), 25, CurrentHoleSprite));
            Holes.Add(new Hole(new Vector2(centerX + 180, row6Y), 25, CurrentHoleSprite));

            // Row 7: 25 points
            float row7Y = startY + rowSpacing * 6;
            Holes.Add(new Hole(new Vector2(centerX - 220, row7Y), 25, CurrentHoleSprite));
            Holes.Add(new Hole(new Vector2(centerX + 220, row7Y), 25, CurrentHoleSprite));

            // Row 8: 10 points
            Holes.Add(new Hole(new Vector2(centerX, row7Y + 50), 10, CurrentHoleSprite));
        }

        private void CreatePegs()
        {
            float centerX = (Margin + ChannelWallX) / 2f;
            float startY = ArcCenter.Y - 260;

            // Row 1
            Pegs.Add(new Peg(new Vector2(centerX - 100, startY)));
            Pegs.Add(new Peg(new Vector2(centerX + 100, startY)));

            // Row 2
            Pegs.Add(new Peg(new Vector2(centerX - 70, startY + 140)));
            Pegs.Add(new Peg(new Vector2(centerX + 70, startY + 140)));

            // Row 3
            Pegs.Add(new Peg(new Vector2(centerX - 210, startY + 250)));
            Pegs.Add(new Peg(new Vector2(centerX + 210, startY + 250)));

            // Row 4
            Pegs.Add(new Peg(new Vector2(centerX - 130, startY + 390)));
            Pegs.Add(new Peg(new Vector2(centerX - 40, startY + 430)));
            Pegs.Add(new Peg(new Vector2(centerX + 40, startY + 430)));
            Pegs.Add(new Peg(new Vector2(centerX + 130, startY + 390)));

            // Row 5
            Pegs.Add(new Peg(new Vector2(centerX - 280, startY + 550)));
            Pegs.Add(new Peg(new Vector2(centerX + 280, startY + 550)));

            // Row 6
            Pegs.Add(new Peg(new Vector2(centerX, startY + 700)));

            // Row 7
            Pegs.Add(new Peg(new Vector2(centerX - 180, startY + 800)));
            Pegs.Add(new Peg(new Vector2(centerX + 180, startY + 800)));

            // Row 8
            Pegs.Add(new Peg(new Vector2(centerX - 220, startY + 1000)));
            Pegs.Add(new Peg(new Vector2(centerX + 220, startY + 1000)));

            // Row 9
            Pegs.Add(new Peg(new Vector2(centerX - 280, startY + 1050)));
            Pegs.Add(new Peg(new Vector2(centerX - 160, startY + 1050)));
            Pegs.Add(new Peg(new Vector2(centerX, startY + 1050)));
            Pegs.Add(new Peg(new Vector2(centerX + 160, startY + 1050)));
            Pegs.Add(new Peg(new Vector2(centerX + 280, startY + 1050)));
        }
    }
}
