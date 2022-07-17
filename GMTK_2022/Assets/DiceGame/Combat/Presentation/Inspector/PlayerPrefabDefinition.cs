using System;

namespace DiceGame.Combat.Presentation.Inspector
{
    [Serializable]

    public class PlayerPrefabDefinition
    {
        public PlayerComponent component;
        public float maxLife = 20;
    }
}
