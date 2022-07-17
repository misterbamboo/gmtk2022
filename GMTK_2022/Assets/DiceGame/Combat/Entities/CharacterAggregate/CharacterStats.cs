namespace DiceGame.Combat.Entities.EnemyAggregate
{
    public interface ICharacterStats
    {
        int Attack { get; }
        int Defence { get; }
        int MaxLife { get; }
        int Heal { get; }
    }

    public struct CharacterStats : ICharacterStats
    {
        public CharacterStats(int attack, int defence, int maxLife, int heal)
        {
            Attack = attack;
            Defence = defence;
            MaxLife = maxLife;
            Heal = heal;
        }

        public int Attack { get; }

        public int Defence { get; }

        public int MaxLife { get; }

        public int Heal { get; }
    }
}
