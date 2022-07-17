using Assets.DiceGame.Combat.Entities;
using Assets.DiceGame.SharedKernel;

namespace Assets.DiceGame.Combat.Events
{
    public class PlayerKilledEvent : IGameEvent
    {
        public int Id { get; }

        public PlayerKilledEvent()
        {
            Id = Player.Id;
        }

        public override string ToString()
        {
            return $"{GetType().Name}: Player={Id}";
        }
    }
}
