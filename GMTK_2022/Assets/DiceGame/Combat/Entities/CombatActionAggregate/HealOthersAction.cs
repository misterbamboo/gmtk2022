using System.Collections.Generic;
using System.Linq;
namespace DiceGame.Combat.Entities.CharacterAggregate
{
    public class HealOthersAction : CombatAction
    {
        public override string Description => "This unit will heal it's allies";
        public override string IconName => "heal_all";

        private int healAmount;
        private int sourceId;
        private List<int> targetIds;

        public HealOthersAction(int healAmount, int sourceId, IEnumerable<int> targets)
        {
            this.healAmount = healAmount;
            this.sourceId = sourceId;
            this.targetIds = targets.ToList();
        }
    }
}
