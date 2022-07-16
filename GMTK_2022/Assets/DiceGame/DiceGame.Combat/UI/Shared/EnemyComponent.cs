using Assets.DiceGame.DiceGame.Combat.Entities.EnemyAggregate;
using Assets.DiceGame.DiceGame.Combat.Entities.Events;
using Assets.DiceGame.SharedKernel;
using Assets.DiceGame.Utils;
using System;
using System.Collections;
using UnityEngine;

public class EnemyComponent : MonoBehaviour
{
    public const string LayerMaskName = "Enemy";

    [SerializeField] float maxLife = 100;
    [SerializeField] float life;
    [SerializeField] HealthBarComponent healthBar;

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
        var q = transform.rotation;
        var eulerAngles = q.eulerAngles;
        for (float i = 0; i < 10; i++)
        {
            var rotation = eulerAngles;
            rotation.z = eulerAngles.z + (i % 2) * 2;

            transform.rotation = Quaternion.Euler(rotation);
            yield return new WaitForSeconds(0.1f);
        }
        transform.rotation = q;
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
