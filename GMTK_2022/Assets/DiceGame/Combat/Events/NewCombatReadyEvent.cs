using Assets.DiceGame.SharedKernel;

namespace Assets.DiceGame.Combat.Events
{
    public class NewCombatReadyEvent : IGameEvent
    {
        public override string ToString()
        {
            return $"{GetType().Name}";
        }
    }
}
