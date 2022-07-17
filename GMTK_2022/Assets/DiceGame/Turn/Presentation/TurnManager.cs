using DiceGame.Assets.DiceGame.DecisionScreen.Events;
using DiceGame.Combat.Events;
using DiceGame.SharedKernel;
using DiceGame.Turn.Application;
using DiceGame.Turn.Events;
using System;
using UnityEngine;

namespace DiceGame.Turn.Presentation
{
    public class TurnManager : MonoBehaviour
    {
        [SerializeField] int numberOfPlayers = 2;
        private TurnController turnController;

        private bool combatEnded;

        private void Start()
        {
            GameEvents.Subscribe<TurnEndedEvent>(OnTurnEnded);
            GameEvents.Subscribe<CombatEndedEvent>(OnCombatEnded);
            GameEvents.Subscribe<DecisionCompletedEvent>(OnDecisionCompleted);
        }



        private void Update()
        {
            if (turnController == null)
            {
                StartNewTurns();
            }
        }

        private void OnTurnEnded(TurnEndedEvent turnEndedEvent)
        {
            if (!combatEnded)
            {
                turnController.EndTurn(turnEndedEvent.PlayerIndex);
            }
        }

        private void OnCombatEnded(CombatEndedEvent obj)
        {
            combatEnded = true;
        }

        private void OnDecisionCompleted(DecisionCompletedEvent obj)
        {
            StartNewTurns();
        }

        private void StartNewTurns()
        {
            combatEnded = false;
            turnController = new TurnController(numberOfPlayers);
            turnController.StartFistTurn();
        }
    }
}
