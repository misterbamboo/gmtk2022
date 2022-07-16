using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DiceGame
{
    public class Dice
    {
        private List<Face> faces = new List<Face>();
        private FaceSides currentSide = FaceSides.Front;

        public Dice(DiceColors color = DiceColors.White)
        {
            faces = Face.StandardAll(color).ToList();
            FaceTowards(FaceSides.Front);
        }

        public Dice(IEnumerable<Face> faces)
        {
            this.faces = faces.ToList();
            FaceTowards(FaceSides.Front);
        }

        public static Dice WithValue(int value)
        {
            var dice = new Dice();
            dice.FaceTowards((FaceSides)value);

            return dice;
        }

        public int Value => CurrentFace.Value;
        public DiceColors Color => CurrentFace.Color;
        public Face CurrentFace => faces[(int)currentSide - 1];

        public void FaceTowards(FaceSides side)
        {
            currentSide = side;
        }

        public void Roll()
        {
            var faceSide = (FaceSides)Random.Range(1, 7);
            FaceTowards(faceSide);
        }
    }
}

