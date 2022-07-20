using DiceGame.Assets.DiceGame.Screens.GameOverScreen.Events;
using DiceGame.Assets.DiceGame.Screens.MainMenuScreen.Events;
using DiceGame.SharedKernel;
using System;
using UnityEngine;

namespace DiceGame
{
    public class ScreenManagement : MonoBehaviour
    {
        [SerializeField] GameObject mainMenuScreen;
        [SerializeField] GameObject uiHand;

        private void Awake()
        {
            mainMenuScreen.SetActive(true);
            uiHand.SetActive(false);
        }

        private void Start()
        {
            GameEvents.Subscribe<NewGameRequestedEvent>(OnNewGameRequested);
            GameEvents.Subscribe<MainMenuRequestedEvent>(OnMainMenuRequested);
        }

        private void OnNewGameRequested(NewGameRequestedEvent obj)
        {
            mainMenuScreen.SetActive(false);
            uiHand.SetActive(true);
        }

        private void OnMainMenuRequested(MainMenuRequestedEvent obj)
        {
            mainMenuScreen.SetActive(true);
            uiHand.SetActive(false);
        }
    }
}
