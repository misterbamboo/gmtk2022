using Assets.DiceGame.Combat.Entities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.DiceGame.Combat.Entities
{
    public class Player : ICharacter
    {
        public const int Id = 0;
        int ICharacter.Id => Id;

        public float MaxLife { get; private set; }
        public float Life { get; private set; }
        public float Attack { get; }

        public Player(float maxLife, float attack)
        {
            MaxLife = maxLife;
            Attack = attack;
            Life = maxLife;
        }

        public void ResetLife()
        {
            Life = MaxLife;
        }

        public void Hit(float damages)
        {
            var newLife = Life - damages;
            Life = Mathf.Clamp(0, MaxLife, newLife);
        }

        public bool IsDead()
        {
            return Life <= 0;
        }
    }
}
