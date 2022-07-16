using Assets.DiceGame.Utils;
using System.Collections;
using UnityEngine;

public abstract class LivingComponent : MonoBehaviour
{
    [SerializeField] protected float maxLife = 100;
    [SerializeField] protected float life;
    [SerializeField] protected HealthBarComponent healthBar;
    [SerializeField] protected Transform imageTransform;

    private TrackValueChange<float, float> lifeRatioChanges = new TrackValueChange<float, float>();

    void Start()
    {
        life = maxLife;
    }

    public void Shake()
    {
        StartCoroutine(ShakeThis());
    }

    private IEnumerator ShakeThis()
    {
        var initialTimestamp = Time.realtimeSinceStartup;

        var q = imageTransform.rotation;
        var eulerAngles = q.eulerAngles;

        float durationInSecs = 0;
        while (durationInSecs < 1)
        {
            durationInSecs = Time.realtimeSinceStartup - initialTimestamp;
            var rotationOffset = Mathf.Sin(durationInSecs * 30) * 5;

            var rotation = eulerAngles;
            rotation.z = eulerAngles.z + rotationOffset;

            imageTransform.rotation = Quaternion.Euler(rotation);
            yield return new WaitForEndOfFrame();
        }
        imageTransform.rotation = q;
    }

    private void OnDrawGizmos()
    {
        UpdateLifeRatio();
    }


    void Update()
    {
        UpdateEnemyInfo();
        UpdateLifeRatio();
    }

    protected abstract void UpdateEnemyInfo();

    private void UpdateLifeRatio()
    {
        lifeRatioChanges.Reset(life, maxLife);
        if (lifeRatioChanges.HasChanged)
        {
            var ratio = life / maxLife;
            RedrawHealthBar(ratio);
        }
    }

    private void RedrawHealthBar(float percentRatio)
    {
        healthBar.SetPercentageRatio(percentRatio);
    }
}
