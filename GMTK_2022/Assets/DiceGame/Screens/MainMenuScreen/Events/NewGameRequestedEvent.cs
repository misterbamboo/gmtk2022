using DiceGame.SharedKernel;

namespace DiceGame.Assets.DiceGame.Screens.MainMenuScreen.Events
{
    public class NewGameRequestedEvent : IGameEvent
    {
        public override string ToString()
        {
            return $"{GetType().Name}";
        }
    }
}
