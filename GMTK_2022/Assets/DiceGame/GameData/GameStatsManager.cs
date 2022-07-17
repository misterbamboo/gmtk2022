using System.Collections.Generic;
using System.Linq;
using Assets.DiceGame.Combat.Entities.EnemyAggregate;
using DiceGame.GameData;
using UnityEngine;

public class GameStatsManager : MonoBehaviour
{
    [SerializeField] private GameStatsSheet gameStatsSheet;
    private Dictionary<string, int> gameStats;

    private void Awake()
    {
        gameStats = gameStatsSheet.Stats.ToDictionary(x => x.statKey, x => x.value);
    }

    public void ApplyStatBuff(string statKey, int valueIncrease)
    {
        if (gameStats.ContainsKey(statKey))
        {
            gameStats[statKey] += valueIncrease;
        }
        else
        {
            throw new System.Exception($"Stat key ({statKey}) not found in game stats sheet");
        }
    }

    public IDictionary<EnemyType, ICharacterStats> GetEnemyStats()
    {
        return new Dictionary<EnemyType, ICharacterStats>()
        {
            {
                EnemyType.SorryPawn,
                    new CharacterStats(
                        gameStats[StatKeys.Ennemies.pawn_attack],
                        gameStats[StatKeys.Ennemies.pawn_armor],
                        gameStats[StatKeys.Ennemies.pawn_health],
                        gameStats[StatKeys.Ennemies.pawn_heal])
            },
            {
                EnemyType.ChessKing,
                    new CharacterStats(
                        gameStats[StatKeys.Ennemies.king_attack],
                        gameStats[StatKeys.Ennemies.king_armor],
                        gameStats[StatKeys.Ennemies.king_health],
                        gameStats[StatKeys.Ennemies.king_heal])
            },
            {
                EnemyType.Domino,
                    new CharacterStats(
                        gameStats[StatKeys.Ennemies.domino_attack],
                        gameStats[StatKeys.Ennemies.domino_armor],
                        gameStats[StatKeys.Ennemies.domino_health],
                        gameStats[StatKeys.Ennemies.domino_heal])
            },
            {
                EnemyType.Iron,
                    new CharacterStats(
                        gameStats[StatKeys.Ennemies.iron_attack],
                        gameStats[StatKeys.Ennemies.iron_armor],
                        gameStats[StatKeys.Ennemies.iron_health],
                        gameStats[StatKeys.Ennemies.iron_heal])
            }
        };
    }
}

