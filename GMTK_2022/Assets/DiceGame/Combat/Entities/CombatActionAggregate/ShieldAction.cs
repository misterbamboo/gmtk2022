namespace DiceGame.Combat.Entities.CharacterAggregate
{
    public class ShieldAction : CombatAction
    {
        public override string Description => "This unit will add armor to it's allies";

        public override string IconName => "armor_all";

        public Shield Shield { get; }

        public ShieldAction(Shield shield, int sourceId, int targetId, params int[] targetIds)
            : base(sourceId, targetId, targetIds)
        {
            Shield = shield;
        }
    }
}
