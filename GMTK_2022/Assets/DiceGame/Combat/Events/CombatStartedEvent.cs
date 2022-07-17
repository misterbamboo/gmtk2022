using DiceGame.SharedKernel;

namespace DiceGame.Combat.Events
{
    public class CombatStartedEvent : IGameEvent
    {
        public override string ToString()
        {
            return $"{GetType().Name}";
        }
    }
}
