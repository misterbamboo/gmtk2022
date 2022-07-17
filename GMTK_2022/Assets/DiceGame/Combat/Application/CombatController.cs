using Assets.DiceGame.Combat.Entities.EnemyAggregate;
using Assets.DiceGame.Combat.Entities;
using Assets.DiceGame.Combat.Events;
using Assets.DiceGame.SharedKernel;
using Assets.DiceGame.Turn.Events;
using System;
using System.Collections.Generic;
using Assets.DiceGame.Combat.Application.Exceptions;
using Assets.DiceGame.Combat.Entities.CombatActionAggregate;
using System.Linq;
using Assets.DiceGame.Combat.Entities.Shared;
using Assets.DiceGame.Combat.Entities.Exceptions;

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
        private List<EnemyDecision> pendingEnemyDecisions = new List<EnemyDecision>();

        public CombatController(int minNumberOfEnnemies, int maxNumberOfEnemies, IDictionary<EnemyType, ICharacterStats> enemyStatsPerType, ICharacterStats playerStats)
        {
            this.minNumberOfEnnemies = minNumberOfEnnemies;
            this.maxNumberOfEnemies = maxNumberOfEnemies;
            this.enemyStatsPerType = enemyStatsPerType;
            Enemies = new List<Enemy>(maxNumberOfEnemies);
            Player = new Player(playerStats.MaxLife, playerStats.Attack);
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

        public void ResolveEnemyDecision(EnemyDecision enemyDecision)
        {
            var sourceCharacter = GetCharacterFromId(enemyDecision.SourceId);
            var targetCharacter = GetCharacterFromId(enemyDecision.TargetId);
            var damages = Translate(sourceCharacter, targetCharacter);
            HitCharacter(targetCharacter, damages);
            pendingEnemyDecisions.Remove(enemyDecision);
        }

        // TODO: Replace with Jeremie's transation mechanics
        private float Translate(ICharacter sourceCharacter, ICharacter targetCharacter)
        {
            return sourceCharacter.Attack;
        }

        private ICharacter GetCharacterFromId(int id)
        {
            if (id == Player.Id)
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
            HitCharacter(focusedEnemy, damages);

            if (focusedEnemy.IsDead())
            {
                OnUnfocusEnemy();
            }
        }

        private void HitCharacter(ICharacter character, float damages)
        {
            character.Hit(damages);

            if (character.IsDead())
            {
                if (Enemies.Contains(character))
                {
                    Enemies.Remove((Enemy)character);
                    GameEvents.Raise(new EnemyKilledEvent(character.Id));
                }
                else
                {
                    GameEvents.Raise(new PlayerKilledEvent());
                }
            }
        }

        public void OnTurnStarted(TurnStartedEvent e)
        {
            if (e.IsEnemyPlayerTurn())
            {
                foreach (var enemyDecision in enemyPlayerAI.EnemiesTakeDecisions())
                {
                    pendingEnemyDecisions.Add(enemyDecision);
                    GameEvents.Raise(new EnemyDecisionTakenEvent(enemyDecision));
                }
            }
        }
    }
}