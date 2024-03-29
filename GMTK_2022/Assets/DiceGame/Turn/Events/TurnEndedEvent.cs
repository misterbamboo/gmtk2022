﻿using DiceGame.SharedKernel;

namespace DiceGame.Turn.Events
{
    public class TurnEndedEvent : IGameEvent
    {
        public int PlayerIndex { get; }

        public TurnEndedEvent(int playerIndex)
        {
            PlayerIndex = playerIndex;
        }

        public override string ToString()
        {
            return $"{GetType().Name}: Player={PlayerIndex}";
        }
    }
}
