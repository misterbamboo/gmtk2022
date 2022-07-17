using System.Collections.Generic;
using DiceGame;
using DiceGame.Assets.DiceGame.DecisionScreen.Events;
using DiceGame.SharedKernel;
using UnityEngine;

public class UIDecisionScreen : MonoBehaviour
{
    [SerializeField] private List<UIDecision> uiDecisions;
    private UpgradeManager upgradeManager;
    private List<Decision> decisions = new List<Decision>();
    private int selectedIndex = -1;

    private void Awake()
    {
        upgradeManager = GameObject.FindWithTag(UpgradeManager.Tag).GetComponent<UpgradeManager>();
    }

    private void Start()
    {
        decisions = GenerateDecisions();
        InitDecisionUis();
    }

    private void InitDecisionUis()
    {
        for (int i = 0; i < uiDecisions.Count; i++)
        {
            uiDecisions[i].Init(i, decisions[i], this);
        }
    }

    public void Select(int index)
    {
        uiDecisions.ForEach(d => d.Deselect());
        selectedIndex = index;
    }

    public void Continue()
    {
        ApplyUpgrade(selectedIndex);
        GameEvents.Raise(new DecisionCompletedEvent());
        Destroy(this.gameObject);
    }

    public void ApplyUpgrade(int index)
    {
        upgradeManager.ApplyUpgrade(decisions[index]);
    }

    public List<Decision> GenerateDecisions()
    {
        var newDecisions = new List<Decision>();
        for (int i = 0; i < 3; i++)
        {
            newDecisions.Add(Decision.Generate());
        }

        return newDecisions;
    }
}
