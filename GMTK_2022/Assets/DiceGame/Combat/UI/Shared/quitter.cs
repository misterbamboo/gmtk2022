using UnityEngine;

namespace DiceGame
{
    public class quitter : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}
