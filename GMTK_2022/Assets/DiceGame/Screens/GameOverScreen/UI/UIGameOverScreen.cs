using DiceGame.Assets.DiceGame.Screens.GameOverScreen.Events;
using DiceGame.SharedKernel;
using UnityEngine;

namespace DiceGame
{
    public class UIGameOverScreen : MonoBehaviour
    {
        public void ReturnToMainMenu()
        {
            GameEvents.Raise(new MainMenuRequestedEvent());
            Destroy(this.gameObject);
        }
    }
}
