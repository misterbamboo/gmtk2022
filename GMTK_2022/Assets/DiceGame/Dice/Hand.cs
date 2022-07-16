using System.Collections.Generic;
using System.Linq;

namespace DiceGame
{
    public class Hand
    {
        private List<Dice> allDice = new List<Dice>();
        private List<Dice> availableDice = new List<Dice>();
        private List<Dice> discardedDice = new List<Dice>();

        public Hand()
        {

        }

        public IEnumerable<Dice> AvailableDice => availableDice;
        public IEnumerable<Dice> DiscardedDice => discardedDice;

        public void ResetDice(IEnumerable<Dice> dice)
        {
            Empty();
            allDice = dice.ToList();
            availableDice = dice.ToList();
        }

        public Dice SelectDice(int index)
        {
            var selectedDice = availableDice[index];
            availableDice.RemoveAt(index);

            discardedDice.AddRange(DiceToDiscard(index));
            availableDice = DiceToKeep(index).ToList();

            return selectedDice;
        }

        public IEnumerable<Dice> DiceToDiscard(int index) => availableDice.Where(d => d.Value < availableDice[index].Value);
        public IEnumerable<Dice> DiceToKeep(int index) => availableDice.Where(d => d.Value >= availableDice[index].Value);

        public void Empty()
        {
            allDice.Clear();
            availableDice.Clear();
            discardedDice.Clear();
        }
    }
}

