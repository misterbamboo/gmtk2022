using DiceGame.Combat.Entities;
using DiceGame;
using System;

public class PlayerComponent : CharacterComponent
{
    public const string LayerMaskName = "Player";
    public const string Tag = "Player";

    internal void TakeDecision(Dice diceSelected, int targetId)
    {
        var player = (Player)Character;
        player.TakeAction(diceSelected, targetId);
    }
}
