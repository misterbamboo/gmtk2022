namespace DiceGame.Combat.Entities.CharacterAggregate
{
    public class AttackAction : CombatAction
    {
        public override string Description => "This unit will do damage to the player";
        public override string IconName => "attack";

        public Attack Attack { get; }

        public AttackAction(Attack attack, int sourceId, int targetId)
            : base(sourceId, targetId)
        {
            Attack = attack;
        }
    }
}
