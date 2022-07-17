using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.DiceGame.Combat.Presentation.Animations.Kinds
{
    public class AttackAnimation
    {
        public bool IsCompleted { get; private set; }

        private Transform animatedTransform;
        private Vector3 sourceInitPos;
        private Vector3 targetInitPos;
        private float duration;
        private float time;

        public AttackAnimation(Transform sourceTransform, Vector3 targetPos, float durationInSecs)
        {
            animatedTransform = sourceTransform;
            sourceInitPos = sourceTransform.position;
            targetInitPos = targetPos;
            duration = durationInSecs;
            time = 0;
        }

        public void Update()
        {
            if (time < duration)
            {
                time += Time.deltaTime;
                var p = time / duration;
                var easeT = Spike(p);

                if (time > duration)
                {
                    time = duration;
                    IsCompleted = true;
                }

                animatedTransform.position = Vector3.Lerp(sourceInitPos, targetInitPos, easeT);
            }
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
