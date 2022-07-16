using Assets.DiceGame.Combat.Entities.CombatActionAggregate;
using Assets.DiceGame.Combat.Events;
using Assets.DiceGame.SharedKernel;
using System.Collections.Generic;

namespace Assets.DiceGame.Combat.Entities.EnemyAggregate
{
    public class Enemy
    {
        private static int nextId;

        public int Id { get; }
        public EnemyType Type { get; private set; }
        public float Life { get; private set; }
        public float MaxLife { get; private set; }
        public float Defence { get; private set; }
        public float Attack { get; private set; }

        public Enemy(EnemyType type, IEnemyStats enemyStats)
        {
            Id = ++nextId;
            Type = type;
            MaxLife = enemyStats.MaxLife;
            Life = enemyStats.MaxLife;
            Attack = enemyStats.Attack;
            Defence = enemyStats.Defence;
        }

        public void Hit(float damages)
        {
            Life -= damages;
            GameEvents.Raise(new EnemyTakeDamageEvent(Id, damages));
        }

        public virtual CombatAction TakeAction(Player player, List<Enemy> enemies)
        {
            if (Attack > player.Life)
            {
                return ChooseAttack();
            }
            else if (Life < MaxLife / 4)
            {
                return ChooseDefence();
            }
            else
            {
                return ChooseAttack();
            }
        }

        private CombatAction ChooseAttack()
        {
            return new CombatAction(CombatActionType.Attack, Id, Player.Id);
        }

        private CombatAction ChooseDefence()
        {
            return new CombatAction(CombatActionType.Defence, Id, Id);
        }

        public bool IsDead()
        {
            return Life <= 0;
        }
    }
}
