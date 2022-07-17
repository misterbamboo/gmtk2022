using DiceGame.Combat.Entities;
using DiceGame.Combat.Entities.CharacterAggregate;

public class WeaknessStatusEffect : StatusEffect
{
    public override string Description => "Attacks will crit against the target";
    public override string IconName => "weakness";

    public WeaknessStatusEffect(Character source, int duration) : base(source, duration)
    {
    }

    public override Attack OnReceiveAttack(Attack attack)
    {
        Duration--;
        return new Attack(attack.Amount * 2, attack.StatusEffects);
    }
}
