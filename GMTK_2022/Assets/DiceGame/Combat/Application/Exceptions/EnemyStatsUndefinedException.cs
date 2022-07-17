using DiceGame.Combat.Entities.EnemyAggregate;
using System;

namespace DiceGame.Combat.Application.Exceptions
{
    public class EnemyStatsUndefinedException : Exception
    {
        public EnemyStatsUndefinedException(EnemyType enemyType)
            : base($"EnemyStats for enemyType {enemyType} is undefined")
        {
        }
    }
}
