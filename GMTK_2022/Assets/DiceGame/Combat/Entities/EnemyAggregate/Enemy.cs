using DiceGame.Combat.Entities.CombatActionAggregate;
using DiceGame.Combat.Events;
using DiceGame.SharedKernel;
using DiceGame.Combat.Entities.CharacterAggregate;
using System.Collections.Generic;

namespace DiceGame.Combat.Entities.EnemyAggregate
{
    public class Enemy : Character
    {
        private static int nextId;

        public EnemyType Type { get; private set; }

        public Enemy(EnemyType type, ICharacterStats stats)
            : base(++nextId, stats)
        {
            Type = type;
        }

        public void TakeDecision(Player player, List<Enemy> enemies)
        {
            if (stats.Attack > player.CurrentHealth)
            {
                TakeAttackAction(Player.PlayerId);
            }
            else if (currentHealth < MaxLife / 4)
            {
                TakeShieldAction();
            }
            else
            {
                TakeAttackAction(Player.PlayerId);
            }
        }

        private EnemyDecisionObsolete ChooseAttack()
        {
            return new EnemyDecisionObsolete(EnemyDecisionType.Attack, Id, Player.PlayerId);
        }

        private EnemyDecisionObsolete ChooseDefence()
        {
            return new EnemyDecisionObsolete(EnemyDecisionType.Defence, Id, Id);
        }
    }
}
