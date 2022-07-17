using DiceGame.SharedKernel;

namespace DiceGame.Assets.DiceGame.Screens.GameOverScreen.Events
{
    public class MainMenuRequestedEvent : IGameEvent
    {
        public override string ToString()
        {
            return $"{GetType().Name}";
        }
    }
}
