using DiceGame.Combat.Entities;
using DiceGame.Combat.Entities.CharacterAggregate;

public abstract class StatusEffect
{
    public abstract string Description { get; }
    public abstract string IconName { get; }

    protected Character source;
    protected Character target;

    public int duration { get; protected set; }

    public StatusEffect(Character source, int duration)
    {
        this.source = source;
    }

    public void Apply(Character target)
    {
        this.target = target;
    }

    public void Refresh(int newDuration)
    {
        duration += newDuration;
    }

    public virtual Attack OnAttacking(Attack attack)
    {
        return attack;
    }

    public virtual Attack OnReceiveAttack(Attack attack)
    {
        return attack;
    }

    public virtual Shield OnShielding(Shield shield)
    {
        return shield;
    }

    public virtual Shield OnReceiveShield(Shield shield)
    {
        return shield;
    }

    public virtual Heal OnHealing(Heal heal)
    {
        return heal;
    }

    public virtual Heal OnReceiveHeal(Heal heal)
    {
        return heal;
    }

    public virtual void OnTurnStart() { }
    public virtual void OnTurnEnd() { }
}
