using DiceGame.SharedKernel;

namespace DiceGame.Combat.Events
{
    public class CharacterKilledEvent : IGameEvent
    {
        public int Id { get; }

        public CharacterKilledEvent(int id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return $"{GetType().Name}: Id={Id}";
        }
    }
}
