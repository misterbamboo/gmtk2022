namespace DiceGame.Combat.Entities.CharacterAggregate
{
    public class SicknessStatusEffect : StatusEffect
    {
        public override string Description => "this unit deals half damage";

        public override string IconName => "sickness";

        public SicknessStatusEffect(Character source, int duration)
            : base(source, duration)
        {
        }

        public override Attack OnAttacking(Attack attack)
        {
            return new Attack(attack.Amount / 2, attack.StatusEffects);
        }
    }
}