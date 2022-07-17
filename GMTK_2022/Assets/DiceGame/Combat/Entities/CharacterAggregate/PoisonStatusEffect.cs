namespace DiceGame.Combat.Entities.CharacterAggregate
{
    public class PoisonStatusEffect : StatusEffect
    {
        public override string Description => "this unit takes poison damage at the end of it's turn";

        public override string IconName => "poison";

        public PoisonStatusEffect(Character source, int duration)
            : base(source, duration)
        {
        }

        public override void OnTurnEnd()
        {
            target.TakeDamage(duration);
        }
    }
}