using Assets.DiceGame.Combat.Entities;

public class PlayerComponent : LivingComponent
{
    public const string LayerMaskName = "Player";

    public Player Player { get; set; }

    public override int GetCharacterID()
    {
        return Player == null ? -1 : Player.Id;
    }

    protected override void UpdateEnemyInfo()
    {
        if (Player != null)
        {
            life = Player.Life;
            maxLife = Player.MaxLife;
        }
    }
}
