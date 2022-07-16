using Assets.DiceGame.SharedKernel;

namespace Assets.DiceGame.DiceGame.Combat.Entities.Events
{
    public class EnemyKilledEvent : IGameEvent
    {
        public int Id { get; }

        public EnemyKilledEvent(int id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return $"EnemyKilledEvent: Id={Id}";
        }
    }
}
