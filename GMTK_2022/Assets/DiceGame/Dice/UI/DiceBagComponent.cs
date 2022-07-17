using System.Collections.Generic;
using DiceGame.Combat.Events;
using DiceGame.SharedKernel;
using UnityEngine;

namespace DiceGame
{
    public class DiceBagComponent : MonoBehaviour
    {
        public DiceBag diceBag;

        private void Awake()
        {
            GameEvents.Subscribe<CombatStartedEvent>(OnCombatStarted);
        }

        private void OnCombatStarted(CombatStartedEvent e)
        {
            var dices = new List<Dice>()
            {
                new Dice(DiceColors.White),
                new Dice(DiceColors.White),
                new Dice(DiceColors.White),
                new Dice(DiceColors.Orange),
                new Dice(DiceColors.Blue),
                new Dice(DiceColors.Blue),
            };

            diceBag = new DiceBag(dices);
        }

        public void AddDice(DiceColors colors)
        {
            diceBag.AddDice(new Dice(colors));
        }

        public IEnumerable<Dice> Draw()
        {
            return diceBag.Draw(6);
        }
    }
}
