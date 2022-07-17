using System.Collections.Generic;
using System.Linq;
namespace DiceGame.Combat.Entities.CharacterAggregate
{
    public class ShieldOthersAction : CombatAction
    {
        public override string Description => "This unit will add armor to it's allies";

        public override string IconName => "armor_all";

        private int armorAmount;
        private int sourceId;
        private List<int> targetIds;

        public ShieldOthersAction(int armorAmount, int sourceId, IEnumerable<int> targets)
        {
            this.armorAmount = armorAmount;
            this.sourceId = sourceId;
            this.targetIds = targets.ToList();
        }
    }
}
