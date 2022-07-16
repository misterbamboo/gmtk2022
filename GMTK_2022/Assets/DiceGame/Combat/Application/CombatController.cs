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

        public Player Player { get; private set; }
        private float playerDefaultLife;

        public List<Enemy> Enemies { get; private set; }

        private readonly int minNumberOfEnnemies;
        private readonly int maxNumberOfEnemies;
        private readonly IDictionary<EnemyType, float> enemiesDefaultLife;

        private Enemy targetEnemy;

        public CombatController(int minNumberOfEnnemies, int maxNumberOfEnemies, IDictionary<EnemyType, float> enemiesDefaultLife, float playerDefaultLife)
        {
            this.minNumberOfEnnemies = minNumberOfEnnemies;
            this.maxNumberOfEnemies = maxNumberOfEnemies;
            this.enemiesDefaultLife = enemiesDefaultLife;
            this.playerDefaultLife = playerDefaultLife;
            Enemies = new List<Enemy>(maxNumberOfEnemies);
            Player = new Player(playerDefaultLife);
        }

        public void NewCombat()
        {
            Enemies.Clear();

            var count = UnityEngine.Random.Range(minNumberOfEnnemies, maxNumberOfEnemies + 1);
            var enemyTypeValues = Enum.GetValues(typeof(EnemyType));
            for (int i = 0; i < count; i++)
            {
                var enemyTypeIndex = UnityEngine.Random.Range(0, enemyTypeValues.Length);
                var enemyType = (EnemyType)enemyTypeValues.GetValue(enemyTypeIndex);
                var life = enemiesDefaultLife[enemyType];
                Enemies.Add(new Enemy(enemyType, life));
            }

            Player = new Player(playerDefaultLife);

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
                Enemies.Remove(enemyToHit);
                GameEvents.Raise(new EnemyKilledEvent(enemyToHit.Id));
            }
        }
    }
}