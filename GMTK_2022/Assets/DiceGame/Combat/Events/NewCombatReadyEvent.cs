using DiceGame.SharedKernel;

namespace DiceGame.Combat.Events
{
    public class NewCombatReadyEvent : IGameEvent
    {
        public override string ToString()
        {
            return $"{GetType().Name}";
        }
    }
}
