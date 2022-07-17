using DiceGame.Assets.DiceGame.Screens.MainMenuScreen.Events;
using DiceGame.SharedKernel;
using UnityEngine;

namespace DiceGame
{
    public class MainMenuManager : MonoBehaviour
    {
        public void GameStart()
        {
            GameEvents.Raise(new NewGameRequestedEvent());
        }

        public void GameQuit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            UnityEditor.EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
    }
}
