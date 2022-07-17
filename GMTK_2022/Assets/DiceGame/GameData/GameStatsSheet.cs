using System.Collections.Generic;
using UnityEngine;

namespace DiceGame.GameData
{
    [CreateAssetMenu(fileName = "NewGameStatsSheet", menuName = "DiceGame/StatsSheet", order = 1)]
    public class GameStatsSheet : ScriptableObject
    {
        public List<StatLine> Stats;
    }

    [System.Serializable]
    public struct StatLine
    {
        public string statKey;
        public int value;
    }
}

