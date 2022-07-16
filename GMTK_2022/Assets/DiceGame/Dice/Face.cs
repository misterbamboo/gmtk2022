using System.Collections.Generic;

namespace DiceGame
{
    // cannonical face sides
    // right handed dice https://en.wikipedia.org/wiki/Dice#Arrangement
    public enum FaceSides
    {
        Front = 1,
        Top = 2,
        Right = 3,
        Left = 4,
        Bottom = 5,
        Back = 6,
    }

    public struct Face
    {
        public DiceColors Color { get; }
        public int Value { get; }
        public FaceSides Side { get; }
        public string SpriteName => Color.ToString() + "_" + Value;

        public Face(DiceColors color, FaceSides side, int value)
        {
            Color = color;
            Value = value;
            Side = side;
        }

        public static IEnumerable<Face> StandardAll(DiceColors color)
        {
            return new List<Face>()
            {
                new Face(color, FaceSides.Front, 1),
                new Face(color, FaceSides.Top, 2),
                new Face(color, FaceSides.Right, 3),
                new Face(color, FaceSides.Left, 4),
                new Face(color, FaceSides.Bottom, 5),
                new Face(color, FaceSides.Back, 6),
            };
        }
    }
}
