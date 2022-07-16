using Assets.DiceGame.Combat.Entities.EnemyAggregate;

public class PlayerComponent : LivingComponent
{
    public const string LayerMaskName = "Player";

    public Player Player { get; set; }

    protected override void UpdateEnemyInfo()
    {
        if (Player != null)
        {
            life = Player.Life;
            maxLife = Player.MaxLife;
        }
    }
}
