using System;
using DiceGame;
using DiceGame.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDice : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image hoverGlow;
    [SerializeField] private Image discardGlow;
    [SerializeField] private Image icon;
    private TargetSelector targetSelector;
    private RectTransform canvasRectTransform;
    private UIHand hand;
    private int index = 0;
    public bool inHand = false;
    public int Value { get; private set; }
    private Dice dice;

    void Awake()
    {
        hoverGlow.enabled = false;
        discardGlow.enabled = false;
        canvasRectTransform = GameObject.Find("Canvas").GetComponent<RectTransform>();
        targetSelector = GameObject.FindWithTag("TargetSelector").GetComponent<TargetSelector>();
    }

    public void Init(int index, Dice dice, UIHand hand)
    {
        this.dice = dice;
        this.index = index;
        this.hand = hand;
        Value = dice.Value;
        inHand = true;

        this.icon.sprite = Sprites.Instance.Get(dice.CurrentFace.SpriteName);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        print("swag");
        hoverGlow.enabled = true;
        hand.HoverDiceEnter(index);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!inHand) return;

        print("no swag");
        hoverGlow.enabled = false;
        hand.HoverDiceExit(index);
    }

    public void HoverDiscardEnter()
    {
        discardGlow.enabled = true;
    }

    public void HoverDiscardExit()
    {
        discardGlow.enabled = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        hoverGlow.enabled = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        gameObject.SetActive(false);

        if (inHand)
        {
            inHand = false;
            hand.TrySelecting(index);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!inHand) return;
        GlowIfOverTarget();

        RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRectTransform, eventData.position, eventData.pressEventCamera, out Vector3 worldPoint);
        transform.position = worldPoint;
    }

    private void GlowIfOverTarget()
    {
        if (targetSelector.HasFocusedTarget)
        {
            hoverGlow.enabled = true;
        }
        else
        {
            hoverGlow.enabled = false;
        }
    }

    public void FakeRoll()
    {
        var side = (FaceSides)UnityEngine.Random.Range(1, 7);
        this.icon.sprite = Sprites.Instance.Get(dice.GetFace(side).SpriteName);
    }
}
