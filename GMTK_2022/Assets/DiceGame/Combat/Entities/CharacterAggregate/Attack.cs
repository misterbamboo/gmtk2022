using System.Collections.Generic;
using System.Linq;

namespace DiceGame.Combat.Entities
{
    public class Attack
    {
        public int Amount { get; set; }
        public List<StatusEffect> StatusEffects { get; set; }

        public Attack(int amount, IEnumerable<StatusEffect> statusEffects = null)
        {
            Amount = amount;
            StatusEffects = statusEffects?.ToList() ?? new List<StatusEffect>();
        }
    }
}

