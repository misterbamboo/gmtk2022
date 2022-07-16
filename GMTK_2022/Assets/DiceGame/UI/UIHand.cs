using System.Collections.Generic;
using System.Linq;
using DiceGame;
using UnityEngine;

public class UIHand : MonoBehaviour
{
    [SerializeField] private GameObject uiDicePrefab;
    [SerializeField] private GameObject uiDiscardedDicePrefab;
    [SerializeField] private Transform discardedUI;
    [SerializeField] private Transform availableUI;

    private Hand hand;

    private List<UIDice> availableUiDices = new List<UIDice>();

    void Start()
    {
        hand = new Hand();
        hand.ResetDice(new List<Dice>()
        {
            Dice.WithValue(1),
            Dice.WithValue(2),
            Dice.WithValue(3),
            Dice.WithValue(4),
            Dice.WithValue(5),
            Dice.WithValue(6)
        });

        RedrawDice();
    }

    public void RedrawDice()
    {
        DestroyDice();
        CreateDice();
    }

    private void CreateDice()
    {
        for (var i = 0; i < hand.AvailableDice.Count(); i++)
        {
            var dice = hand.AvailableDice.ElementAt(i);
            var go = Instantiate(uiDicePrefab, availableUI);
            go.GetComponent<UIDice>().Init(i, dice, this);
            availableUiDices.Add(go.GetComponent<UIDice>());
        }

        for (var i = 0; i < hand.DiscardedDice.Count(); i++)
        {
            var dice = hand.DiscardedDice.ElementAt(i);
            var go = Instantiate(uiDiscardedDicePrefab, discardedUI);
            go.GetComponent<UIDiscardedDice>().Init(dice);
        }
    }

    private void DestroyDice()
    {
        availableUiDices.Clear();

        for (var i = discardedUI.childCount; i > 0; i--)
        {
            Destroy(discardedUI.GetChild(i - 1).gameObject);
        }

        for (var i = availableUI.childCount; i > 0; i--)
        {
            Destroy(availableUI.GetChild(i - 1).gameObject);
        }
    }

    public void Select(int index)
    {
        _ = hand.SelectDice(index);
        RedrawDice();
    }

    public void HoverDiceEnter(int index)
    {
        foreach (var d in ToBeDiscarded(index))
        {
            d.HoverDiscardEnter();
        }
    }

    public void HoverDiceExit(int index)
    {
        foreach (var d in ToBeDiscarded(index))
        {
            d.HoverDiscardExit();
        }
    }

    public IEnumerable<UIDice> ToBeDiscarded(int index) => availableUiDices.Where(d => d.Value < availableUiDices[index].Value);
}
