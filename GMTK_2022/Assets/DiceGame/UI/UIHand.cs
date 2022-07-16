using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DiceGame;
using UnityEngine;
using UnityEngine.UI;

public enum HandState
{
    Selection,
    Roll,
}

public class UIHand : MonoBehaviour
{
    [SerializeField] private GameObject uiDicePrefab;
    [SerializeField] private GameObject uiDiscardedDicePrefab;
    [SerializeField] private Transform discardedUI;
    [SerializeField] private Transform availableUI;
    [SerializeField] private Button rollButton;
    [SerializeField] private Image diceCover;
    [SerializeField] private float rollTime = 1f;
    [SerializeField] private float rollPerSeconds = 10f;
    private Hand hand;

    private List<UIDice> availableUiDices = new List<UIDice>();

    public HandState State { get; private set; }

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

        State = HandState.Roll;
        RedrawUI();
    }

    public void RedrawUI()
    {
        DestroyDice();
        CreateDice();
        UpdateButton();
        UpdateCover();
    }

    private void UpdateButton()
    {
        rollButton.interactable = State == HandState.Roll;
    }

    private void UpdateCover()
    {
        diceCover.enabled = State != HandState.Selection;
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
        if (State != HandState.Selection) return;

        _ = hand.SelectDice(index);

        State = HandState.Roll;
        RedrawUI();
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

    public void Roll()
    {
        if (State != HandState.Roll) return;

        StartCoroutine(RollCoroutine());
    }

    private IEnumerator RollCoroutine()
    {
        var totaltime = 0f;
        var time = 0f;

        while (totaltime < rollTime)
        {
            time += Time.deltaTime;
            totaltime += Time.deltaTime;
            if (time > 1f / rollPerSeconds)
            {
                time = 0f;
                availableUiDices.ForEach(d => d.FakeRoll());
            }

            yield return null;
        }

        hand.Roll();
        State = HandState.Selection;

        RedrawUI();
    }

    public IEnumerable<UIDice> ToBeDiscarded(int index) => availableUiDices.Where(d => d.Value < availableUiDices[index].Value);
}
