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

            discardedDice.AddRange(DiceToDiscard(selectedDice.Value));
            availableDice = DiceToKeep(selectedDice.Value).ToList();

            return selectedDice;
        }

        public IEnumerable<Dice> DiceToDiscard(int value) => availableDice.Where(d => d.Value < value);
        public IEnumerable<Dice> DiceToKeep(int value) => availableDice.Where(d => d.Value >= value);

        public void Empty()
        {
            allDice.Clear();
            availableDice.Clear();
            discardedDice.Clear();
        }

        public void Roll()
        {
            foreach (var dice in availableDice)
            {
                dice.Roll();
            }
        }
    }
}

