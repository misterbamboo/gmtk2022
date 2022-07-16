using Assets.DiceGame.SharedKernel;

namespace Assets.DiceGame.Turn.Events
{
    public class TurnStartedEvent : IGameEvent
    {
        public int PlayerIndex { get; }
        public int NumberOfPlayers { get; }

        public TurnStartedEvent(int playerTurnIndex, int numberOfPlayers)
        {
            PlayerIndex = playerTurnIndex;
            NumberOfPlayers = numberOfPlayers;
        }

        public override string ToString()
        {
            return $"{GetType().Name}: Player={PlayerIndex}";
        }
    }
}
