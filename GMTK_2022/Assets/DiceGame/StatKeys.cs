using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceGame
{
    public static class StatKeys
    {
        public static class Player
        {
            public const string attack = "player_attack";
            public const string shield = "player_shield";
            public const string health = "player_health";

            public static readonly IEnumerable<string> All = new string[]
            {
                attack,
                shield,
                health
            };
        }

        public static class Battle
        {
            public const string min_ennemies = "battle_min_ennemies";
            public const string max_ennemies = "battle_min_ennemies";

            public static readonly IEnumerable<string> All = new string[]
            {
                min_ennemies,
                max_ennemies
            };
        }

        public static class Ennemies
        {
            public const string pawn_attack = "pawn_attack";
            public const string pawn_shield = "pawn_shield";
            public const string pawn_health = "pawn_health";

            public const string iron_attack = "iron_attack";
            public const string iron_shield = "iron_shield";
            public const string iron_health = "iron_health";

            public const string domino_attack = "domino_attack";
            public const string domino_shield = "domino_shield";
            public const string domino_health = "domino_health";

            public const string king_attack = "king_attack";
            public const string king_shield = "king_shield";
            public const string king_health = "king_health";

            public static readonly IEnumerable<string> All = new string[]
            {
                pawn_attack,
                pawn_shield,
                pawn_health,
                iron_attack,
                iron_shield,
                iron_health,
                domino_attack,
                domino_shield,
                domino_health,
                king_attack,
                king_shield,
                king_health
            };
        }
    }
}

