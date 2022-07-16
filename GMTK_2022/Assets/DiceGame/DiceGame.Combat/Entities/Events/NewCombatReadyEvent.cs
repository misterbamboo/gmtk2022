using Assets.DiceGame.SharedKernel;

namespace Assets.DiceGame.DiceGame.Combat.Entities.Events
{
    public class NewCombatReadyEvent : IGameEvent
    {
        public override string ToString()
        {
            return $"NewCombatReadyEvent";
        }
    }
}
