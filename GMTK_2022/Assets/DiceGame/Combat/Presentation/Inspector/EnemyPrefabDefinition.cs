using Assets.DiceGame.Combat.Entities.EnemyAggregate;
using System;

namespace Assets.DiceGame.Combat.Presentation.Inspector
{
    [Serializable]
    public class EnemyPrefabDefinition
    {
        public EnemyType type;
        public EnemyComponent component;
    }
}
