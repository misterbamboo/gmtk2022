namespace DiceGame.Combat.Entities.CharacterAggregate
{
    public class AttackAction : CombatAction
    {
        public override string Description => "This unit will do damage to the player";
        public override string IconName => "attack";

        public Attack Attack { get; private set; }
        public int TargetId { get; private set; }
        public int SourceId { get; private set; }

        public AttackAction(Attack attack, int sourceId, int targetId)
        {
            Attack = attack;
            TargetId = targetId;
            SourceId = sourceId;
        }
    }
}
