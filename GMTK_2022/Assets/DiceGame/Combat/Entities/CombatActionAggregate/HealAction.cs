namespace DiceGame.Combat.Entities.CharacterAggregate
{
    public class HealAction : CombatAction
    {
        public override string Description => "This unit will heal it's allies";
        public override string IconName => "heal_all";

        public Heal Heal { get; }

        public HealAction(Heal heal, int sourceId, int targetId, params int[] targetIds)
            : base(sourceId, targetId, targetIds)
        {
            Heal = heal;
        }
    }
}
