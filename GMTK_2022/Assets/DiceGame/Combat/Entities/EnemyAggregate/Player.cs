using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.DiceGame.Combat.Entities.EnemyAggregate
{
    public class Player
    {
        public float MaxLife { get; private set; }
        public float Life { get; private set; }

        public Player(float maxLife)
        {
            MaxLife = maxLife;
            Life = maxLife;
        }
    }
}
