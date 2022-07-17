using Assets.DiceGame.Combat.Entities.EnemyAggregate;
using System;

public class EnemyComponent : LivingComponent
{
    public const string LayerMaskName = "Enemy";

    public Enemy Enemy { get; set; }

    public override int GetCharacterID()
    {
        return Enemy?.Id ?? -1;
    }

    protected override void UpdateEnemyInfo()
    {
        if (Enemy != null)
        {
            life = Enemy.Life;
            maxLife = Enemy.MaxLife;
        }
    }
}
