using System.Collections.Generic;
using System.Linq;
using DiceGame.Combat.Entities.EnemyAggregate;
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
            ApplyEnnemiesCountLimitations();
        }
        else
        {
            throw new System.Exception($"Stat key ({statKey}) not found in game stats sheet");
        }
    }

    private void ApplyEnnemiesCountLimitations()
    {
        var minEnemies = gameStats[StatKeys.Battle.min_ennemies];
        var maxEnemies = gameStats[StatKeys.Battle.max_ennemies];
        if (minEnemies > maxEnemies)
        {
            gameStats[StatKeys.Battle.max_ennemies] = minEnemies;
        }

        if (maxEnemies < minEnemies)
        {
            gameStats[StatKeys.Battle.max_ennemies] = minEnemies;
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

    public ICharacterStats GetPlayerStats()
    {
        return
            new CharacterStats(
                gameStats[StatKeys.Player.attack],
                gameStats[StatKeys.Player.armor],
                gameStats[StatKeys.Player.health],
                gameStats[StatKeys.Player.heal]);
    }

    public int GetMinEnemies() => gameStats[StatKeys.Battle.min_ennemies];

    public int GetMaxEnemies() => gameStats[StatKeys.Battle.min_ennemies];
}

