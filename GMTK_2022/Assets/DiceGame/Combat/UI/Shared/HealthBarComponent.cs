using DiceGame.Utils;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarComponent : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] RectTransform healthBarImage;

    [Header("Ratio")]
    [SerializeField]
    [Range(0f, 1f)]
    float percentageRatio = 1;

    [Header("Color")]
    [SerializeField] bool isEnemy;
    [SerializeField] Color EnemyColor = new Color(1 * 255, 0 * 255, 0.4166775f * 255, 255);
    [SerializeField] Color FriendlyColor = new Color(0 * 255, 1 * 255, 0.659174f * 255, 255);

    TrackValueChange<bool> trackIsEnemyChange = new TrackValueChange<bool>();

    private void OnDrawGizmos()
    {
        RedrawImage();
    }

    void Update()
    {
        RedrawImage();
    }

    public void SetPercentageRatio(float value)
    {
        percentageRatio = Mathf.Clamp(value, 0, 1);
    }

    private void RedrawImage()
    {
        CheckColorChange();
        CheckImageSize();
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
