using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DiceGame
{
    public class DiceBag : MonoBehaviour
    {
        private List<Dice> dices = new List<Dice>();
        private int currentDiceIndex = 0;

        public DiceBag(IEnumerable<Dice> dices)
        {
            this.dices = dices.ToList();
        }

        public IEnumerable<Dice> Draw(int count)
        {
            if (dices.Count < count)
            {
                return Enumerable.Empty<Dice>();
            }

            if (dices.Count < currentDiceIndex + count)
            {
                ResetBag();
            }

            var result = dices.Skip(currentDiceIndex).Take(count);
            currentDiceIndex += count;

            return result;
        }

        public void AddDice(Dice dice)
        {
            dices.Add(dice);
        }

        private void ResetBag()
        {
            Shuffle();
            currentDiceIndex = 0;
        }

        /// <summary>
        /// https://www.dotnetperls.com/fisher-yates-shuffle
        /// </summary>
        public void Shuffle()
        {
            int n = dices.Count;
            for (int i = 0; i < (n - 1); i++)
            {
                int r = i + Random.Range(0, n - i);
                var t = dices[r];
                dices[r] = dices[i];
                dices[i] = t;
            }
        }
    }
}

