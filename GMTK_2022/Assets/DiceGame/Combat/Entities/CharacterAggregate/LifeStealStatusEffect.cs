namespace DiceGame.Combat.Entities.CharacterAggregate
{
    public class LifeStealStatusEffect : StatusEffect
    {
        public override string Description => "converts damage dealt to this unit to life for the one who applied this";

        public override string IconName => "life_steal";

        public LifeStealStatusEffect(Character source, int duration)
            : base(source, duration)
        {
        }

        public override void OnReceiveDamage(int amount)
        {
            source.TakeHeal(amount);
            Duration--;
        }
    }
}