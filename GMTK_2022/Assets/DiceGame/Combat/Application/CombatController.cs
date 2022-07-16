using Assets.DiceGame.Combat.Entities.EnemyAggregate;
using Assets.DiceGame.Combat.Events;
using Assets.DiceGame.SharedKernel;
using System;
using System.Collections.Generic;

namespace Assets.DiceGame.Combat.Application
{
    public class CombatController
    {
        public bool HasTarget => targetEnemy != null;

        public List<Enemy> enemies { get; private set; }

        private readonly int minNumberOfEnnemies;
        private readonly int maxNumberOfEnemies;
        private readonly IDictionary<EnemyType, float> enemiesDefaultLife;
        private Enemy targetEnemy;

        public CombatController(int minNumberOfEnnemies, int maxNumberOfEnemies, IDictionary<EnemyType, float> enemiesDefaultLife)
        {
            this.minNumberOfEnnemies = minNumberOfEnnemies;
            this.maxNumberOfEnemies = maxNumberOfEnemies;
            this.enemiesDefaultLife = enemiesDefaultLife;
            enemies = new List<Enemy>(maxNumberOfEnemies);
        }

        public void NewCombat()
        {
            enemies.Clear();

            var count = UnityEngine.Random.Range(minNumberOfEnnemies, maxNumberOfEnemies + 1);
            var enemyTypeValues = Enum.GetValues(typeof(EnemyType));
            for (int i = 0; i < count; i++)
            {
                var enemyTypeIndex = UnityEngine.Random.Range(0, enemyTypeValues.Length);
                var enemyType = (EnemyType)enemyTypeValues.GetValue(enemyTypeIndex);
                var life = enemiesDefaultLife[enemyType];
                enemies.Add(new Enemy(enemyType, life));
            }

            GameEvents.Raise(new NewCombatReadyEvent());
        }

        public void Target(Enemy enemy)
        {
            if (targetEnemy != null)
            {
                if (targetEnemy.Id == enemy.Id)
                {
                    // Didn't changed ...
                    return;
                }

                GameEvents.Raise(new EnemyUnselectedEvent(targetEnemy.Id));
            }

            targetEnemy = enemy;
            GameEvents.Raise(new EnemySelectedEvent(targetEnemy.Id));
        }

        public void UnfocusTarget()
        {
            if (targetEnemy != null)
            {
                var id = targetEnemy.Id;
                targetEnemy = null;
                GameEvents.Raise(new EnemyUnselectedEvent(id));
            }
        }

        public void HitTarget(float damages)
        {
            if (targetEnemy == null) return;

            var enemyToHit = targetEnemy;
            enemyToHit.Hit(damages);

            if (enemyToHit.IsDead())
            {
                UnfocusTarget();
                enemies.Remove(enemyToHit);
                GameEvents.Raise(new EnemyKilledEvent(enemyToHit.Id));
            }
        }
    }
}