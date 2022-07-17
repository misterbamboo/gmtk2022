using Assets.DiceGame.Combat.Entities.CombatActionAggregate;
using Assets.DiceGame.Combat.Presentation.Animations.Kinds;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.DiceGame.Combat.Presentation.Animations
{
    public class CombatAnimatorComponent : MonoBehaviour
    {
        List<AttackAnimation> attackAnimations = new List<AttackAnimation>();

        public void QueueAnimationForEnemyDecision(EnemyDecisionType decision, Transform sourceTransform, Transform targetTransform)
        {
            if (decision == EnemyDecisionType.Attack)
            {
                var newTarget = sourceTransform.position;
                newTarget.x -= 0.5f;

                attackAnimations.Add(new AttackAnimation(sourceTransform, newTarget, durationInSecs: 0.2f));
            }
        }

        public void Update()
        {
            if (attackAnimations.Any())
            {
                AnimateOne();
            }
        }

        private void AnimateOne()
        {
            var attackAnimation = attackAnimations[0];
            attackAnimation.Update();

            if (attackAnimation.IsCompleted)
            {
                attackAnimations.RemoveAt(0);
            }
        }
    }
}
