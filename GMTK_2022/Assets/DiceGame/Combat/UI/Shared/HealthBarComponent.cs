using DiceGame;
using DiceGame.Combat.Entities.CharacterAggregate;
using DiceGame.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarComponent : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] RectTransform healthBarImage;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] Image healthImage;
    [SerializeField] CharacterComponent characterComponent;
    [SerializeField] GameObject statusIndicatorPrefab;
    [SerializeField] Transform statusIndicatorHolder;


    [Header("Ratio")]
    [SerializeField]
    [Range(0f, 1f)]
    float percentageRatio = 1;

    [Header("Color")]
    [SerializeField] bool isEnemy;
    [SerializeField] Color EnemyColor = new Color(1 * 255, 0 * 255, 0.4166775f * 255, 255);
    [SerializeField] Color FriendlyColor = new Color(0 * 255, 1 * 255, 0.659174f * 255, 255);

    TrackValueChange<bool> trackIsEnemyChange = new TrackValueChange<bool>();

    private void Start()
    {
        if (characterComponent == null)
        {
            Debug.LogError("characterComponent is null. Remember to bind you health bar to a character component");
        }
        RedrawImage();
    }

    public void RedrawImage()
    {
        var character = characterComponent?.Character;
        if (character == null) return;

        percentageRatio = Mathf.Clamp(character.CurrentHealth / character.MaxLife, 0, 1);
        SetIcon(character);
        SetLabel(character);
        SetStatusEffects(character);
        CheckColorChange();
        CheckImageSize();
    }

    private void SetIcon(Character character)
    {
        healthImage.sprite = character.CurrentArmor > 0 ? Sprites.Instance.Get("shield") : Sprites.Instance.Get("health");
    }

    private void SetLabel(Character character)
    {
        healthText.text = character.CurrentArmor > 0 ? character.CurrentArmor.ToString() : character.CurrentHealth.ToString();
    }

    private void SetStatusEffects(Character character)
    {
        //destroy all status effects in holder
        for (int i = statusIndicatorHolder.childCount; i > 0; i--)
        {
            Destroy(statusIndicatorHolder.GetChild(i - 1).gameObject);
        }

        foreach (var statusEffect in character.ActiveStatusEffects)
        {
            var statusIndicator = Instantiate(statusIndicatorPrefab, statusIndicatorHolder);
            statusIndicator.transform.Find("Icon").GetComponent<Image>().sprite = Sprites.Instance.Get(statusEffect.IconName);
            statusIndicator.transform.Find("Label").GetComponent<TextMeshProUGUI>().text = statusEffect.Duration.ToString();
        }
    }

    private void CheckColorChange()
    {
        trackIsEnemyChange.Reset(isEnemy);
        if (trackIsEnemyChange.HasChanged)
        {
            var image = healthBarImage.GetComponent<Image>();
            image.color = isEnemy ? EnemyColor : FriendlyColor;
        }
    }

    private void CheckImageSize()
    {
        if (healthBarImage != null)
        {
            var scale = healthBarImage.localScale;
            scale.x = percentageRatio;
            healthBarImage.localScale = scale;
        }
    }
}
