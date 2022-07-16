using Assets.DiceGame.Combat.Entities.EnemyAggregate;
using System;
using UnityEngine;

namespace Assets.DiceGame.Combat.Presentation.Inspector
{
    [Serializable]
    public class EnemyPrefabDefinition
    {
        public EnemyType type;
        public EnemyComponent component;
        public EnemyStatsDefinition stats;
    }

    [Serializable]
    public class EnemyStatsDefinition : IEnemyStats
    {
        [SerializeField] float attackPower;
        public float Attack => attackPower;

        [SerializeField] float defencePower;
        public float Defence => defencePower;

        [SerializeField] float maxLife;
        public float MaxLife => maxLife;
    }
}
