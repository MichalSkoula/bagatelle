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
            float startY = ArcCenter.Y - 240;
            float rowSpacing = 135;

            // Row 1: 125 points
            Holes.Add(new Hole(new Vector2(centerX, startY), 125, CurrentHoleSprite));

            // Row 2: 75 points
            Holes.Add(new Hole(new Vector2(centerX - 210, startY + rowSpacing), 75, CurrentHoleSprite));
            Holes.Add(new Hole(new Vector2(centerX + 210, startY + rowSpacing), 75, CurrentHoleSprite));

            // Row 3: 100 points
            float row3Y = startY + rowSpacing * 2;
            Holes.Add(new Hole(new Vector2(centerX, row3Y), 100, CurrentHoleSprite));

            // Row 4: 50 points
            float row4Y = startY + rowSpacing * 3;
            Holes.Add(new Hole(new Vector2(centerX - 250, row4Y), 50, CurrentHoleSprite));
            Holes.Add(new Hole(new Vector2(centerX + 250, row4Y), 50, CurrentHoleSprite));

            // Row 5: 75 points
            float row5Y = startY + rowSpacing * 4;
            Holes.Add(new Hole(new Vector2(centerX - 120, row5Y), 75, CurrentHoleSprite));
            Holes.Add(new Hole(new Vector2(centerX + 120, row5Y), 75, CurrentHoleSprite));

            // Row 6: 50 points
            float row6Y = startY + rowSpacing * 5 - 30;
            Holes.Add(new Hole(new Vector2(centerX, row6Y), 50, CurrentHoleSprite));

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
            float startY = ArcCenter.Y - 285;

            // Row 1
            Pegs.Add(new Peg(new Vector2(centerX - 140, startY)));
            Pegs.Add(new Peg(new Vector2(centerX + 140, startY)));

            // Row 2
            Pegs.Add(new Peg(new Vector2(centerX, startY + 110)));

            // Row 3
            Pegs.Add(new Peg(new Vector2(centerX - 200, startY + 210)));
            Pegs.Add(new Peg(new Vector2(centerX + 200, startY + 210)));

            // Row 4
            Pegs.Add(new Peg(new Vector2(centerX - 90, startY + 310)));
            Pegs.Add(new Peg(new Vector2(centerX + 90, startY + 310)));

            // Row 5
            Pegs.Add(new Peg(new Vector2(centerX - 260, startY + 420)));
            Pegs.Add(new Peg(new Vector2(centerX + 260, startY + 420)));

            // Row 6
            Pegs.Add(new Peg(new Vector2(centerX, startY + 530)));

            // Row 7
            Pegs.Add(new Peg(new Vector2(centerX - 170, startY + 650)));
            Pegs.Add(new Peg(new Vector2(centerX + 170, startY + 650)));

            // Row 8
            Pegs.Add(new Peg(new Vector2(centerX - 240, startY + 780)));
            Pegs.Add(new Peg(new Vector2(centerX + 240, startY + 780)));

            // Row 9
            Pegs.Add(new Peg(new Vector2(centerX - 80, startY + 910)));
            Pegs.Add(new Peg(new Vector2(centerX + 80, startY + 910)));
        }
    }
}
