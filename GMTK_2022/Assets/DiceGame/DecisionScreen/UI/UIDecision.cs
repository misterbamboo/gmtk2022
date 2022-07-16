using DiceGame;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UIDecision : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Image selectBorder;
    [SerializeField] private Image hoverBackground;

    [SerializeField] private Image playerUpgradeIcon;
    [SerializeField] private TextMeshProUGUI playerUpgradeLabel;

    [SerializeField] private Image enemyUpgradeIcon;
    [SerializeField] private TextMeshProUGUI enemyUpgradeLabel;

    private UIDecisionScreen decisionScreen;
    private int index;

    public void Init(int index, Decision decision, UIDecisionScreen decisionScreen)
    {
        this.decisionScreen = decisionScreen;
        this.index = index;
        UpdateLabels(decision);
    }

    private void UpdateLabels(Decision decision)
    {
        playerUpgradeIcon.sprite = Sprites.Instance.Get(decision.playerUpgrade.IconName);
        playerUpgradeLabel.text = decision.playerUpgrade.Label;

        enemyUpgradeIcon.sprite = Sprites.Instance.Get(decision.EnemyUpgrade.IconName);
        enemyUpgradeLabel.text = decision.EnemyUpgrade.Label;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Select();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverBackground.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverBackground.enabled = false;
    }

    private void Select()
    {
        decisionScreen.Select(index);
        selectBorder.enabled = true;
    }

    public void Deselect()
    {
        selectBorder.enabled = false;
    }
}
