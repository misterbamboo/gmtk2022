using DiceGame.Combat.Entities.EnemyAggregate;
using DiceGame.Combat.Entities;
using DiceGame.Combat.Events;
using DiceGame.SharedKernel;
using System;
using System.Collections.Generic;
using DiceGame.Combat.Application.Exceptions;
using System.Linq;
using DiceGame.Combat.Entities.Exceptions;
using DiceGame.Combat.Entities.CharacterAggregate;

namespace DiceGame.Combat.Application
{
    public class CombatController
    {
        public Player Player { get; private set; }
        public List<Enemy> Enemies { get; private set; }
        public bool CombatFinished { get; private set; }

        private readonly int minNumberOfEnnemies;
        private readonly int maxNumberOfEnemies;
        private readonly IDictionary<EnemyType, ICharacterStats> enemyStatsPerType;

        public CombatController(int minNumberOfEnnemies, int maxNumberOfEnemies, IDictionary<EnemyType, ICharacterStats> enemyStatsPerType, ICharacterStats playerStats)
        {
            this.minNumberOfEnnemies = minNumberOfEnnemies;
            this.maxNumberOfEnemies = maxNumberOfEnemies;
            this.enemyStatsPerType = enemyStatsPerType;
            Enemies = new List<Enemy>(maxNumberOfEnemies);
            Player = new Player(playerStats);
        }

        public void StartCombat()
        {
            GenerateEnemies(minNumberOfEnnemies, maxNumberOfEnemies);
            GameEvents.Raise(new CombatStartedEvent());
        }

        private void GenerateEnemies(int minNumberOfEnnemies, int maxNumberOfEnemies)
        {
            var count = UnityEngine.Random.Range(minNumberOfEnnemies, maxNumberOfEnemies + 1);
            var enemyTypeValues = Enum.GetValues(typeof(EnemyType));
            for (int i = 0; i < count; i++)
            {
                var enemyTypeIndex = UnityEngine.Random.Range(0, enemyTypeValues.Length);
                var enemyType = (EnemyType)enemyTypeValues.GetValue(enemyTypeIndex);
                var enemyStats = GetEnemyStatsFromType(enemyType);

                Enemies.Add(new Enemy(enemyType, enemyStats));
            }
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

        public void DispatchCombatAction(CombatAction combatAction)
        {
            var targetCharacters = combatAction.TargetIds.Select(t => GetCharacterFromId(t)).ToList();
            targetCharacters.ForEach(c => c.ReceiveAction(combatAction));
        }

        private Character GetCharacterFromId(int id)
        {
            if (id == Player.PlayerId)
            {
                return Player;
            }

            var sourceEnemy = Enemies.FirstOrDefault(e => e.Id == id);
            if (sourceEnemy != null)
            {
                return sourceEnemy;
            }

            throw new CharacterNotFoundException(id);
        }

        public void EnemyTakeTurn(int id)
        {
            var enemy = Enemies.FirstOrDefault(e => e.Id == id);
            enemy.TakeDecision(Player, Enemies);
        }

        public void CheckWinningCondition()
        {
            if (Enemies.All(e => e.IsDead) || !Enemies.Any())
            {
                CombatFinished = true;
                GameEvents.Raise(new CombatEndedEvent(playerWon: true));
            }
            else if (Player.IsDead)
            {
                CombatFinished = true;
                GameEvents.Raise(new CombatEndedEvent(playerWon: false));
            }
        }
    }
}