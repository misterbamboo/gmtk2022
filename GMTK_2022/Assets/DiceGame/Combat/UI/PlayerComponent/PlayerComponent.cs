using DiceGame.Combat.Entities;
using DiceGame;
using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using DiceGame.Combat.Entities.CharacterAggregate;

public class PlayerComponent : CharacterComponent
{
    public const string LayerMaskName = "Player";
    public const string Tag = "Player";

    public void TakeDecision(Dice diceSelected, int targetId)
    {
        switch (diceSelected.Color)
        {
            case DiceColors.White:
                StartCoroutine(TakeAttackActionsCoroutine(diceSelected.Value, targetId));
                break;
            case DiceColors.Blue:
                TakeDefendAction(diceSelected.Value, targetId);
                break;
            case DiceColors.Orange:
                TakeAddWeaknessAction(diceSelected.Value, targetId);
                break;
            case DiceColors.Green:
                TakeAddLifeStealAction(diceSelected.Value, targetId);
                break;
            case DiceColors.Yellow:
                TakeAddSicknessAction(diceSelected.Value, targetId);
                break;
            case DiceColors.Purple:
                TakeAddPoisonAction(diceSelected.Value, targetId);
                break;
            default:
                break;
        }
    }

    private void TakeAddSicknessAction(int value, int targetId)
    {
        var statusEffect = new SicknessStatusEffect(Character, value);
        var attack = new Attack(0, new StatusEffect[] { statusEffect });
        ((Player)Character).Attack(targetId, attack);
    }

    private void TakeAddPoisonAction(int value, int targetId)
    {
        var statusEffect = new PoisonStatusEffect(Character, value);
        var attack = new Attack(0, new StatusEffect[] { statusEffect });
        ((Player)Character).Attack(targetId, attack);
    }

    private void TakeAddLifeStealAction(int value, int targetId)
    {
        var statusEffect = new LifeStealStatusEffect(Character, value);
        var attack = new Attack(0, new StatusEffect[] { statusEffect });
        ((Player)Character).Attack(targetId, attack);
    }

    private void TakeAddWeaknessAction(int value, int targetId)
    {
        var StatusEffect = new WeaknessStatusEffect(Character, value);
        var attack = new Attack(0, new StatusEffect[] { StatusEffect });
        ((Player)Character).Attack(targetId, attack);
    }

    private void TakeDefendAction(int value, int targetId)
    {
        var shield = new Shield(value * Character.Stats.Defence);
        ((Player)Character).Shield(Character.Id, shield);
    }

    public IEnumerator TakeAttackActionsCoroutine(int value, int targetId)
    {
        for (int i = 0; i < value; i++)
        {
            if (!CombatActionCancellationRequested)
            {
                ((Player)Character).Attack(targetId);
                yield return new WaitForSeconds(0.5f);
            }
        }
        CombatActionCancellationRequested = false;
    }
}
