using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DiceGame;
using UnityEngine;
using UnityEngine.UI;

public enum HandState
{
    WaitingForSelection,
    Rolling,
    WaitingForRoll,
    EndTurn,
    WaitingForEnemies,
}

public class UIHand : MonoBehaviour
{
    [SerializeField] private GameObject uiDicePrefab;
    [SerializeField] private GameObject uiDiscardedDicePrefab;
    [SerializeField] private Transform discardedUI;
    [SerializeField] private Transform availableUI;
    [SerializeField] private Button rollButton;
    [SerializeField] private Button endTurnButton;
    [SerializeField] private Image diceCover;
    [SerializeField] private float rollTime = 1f;
    [SerializeField] private float rollPerSeconds = 10f;
    [SerializeField] private CombatManager combatManager;
    private Hand hand;
    private int diceUsed = 0;

    private List<UIDice> availableUiDices = new List<UIDice>();

    public HandState State { get; private set; }

    void Start()
    {
        hand = new Hand();
        ResetHand();
    }

    private void ResetHand()
    {
        diceUsed = 0;
        hand.ResetDice(new List<Dice>()
        {
            Dice.WithValue(1),
            Dice.WithValue(2),
            Dice.WithValue(3),
            Dice.WithValue(4),
            Dice.WithValue(5),
            Dice.WithValue(6)
        });

        State = HandState.WaitingForRoll;
        RedrawUI();
        Roll();
    }

    public void RedrawUI()
    {
        DestroyDice();
        CreateDice();
        UpdateRollButton();
        UpdateEndTurnButton();
        UpdateCover();
    }

    private void UpdateEndTurnButton()
    {
        endTurnButton.interactable = State == HandState.EndTurn;
    }

    private void UpdateRollButton()
    {
        rollButton.interactable = State == HandState.WaitingForRoll;
    }

    private void UpdateCover()
    {
        diceCover.enabled = State != HandState.WaitingForSelection;
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

    public void TrySelecting(int index)
    {
        if (State != HandState.WaitingForSelection) return;


        if (CheckForValidTarget())
        {
            Select(index);
        }
        else
        {
            RedrawUI();
        }
    }

    public bool CheckForValidTarget()
    {
        return combatManager.HasTarget;
    }

    private void Select(int index)
    {
        var diceSelected = hand.SelectDice(index);

        combatManager.HitTarget(diceSelected.Value);
        diceUsed++;
        if (diceUsed == 3 || hand.AvailableDice.Count() == 0)
        {
            State = HandState.EndTurn;
        }
        else
        {
            State = HandState.WaitingForRoll;
        }

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
        if (State != HandState.WaitingForRoll) return;
        State = HandState.Rolling;
        UpdateRollButton();

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
        State = HandState.WaitingForSelection;

        RedrawUI();
    }

    public void EndTurn()
    {
        ResetHand();
    }

    public IEnumerable<UIDice> ToBeDiscarded(int index) => availableUiDices.Where(d => d.Value < availableUiDices[index].Value);
}
