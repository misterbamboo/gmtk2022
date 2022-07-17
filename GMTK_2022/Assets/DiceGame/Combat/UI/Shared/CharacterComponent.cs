using DiceGame.Utils;
using DiceGame.Combat.Entities.CharacterAggregate;
using System.Collections;
using UnityEngine;

public abstract class CharacterComponent : MonoBehaviour
{
    [SerializeField] protected float maxLife = 100;
    [SerializeField] protected float life;
    [SerializeField] protected HealthBarComponent healthBar;
    [SerializeField] protected Transform imageTransform;

    public int CharacterId => Character?.Id ?? -1;
    public Character Character { get; set; }

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

    protected void UpdateEnemyInfo()
    {
        if (Character != null)
        {
            life = Character.CurrentHealth;
            maxLife = Character.MaxLife;
        }
    }

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
