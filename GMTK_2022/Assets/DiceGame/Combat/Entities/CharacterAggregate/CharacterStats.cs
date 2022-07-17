namespace Assets.DiceGame.Combat.Entities.EnemyAggregate
{
    public interface ICharacterStats
    {
        float Attack { get; }
        float Defence { get; }
        float MaxLife { get; }
    }

    public struct CharacterStats : ICharacterStats
    {
        public CharacterStats(float attack, float defence, float maxLife)
        {
            Attack = attack;
            Defence = defence;
            MaxLife = maxLife;
        }

        public float Attack { get; }

        public float Defence { get; }

        public float MaxLife { get; }
    }
}
