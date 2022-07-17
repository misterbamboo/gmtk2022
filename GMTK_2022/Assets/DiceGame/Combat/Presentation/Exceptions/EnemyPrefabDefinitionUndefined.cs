using DiceGame.Combat.Entities.EnemyAggregate;
using System;

namespace DiceGame.Combat.Presentation.Exceptions
{
    public class EnemyPrefabDefinitionUndefined : Exception
    {
        public EnemyPrefabDefinitionUndefined(EnemyType type)
            : base($"EnemyPrefabDefinition is undefined for enemy type {type}")
        {
        }
    }
}
