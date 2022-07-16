using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceGame
{
    public enum DecisionType
    {
        StatBuff,
        NewDice,
        Customization,
    }

    public struct Decision
    {
        public Upgrade playerUpgrade { get; }
        public Upgrade EnemyUpgrade { get; }

        public Decision(Upgrade playerUpgrade, Upgrade enemyUpgrade)
        {
            this.playerUpgrade = playerUpgrade;
            this.EnemyUpgrade = enemyUpgrade;
        }

        public static Decision Generate()
        {
            var playerUpgrade = Upgrade.GeneratePlayer();
            var enemyUpgrade = Upgrade.GenerateEnemy();

            return new Decision(playerUpgrade, enemyUpgrade);
        }
    }
}

