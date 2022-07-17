using DiceGame.Combat.Entities.EnemyAggregate;
using DiceGame.Combat.Entities;
using System.Collections.Generic;
using DiceGame.Combat.Entities.CombatActionAggregate;

namespace DiceGame.Combat.Application
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

        public void EnemiesTakeDecisions()
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.TakeDecision(player, enemies);
            }
        }
    }
}
