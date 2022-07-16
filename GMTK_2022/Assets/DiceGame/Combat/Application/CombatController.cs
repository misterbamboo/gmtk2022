using Assets.DiceGame.Combat.Entities.EnemyAggregate;
using Assets.DiceGame.Combat.Entities;
using Assets.DiceGame.Combat.Events;
using Assets.DiceGame.SharedKernel;
using Assets.DiceGame.Turn.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using Assets.DiceGame.Combat.Application.Exceptions;

namespace Assets.DiceGame.Combat.Application
{
    public class CombatController
    {
        public bool HasTarget => focusedEnemy != null;

        public Player Player { get; private set; }

        public List<Enemy> Enemies { get; private set; }
        private EnemyPlayerAI enemyPlayerAI;

        private readonly int minNumberOfEnnemies;
        private readonly int maxNumberOfEnemies;
        private readonly IDictionary<EnemyType, ICharacterStats> enemyStatsPerType;

        private Enemy focusedEnemy;

        public CombatController(int minNumberOfEnnemies, int maxNumberOfEnemies, IDictionary<EnemyType, ICharacterStats> enemyStatsPerType, float playerDefaultLife)
        {
            this.minNumberOfEnnemies = minNumberOfEnnemies;
            this.maxNumberOfEnemies = maxNumberOfEnemies;
            this.enemyStatsPerType = enemyStatsPerType;
            Enemies = new List<Enemy>(maxNumberOfEnemies);
            Player = new Player(playerDefaultLife);
            enemyPlayerAI = new EnemyPlayerAI(Player, Enemies);
        }

        public void NewCombat()
        {
            Enemies.Clear();
            Player.ResetLife();

            var count = UnityEngine.Random.Range(minNumberOfEnnemies, maxNumberOfEnemies + 1);
            var enemyTypeValues = Enum.GetValues(typeof(EnemyType));
            for (int i = 0; i < count; i++)
            {
                var enemyTypeIndex = UnityEngine.Random.Range(0, enemyTypeValues.Length);
                var enemyType = (EnemyType)enemyTypeValues.GetValue(enemyTypeIndex);
                var enemyStats = GetEnemyStatsFromType(enemyType);

                Enemies.Add(new Enemy(enemyType, enemyStats));
            }

            GameEvents.Raise(new NewCombatReadyEvent());
        }

        private ICharacterStats GetEnemyStatsFromType(EnemyType enemyType)
        {
            ICharacterStats stats;
            if (!enemyStatsPerType.ContainsKey(enemyType) || (stats = enemyStatsPerType[enemyType]) == null)
            {
                throw new EnemyStatsUndefinedException(enemyType);
            }
            return stats;
        }

        public void OnFocusEnemy(Enemy enemy)
        {
            if (focusedEnemy != null)
            {
                if (focusedEnemy.Id == enemy.Id)
                {
                    // Didn't changed ...
                    return;
                }

                GameEvents.Raise(new EnemyUnselectedEvent(focusedEnemy.Id));
            }

            focusedEnemy = enemy;
            GameEvents.Raise(new EnemySelectedEvent(focusedEnemy.Id));
        }

        public void OnUnfocusEnemy()
        {
            if (focusedEnemy != null)
            {
                var id = focusedEnemy.Id;
                focusedEnemy = null;
                GameEvents.Raise(new EnemyUnselectedEvent(id));
            }
        }

        public void HitFocusEnemy(float damages)
        {
            if (focusedEnemy == null) return;

            var enemyToHit = focusedEnemy;
            enemyToHit.Hit(damages);

            if (enemyToHit.IsDead())
            {
                OnUnfocusEnemy();
                Enemies.Remove(enemyToHit);
                GameEvents.Raise(new EnemyKilledEvent(enemyToHit.Id));
            }
        }

        public void OnTurnStarted(TurnStartedEvent e)
        {
            if (e.IsEnemyPlayerTurn())
            {
                enemyPlayerAI.EnemiesTakeActions();
            }
        }
    }
}