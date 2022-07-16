using Assets.DiceGame.SharedKernel;
using Assets.DiceGame.Turn.Application;
using Assets.DiceGame.Turn.Events;
using UnityEngine;

namespace Assets.DiceGame.Turn.Presentation
{
    public class TurnManager : MonoBehaviour
    {
        [SerializeField] int numberOfPlayers = 2;
        private TurnController turnController;

        private void Start()
        {
            turnController = new TurnController(numberOfPlayers);
            GameEvents.Subscribe<TurnEndedEvent>(EventsReceiver);
        }

        private void Update()
        {
            if (!turnController.IsFistTurnStarted)
            {
                turnController.StartFistTurn();
            }
        }

        private void EventsReceiver(IGameEvent gameEvent)
        {
            if (gameEvent is TurnEndedEvent) turnController.EndTurn(((TurnEndedEvent)gameEvent).PlayerIndex);
        }
    }
}
