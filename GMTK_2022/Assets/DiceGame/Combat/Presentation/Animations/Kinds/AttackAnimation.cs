using System;
using UnityEngine;

namespace DiceGame.Combat.Presentation.Animations.Kinds
{
    public class AttackAnimation : BaseAnimation
    {
        public AttackAnimation(Transform sourceTransform, Vector3 targetPos, float durationInSecs, Action callback)
            : base(sourceTransform, targetPos, durationInSecs, callback)
        {
        }

        protected override float EaseFonction(float p)
        {
            return Spike(p);
        }

        // https://www.febucci.com/2018/08/easing-functions/
        public static float Spike(float t)
        {
            if (t <= .5f)
                return EaseIn(t / .5f);

            return EaseIn(Flip(t) / .5f);
        }

        public static float EaseIn(float t)
        {
            return t * t;
        }

        static float Flip(float x)
        {
            return 1 - x;
        }
    }
}
