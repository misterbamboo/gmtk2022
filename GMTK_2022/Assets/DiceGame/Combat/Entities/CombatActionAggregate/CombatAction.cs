namespace DiceGame.Combat.Entities.CharacterAggregate
{
    public abstract class CombatAction
    {
        public abstract string Description { get; }
        public abstract string IconName { get; }
    }
}