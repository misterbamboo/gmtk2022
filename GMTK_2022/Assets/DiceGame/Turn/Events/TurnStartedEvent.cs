using Assets.DiceGame.SharedKernel;
using System;

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

        public bool IsEnemyPlayerTurn()
        {
            return PlayerIndex != 0;
        }

        public bool IsHumanPlayer()
        {
            return PlayerIndex == 0;
        }

        public override string ToString()
        {
            return $"{GetType().Name}: Player={PlayerIndex}";
        }
    }
}
