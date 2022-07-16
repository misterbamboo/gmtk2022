using Assets.DiceGame.DiceGame.Combat.Entities.EnemyAggregate;
using System;

namespace Assets.DiceGame.DiceGame.Combat.Presentation.Inspector
{
    [Serializable]
    public class EnemyPrefabDefinition
    {
        public EnemyType type;
        public EnemyComponent component;
    }
}
