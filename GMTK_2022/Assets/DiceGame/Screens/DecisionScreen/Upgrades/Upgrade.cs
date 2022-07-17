using System.Linq;
using DiceGame.GameData;
using UnityEngine;

namespace DiceGame
{
    public abstract class Upgrade
    {
        public abstract string Label { get; }
        public abstract string IconName { get; }

        public static Upgrade GeneratePlayer()
        {
            return UnityEngine.Random.Range(1, 5) switch
            {
                1 => StatBuffUpgrade.Player(),
                2 => NewDiceUpgrade.Random(),
                3 => RandomDiceUpgrade.Random(),
                4 => new CustomizationUpgrade(),
                _ => throw new System.Exception("Invalid upgrade type")
            };
        }

        public static Upgrade GenerateEnemy()
        {
            return UnityEngine.Random.Range(1, 6) switch
            {
                <= 4 => StatBuffUpgrade.Ennemy(),
                > 4 => StatBuffUpgrade.Battle(),
            };
        }
    }

    public class StatBuffUpgrade : Upgrade
    {
        public override string Label => $"+{ValueIncrease} to {StatKey.Replace("_", " ")}";
        public override string IconName => StatKey switch
        {
            string a when a.Contains("attack") => "attack",
            string b when b.Contains("armor") => "armor",
            string c when c.Contains("health") => "health",
            string d when d.Contains("heal") => "health", //TODO: add icon for heal
            _ => "upgrade"
        };

        public string StatKey { get; }
        public int ValueIncrease { get; }

        private StatBuffUpgrade(string statKey, int valueIncrease)
        {
            StatKey = statKey;
            ValueIncrease = valueIncrease;
        }

        public static StatBuffUpgrade Player()
        {
            var keys = StatKeys.Player.All;
            var statKey = keys.ElementAt(Random.Range(0, keys.Count()));
            var valueIncrease = Random.Range(1, 7);

            return new StatBuffUpgrade(statKey, valueIncrease);
        }

        public static StatBuffUpgrade Ennemy()
        {
            var keys = StatKeys.Ennemies.All;
            var statKey = keys.ElementAt(Random.Range(0, keys.Count()));
            var valueIncrease = Random.Range(1, 7);

            return new StatBuffUpgrade(statKey, valueIncrease);
        }

        public static StatBuffUpgrade Battle()
        {
            var keys = StatKeys.Battle.All;
            var statKey = keys.ElementAt(Random.Range(0, keys.Count()));
            var valueIncrease = 1;

            return new StatBuffUpgrade(statKey, valueIncrease);
        }
    }

    public class NewDiceUpgrade : Upgrade
    {
        public override string Label => $"+1 {Color.ToString()} Dice";
        public override string IconName => $"{Color.ToString()}_6";

        public DiceColors Color { get; }

        public NewDiceUpgrade(DiceColors color)
        {
            Color = color;
        }

        public static NewDiceUpgrade Random()
        {
            var allColors = System.Enum.GetValues(typeof(DiceColors)).Cast<DiceColors>();
            var color = allColors.ElementAt(UnityEngine.Random.Range(0, allColors.Count()));

            return new NewDiceUpgrade(color);
        }
    }

    public class RandomDiceUpgrade : Upgrade
    {
        public override string Label => "+1 Dice of any color";
        public override string IconName => "random_dice";
        public DiceColors Color { get; }

        public RandomDiceUpgrade(DiceColors color)
        {
            Color = color;
        }

        public static RandomDiceUpgrade Random()
        {
            var allColors = System.Enum.GetValues(typeof(DiceColors)).Cast<DiceColors>();
            var color = allColors.ElementAt(UnityEngine.Random.Range(0, allColors.Count()));

            return new RandomDiceUpgrade(color);
        }
    }

    public class CustomizationUpgrade : Upgrade
    {
        public override string Label => "Customize a dice";
        public override string IconName => "customization";

        public CustomizationUpgrade()
        {
        }
    }
}

