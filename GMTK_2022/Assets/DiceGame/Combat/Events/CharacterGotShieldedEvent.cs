using DiceGame.SharedKernel;

namespace DiceGame.Combat.Events
{
    public class CharacterGotShieldedEvent : IGameEvent
    {
        public int Id { get; }
        public int Amount { get; }

        public CharacterGotShieldedEvent(int id, int amount)
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
