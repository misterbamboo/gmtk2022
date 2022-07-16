namespace Assets.DiceGame.Combat.Entities.EnemyAggregate
{
    public interface ICharacterStats
    {
        float Attack { get; }
        float Defence { get; }
        float MaxLife { get; }
    }

    public class CharacterStats : ICharacterStats
    {
        public float Attack { get; set; }

        public float Defence { get; set; }

        public float MaxLife { get; set; }
    }
}
