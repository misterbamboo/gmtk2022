using Assets.DiceGame.SharedKernel;

namespace Assets.DiceGame.DiceGame.Combat.Entities.Events
{
    public class EnemyTakeDamageEvent : IGameEvent
    {
        public int Id { get; }
        public float Damages { get; }

        public EnemyTakeDamageEvent(int id, float damages)
        {
            Id = id;
            Damages = damages;
        }

        public override string ToString()
        {
            return $"EnemyTakeDamageEvent: Id={Id}, Damages={Damages}";
        }
    }
}
