using DiceGame.Utils;
using DiceGame.Combat.Entities.CharacterAggregate;
using System.Collections;
using UnityEngine;
using System;

public abstract class CharacterComponent : MonoBehaviour
{
    [SerializeField] protected HealthBarComponent healthBar;
    [SerializeField] protected Transform imageTransform;

    public int CharacterId => Character?.Id ?? -1;
    public Character Character { get; set; }

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

    public void UpdateUIs()
    {
        RedrawHealthBar(Character.CurrentHealth / Character.MaxLife);
    }

    private void RedrawHealthBar(float percentRatio)
    {
        healthBar.SetPercentageRatio(percentRatio);
    }

    private void OnDrawGizmos()
    {
        RedrawHealthBar(Character.CurrentHealth / Character.MaxLife);
    }
}
