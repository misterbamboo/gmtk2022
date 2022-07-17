namespace DiceGame.Combat.Entities.CharacterAggregate
{
    public class ShieldSelfAction : CombatAction
    {
        public override string Description => "This unit will add armor to itself";
        public override string IconName => "armor";

        private Shield shield;
        private int sourceId;

        public ShieldSelfAction(Shield shield, int sourceId)
        {
            this.shield = shield;
            this.sourceId = sourceId;
        }
    }
}
