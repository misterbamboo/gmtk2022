using Assets.DiceGame.SharedKernel;

namespace Assets.DiceGame.Combat.Events
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
            return $"{GetType().Name}: EnemyId={Id}";
        }
    }
}
