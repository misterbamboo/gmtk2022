using System.Collections.Generic;

namespace DiceGame.GameData
{
    public static class StatKeys
    {
        public static class Player
        {
            public const string attack = "player_attack";
            public const string armor = "player_armor";
            public const string health = "player_health";
            public const string heal = "player_health";

            public static readonly IEnumerable<string> All = new string[]
            {
                attack,
                armor,
                health,
                heal
            };
        }

        public static class Battle
        {
            public const string min_ennemies = "battle_min_ennemies";
            public const string max_ennemies = "battle_max_ennemies";

            public static readonly IEnumerable<string> All = new string[]
            {
                min_ennemies,
                max_ennemies
            };
        }

        public static class Ennemies
        {
            public const string pawn_attack = "pawn_attack";
            public const string pawn_armor = "pawn_armor";
            public const string pawn_health = "pawn_health";
            public const string pawn_heal = "pawn_heal";

            public const string iron_attack = "iron_attack";
            public const string iron_armor = "iron_armor";
            public const string iron_health = "iron_health";
            public const string iron_heal = "iron_heal";

            public const string domino_attack = "domino_attack";
            public const string domino_armor = "domino_armor";
            public const string domino_health = "domino_health";
            public const string domino_heal = "domino_heal";

            public const string king_attack = "king_attack";
            public const string king_armor = "king_armor";
            public const string king_health = "king_health";
            public const string king_heal = "king_heal";

            public static readonly IEnumerable<string> All = new string[]
            {
                pawn_attack,
                pawn_armor,
                pawn_health,
                pawn_heal,
                iron_attack,
                iron_armor,
                iron_health,
                iron_heal,
                domino_attack,
                domino_armor,
                domino_health,
                domino_heal,
                king_attack,
                king_armor,
                king_health,
                king_heal
            };
        }
    }
}

