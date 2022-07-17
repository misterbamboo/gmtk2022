using DiceGame.Combat.Presentation.Animations.Kinds;
using DiceGame.Combat.Entities.CharacterAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DiceGame.Combat.Presentation.Animations
{
    public class CombatAnimatorComponent : MonoBehaviour
    {
        Queue<BaseAnimation> animations = new Queue<BaseAnimation>();

        public void QueueAnimationForEnemyDecision(CombatAction combatAction, Transform sourceTransform, Transform targetTransform, Action callback)
        {
            switch (combatAction)
            {
                case AttackAction:
                    AddAttackAnimation(sourceTransform, targetTransform, callback);
                    break;
                case ShieldAction:
                    AddDefenseAnimation(sourceTransform, targetTransform, callback);
                    break;
                case HealAction:
                default:
                    break;
            }
        }

        private void AddAttackAnimation(Transform sourceTransform, Transform targetTransform, Action callback)
        {
            float sign = GetSignTowardTarget(sourceTransform, targetTransform);

            var newTarget = sourceTransform.position;
            newTarget.x += sign * 0.5f;
            animations.Enqueue(new AttackAnimation(sourceTransform, newTarget, durationInSecs: 0.2f, callback));
        }

        private void AddDefenseAnimation(Transform sourceTransform, Transform targetTransform, Action callback)
        {
            float invedtedSign = -GetSignTowardTarget(sourceTransform, targetTransform);

            var newTarget = sourceTransform.position;
            newTarget.x += 0.25f * invedtedSign;
            animations.Enqueue(new DefenseAnimation(sourceTransform, newTarget, durationInSecs: 1f, callback));
        }

        private static float GetSignTowardTarget(Transform sourceTransform, Transform targetTransform)
        {
            var diff = targetTransform.position - sourceTransform.position;
            return Mathf.Sign(diff.x);
        }

        public void Update()
        {
            if (animations.Any())
            {
                AnimateOne();
            }
        }

        private void AnimateOne()
        {
            var attackAnimation = animations.Peek();
            attackAnimation.Update();

            if (attackAnimation.IsCompleted)
            {
                attackAnimation.CompletedCallback();
                animations.Dequeue();
            }
        }
    }
}
