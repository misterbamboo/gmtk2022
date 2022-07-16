using Assets.DiceGame.DiceGame.Combat.Entities.Events;
using Assets.DiceGame.SharedKernel;
using System;

namespace Assets.DiceGame.DiceGame.Combat.Entities.EnemyAggregate
{
    public class Enemy
    {
        private static int nextId;

        public int Id { get; }
        public EnemyType Type { get; private set; }
        public float Life { get; private set; }
        public float MaxLife { get; private set; }

        public Enemy(EnemyType type, float maxLife)
        {
            Id = ++nextId;
            Type = type;
            MaxLife = maxLife;
            Life = maxLife;
        }

        public void Hit(int damages)
        {
            Life -= damages;
            GameEvents.Raise(new EnemyTakeDamageEvent(Id, damages));
        }

        public bool IsDead()
        {
            return Life <= 0;
        }
    }
}
