using DiceGame.SharedKernel;
using System;

namespace DiceGame.Turn.Events
{
    public class TurnStartedEvent : IGameEvent
    {
        public int PlayerIndex { get; }

        public TurnStartedEvent(int playerTurnIndex)
        {
            PlayerIndex = playerTurnIndex;
        }

        public bool IsEnemyTurn => PlayerIndex != 0;
        public bool IsHumanTurn => PlayerIndex == 0;

        public override string ToString()
        {
            return $"{GetType().Name}: Player={PlayerIndex}";
        }
    }
}
