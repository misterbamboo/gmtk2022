using DiceGame.SharedKernel;

namespace DiceGame.Combat.Events
{
    public class CombatEndedEvent : IGameEvent
    {
        public bool PlayerWon { get; }

        public CombatEndedEvent(bool playerWon)
        {
            PlayerWon = playerWon;
        }

        public override string ToString()
        {
            return $"{GetType().Name}: {(PlayerWon ? "Congrats !" : "Better luck next time !")}";
        }
    }
}
