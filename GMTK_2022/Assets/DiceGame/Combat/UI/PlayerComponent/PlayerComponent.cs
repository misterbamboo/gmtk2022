using DiceGame.Combat.Entities;
using DiceGame;
using System.Collections;
using UnityEngine;

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
        }
    }

    public IEnumerator TakeAttackActionsCoroutine(int value, int targetId)
    {
        for (int i = 0; i < value; i++)
        {
            ((Player)Character).Attack(targetId);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
