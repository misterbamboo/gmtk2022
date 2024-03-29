﻿
using DiceGame.Combat.Entities.EnemyAggregate;
using DiceGame.Combat.Entities.CharacterAggregate;

namespace DiceGame.Combat.Entities
{
    public class Player : Character
    {
        public const int PlayerId = 0;

        public Player(ICharacterStats stats)
            : base(PlayerId, stats)
        {
        }

        public void Attack(int targetId, Attack attack = null)
        {
            TakeAttackAction(targetId, attack);
        }

        public void Shield(int targetId, Shield shield = null)
        {
            TakeShieldAction(targetId, shield);
        }
    }
}
