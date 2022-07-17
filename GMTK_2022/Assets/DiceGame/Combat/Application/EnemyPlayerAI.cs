using Assets.DiceGame.Combat.Entities.EnemyAggregate;
using Assets.DiceGame.Combat.Entities;
using System.Collections.Generic;
using Assets.DiceGame.Combat.Entities.CombatActionAggregate;

namespace Assets.DiceGame.Combat.Application
{
    public class EnemyPlayerAI
    {
        private Player player;
        private List<Enemy> enemies;

        public EnemyPlayerAI(Player player, List<Enemy> enemies)
        {
            this.player = player;
            this.enemies = enemies;
        }

        public IEnumerable<EnemyDecision> EnemiesTakeDecisions()
        {
            var enemyDecisions = new List<EnemyDecision>();
            foreach (Enemy enemy in enemies)
            {
                enemyDecisions.Add(enemy.TakeDecision(player, enemies));
            }
            return enemyDecisions;
        }
    }
}
