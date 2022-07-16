using Assets.DiceGame.SharedKernel;

namespace Assets.DiceGame.Combat.Events
{
    public class EnemySelectedEvent : IGameEvent
    {
        public int Id { get; }

        public EnemySelectedEvent(int id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return $"EnemySelectedEvent: Id={Id}";
        }
    }
}
