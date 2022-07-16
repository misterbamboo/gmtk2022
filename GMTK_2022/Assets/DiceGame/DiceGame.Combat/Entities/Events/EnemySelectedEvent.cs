using Assets.DiceGame.SharedKernel;

namespace Assets.DiceGame.DiceGame.Combat.Entities.Events
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
