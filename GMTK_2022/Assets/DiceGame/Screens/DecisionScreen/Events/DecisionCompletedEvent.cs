using DiceGame.SharedKernel;

namespace DiceGame.Assets.DiceGame.DecisionScreen.Events
{
    public class DecisionCompletedEvent : IGameEvent
    {
        public override string ToString()
        {
            return $"{GetType().Name}: Ready to continue";
        }
    }
}
