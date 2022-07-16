using UnityEngine;

public class HealthBarComponent : MonoBehaviour
{
    [SerializeField] RectTransform healthBarImage;

    [SerializeField]
    [Range(0f, 1f)]
    float percentageRatio = 1;

    private void OnDrawGizmos()
    {
        ResizeImage();
    }

    void Update()
    {
        ResizeImage();
    }

    public void SetPercentageRatio(float value)
    {
        percentageRatio = Mathf.Clamp(value, 0, 1);
    }

    private void ResizeImage()
    {
        if (healthBarImage != null)
        {
            var scale = healthBarImage.localScale;
            scale.x = percentageRatio;
            healthBarImage.localScale = scale;
        }
    }
}
