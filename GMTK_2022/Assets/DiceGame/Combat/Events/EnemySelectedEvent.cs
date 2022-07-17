using DiceGame.SharedKernel;

namespace DiceGame.Combat.Events
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
            return $"{GetType().Name}: EnemyId={Id}";
        }
    }
}
