using DiceGame.SharedKernel;

namespace DiceGame.Combat.Events
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
            return $"{GetType().Name}: EnemyId={Id}, Damages={Damages}";
        }
    }
}
