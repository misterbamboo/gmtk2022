namespace Assets.DiceGame.Combat.Entities.EnemyAggregate
{
    public interface IEnemyStats
    {
        float Attack { get; }
        float Defence { get; }
        float MaxLife { get; }
    }


    public class EnemyStats : IEnemyStats
    {
        public float Attack { get; set; }

        public float Defence { get; set; }

        public float MaxLife { get; set; }
    }
}
