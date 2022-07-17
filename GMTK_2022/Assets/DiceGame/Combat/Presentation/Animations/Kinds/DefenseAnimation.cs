using System;
using UnityEngine;

namespace DiceGame.Combat.Presentation.Animations.Kinds
{
    public class DefenseAnimation : BaseAnimation
    {
        public DefenseAnimation(Transform sourceTransform, Vector3 targetPos, float durationInSecs, Action callback)
            : base(sourceTransform, targetPos, durationInSecs, callback)
        {
        }

        protected override float EaseFonction(float p)
        {
            if (p < 0.5f)
            {
                return EasingFunction.EaseInOutSine(0, 1, p * 2);
            }

            p = (1 - p) * 2f;
            return EasingFunction.EaseInOutSine(0, 1, p);
        }
    }
}
