using DiceGame;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public const string Tag = "UpgradeManager";
    [SerializeField] private DiceBagComponent diceBagComponent;
    public GameStatsManager statsManager;

    public void ApplyUpgrade(Decision decision)
    {
        ApplyPlayerUpgrade(decision.PlayerUpgrade);
        ApplyEnemyUpgrade(decision.EnemyUpgrade);
    }

    public void ApplyPlayerUpgrade(Upgrade upgrade)
    {
        switch (upgrade)
        {
            case StatBuffUpgrade:
                ApplyStatBuffUpgrade(upgrade as StatBuffUpgrade);
                break;
            case NewDiceUpgrade:
                ApplyNewDiceUpgrade(upgrade as NewDiceUpgrade);
                break;
            case RandomDiceUpgrade:
                ApplyRandomDiceUpgrade(upgrade as RandomDiceUpgrade);
                break;
            case CustomizationUpgrade:
                ApplyCustomizationUpgrade(upgrade as CustomizationUpgrade);
                break;
            default:
                Debug.LogError("Unknown upgrade type: " + upgrade.GetType());
                break;
        }
    }

    public void ApplyEnemyUpgrade(Upgrade upgrade)
    {
        // as of right now ennemy upgrades are only stat buffs
        ApplyStatBuffUpgrade(upgrade as StatBuffUpgrade);
    }

    public void ApplyStatBuffUpgrade(StatBuffUpgrade upgrade)
    {
        statsManager.ApplyStatBuff(upgrade.StatKey, upgrade.ValueIncrease);
    }

    public void ApplyNewDiceUpgrade(NewDiceUpgrade upgrade)
    {
        diceBagComponent.AddDice(upgrade.Color);
    }

    public void ApplyRandomDiceUpgrade(RandomDiceUpgrade upgrade)
    {
        diceBagComponent.AddDice(upgrade.Color);
    }

    public void ApplyCustomizationUpgrade(CustomizationUpgrade upgrade)
    {
        Debug.LogWarning("Customization upgrade not implemented yet");
    }
}
