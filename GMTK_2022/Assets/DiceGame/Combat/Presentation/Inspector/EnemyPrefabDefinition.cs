using DiceGame.Combat.Entities.EnemyAggregate;
using System;
using UnityEngine;

namespace DiceGame.Combat.Presentation.Inspector
{
    [Serializable]
    public class EnemyPrefabDefinition
    {
        public EnemyType type;
        public EnemyComponent component;
    }
}
