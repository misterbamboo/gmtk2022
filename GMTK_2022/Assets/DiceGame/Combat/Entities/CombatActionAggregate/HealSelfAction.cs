using UnityEngine;
namespace DiceGame.Combat.Entities.CharacterAggregate
{
    public class HealSelfAction : CombatAction
    {
        public override string Description => "This unit will heal itself";

        public override string IconName => "heal";

        private Heal heal;
        private int sourceId;

        public HealSelfAction(Heal heal, int sourceId)
        {
            this.heal = heal;
            this.sourceId = sourceId;
        }
    }
}
