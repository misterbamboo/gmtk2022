using Assets.DiceGame.Combat.Entities.EnemyAggregate;
using Assets.DiceGame.Combat.Events;
using Assets.DiceGame.SharedKernel;
using Assets.DiceGame.Utils;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyComponent : MonoBehaviour
{
    public const string LayerMaskName = "Enemy";

    [SerializeField] float maxLife = 100;
    [SerializeField] float life;
    [SerializeField] HealthBarComponent healthBar;
    [SerializeField] Transform enemyImageTransform;

    private TrackValueChange<float, float> lifeRatioChanges = new TrackValueChange<float, float>();

    Action<EnemyTakeDamageEvent> takeDamageSubscription;

    private Enemy enemy;
    public void SetEnemy(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public Enemy GetEnemy()
    {
        return enemy;
    }

    public float GetMaxLife()
    {
        return maxLife;
    }

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

        var q = enemyImageTransform.rotation;
        var eulerAngles = q.eulerAngles;

        float durationInSecs = 0;
        while (durationInSecs < 1)
        {
            durationInSecs = Time.realtimeSinceStartup - initialTimestamp;
            var rotationOffset = Mathf.Sin(durationInSecs * 30) * 5;

            var rotation = eulerAngles;
            rotation.z = eulerAngles.z + rotationOffset;

            enemyImageTransform.rotation = Quaternion.Euler(rotation);
            yield return new WaitForEndOfFrame();
        }
        enemyImageTransform.rotation = q;
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

    private void UpdateEnemyInfo()
    {
        if (enemy != null)
        {
            life = enemy.Life;
            maxLife = enemy.MaxLife;
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
