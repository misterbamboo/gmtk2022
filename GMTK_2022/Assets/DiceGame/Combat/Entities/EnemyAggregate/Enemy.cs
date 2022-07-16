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

        public Enemy(EnemyType type, ICharacterStats stats)
        {
            Id = ++nextId;
            Type = type;
            MaxLife = stats.MaxLife;
            Life = stats.MaxLife;
            Attack = stats.Attack;
            Defence = stats.Defence;
        }

        public void Hit(float damages)
        {
            Life -= damages;
            GameEvents.Raise(new EnemyTakeDamageEvent(Id, damages));
        }

        public virtual EnemyDecision TakeDecision(Player player, List<Enemy> enemies)
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

        private EnemyDecision ChooseAttack()
        {
            return new EnemyDecision(EnemyDecisionType.Attack, Id, Player.Id);
        }

        private EnemyDecision ChooseDefence()
        {
            return new EnemyDecision(EnemyDecisionType.Defence, Id, Id);
        }

        public bool IsDead()
        {
            return Life <= 0;
        }
    }
}
