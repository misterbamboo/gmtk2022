using DiceGame.SharedKernel;

namespace DiceGame.Combat.Events
{
    public class CharacterTookDamageEvent : IGameEvent
    {
        public int Id { get; }
        public int Amount { get; }

        public CharacterTookDamageEvent(int id, int amount)
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
