using System;
using DiceGame;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDice : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image hoverGlow;
    [SerializeField] private Image discardGlow;
    [SerializeField] private Image icon;
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
        hoverGlow.enabled = true;
        hand.HoverDiceEnter(index);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!inHand) return;

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

        rectTransform.anchoredPosition += eventData.delta / canvasRectTransform.localScale.x;
    }

    public void FakeRoll()
    {
        var side = (FaceSides)UnityEngine.Random.Range(1, 7);
        this.icon.sprite = Sprites.Instance.Get(dice.GetFace(side).SpriteName);
    }
}
