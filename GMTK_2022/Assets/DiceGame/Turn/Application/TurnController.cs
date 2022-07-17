﻿using DiceGame.SharedKernel;
using DiceGame.Turn.Events;
using System;
using UnityEngine;

namespace DiceGame.Turn.Application
{
    public class TurnController
    {
        public bool IsFistTurnStarted { get; private set; }
        private int currentPlayerIndex;
        private int numberOfPlayers;

        public TurnController(int numberOfPlayers)
        {
            currentPlayerIndex = 0;
            this.numberOfPlayers = numberOfPlayers;
        }

        public void StartFistTurn()
        {
            IsFistTurnStarted = true;
            GameEvents.Raise(new TurnStartedEvent(currentPlayerIndex));
        }

        internal void EndTurn(int playerIndex)
        {
            if (currentPlayerIndex != playerIndex)
            {
                Debug.LogError("Not possible to endTurn of a player, if it's not the current player");
                return;
            }

            var nextPlayerIndex = currentPlayerIndex + 1;
            if (nextPlayerIndex >= numberOfPlayers)
            {
                nextPlayerIndex = 0;
            }

            currentPlayerIndex = nextPlayerIndex;
            GameEvents.Raise(new TurnStartedEvent(currentPlayerIndex));
        }
    }
}
