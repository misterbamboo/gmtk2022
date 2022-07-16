using DiceGame;
using UnityEngine;
using UnityEngine.UI;

public class UIDiscardedDice : MonoBehaviour
{
    [SerializeField] private Image icon;

    public void Init(Dice dice)
    {
        this.icon.sprite = Sprites.Instance.Get(dice.CurrentFace.SpriteName);
    }
}
