using System;
using UnityEngine;

namespace DiceGame.Combat.Presentation.Animations.Kinds
{
    public abstract class BaseAnimation
    {
        public bool IsCompleted { get; private set; }

        private Transform animatedTransform;
        private Vector3 sourceInitPos;
        private Vector3 targetInitPos;
        private float duration;
        private float time;

        private readonly Action callback;

        public BaseAnimation(Transform sourceTransform, Vector3 targetPos, float durationInSecs, Action callback)
        {
            animatedTransform = sourceTransform;
            sourceInitPos = sourceTransform.position;
            targetInitPos = targetPos;
            duration = durationInSecs;
            this.callback = callback;
            time = 0;
        }

        public void Update()
        {
            if (time < duration)
            {
                time += Time.deltaTime;
                var p = time / duration;
                float easeT = EaseFonction(p);

                if (time > duration)
                {
                    time = duration;
                    IsCompleted = true;
                }

                animatedTransform.position = Vector3.Lerp(sourceInitPos, targetInitPos, easeT);
            }
        }

        protected abstract float EaseFonction(float p);

        public void CompletedCallback()
        {
            callback?.Invoke();
        }
    }
}
