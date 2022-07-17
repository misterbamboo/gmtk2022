using DiceGame.SharedKernel;

namespace DiceGame.Combat.Events
{
    public class CharacterGotHealedEvent : IGameEvent
    {
        private int Id { get; }
        private int Amount { get; }

        public CharacterGotHealedEvent(int id, int amount)
        {
            Id = id;
            Amount = amount;
        }

        public override string ToString()
        {
            return $"{GetType().Name}: Id={Id}, Amount={Amount}";
        }
    }
}
