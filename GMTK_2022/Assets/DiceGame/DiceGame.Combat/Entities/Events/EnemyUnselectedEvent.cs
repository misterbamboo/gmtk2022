using Assets.DiceGame.SharedKernel;

namespace Assets.DiceGame.DiceGame.Combat.Entities.Events
{
    public class EnemyUnselectedEvent : IGameEvent
    {
        public int Id { get; }

        public EnemyUnselectedEvent(int id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return $"EnemyUnselectedEvent: Id={Id}";
        }
    }
}
