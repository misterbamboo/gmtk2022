using Assets.DiceGame.Combat.Entities.EnemyAggregate;

public class EnemyComponent : LivingComponent
{
    public const string LayerMaskName = "Enemy";

    public Enemy Enemy { get; set; }

    protected override void UpdateEnemyInfo()
    {
        if (Enemy != null)
        {
            life = Enemy.Life;
            maxLife = Enemy.MaxLife;
        }
    }
}
