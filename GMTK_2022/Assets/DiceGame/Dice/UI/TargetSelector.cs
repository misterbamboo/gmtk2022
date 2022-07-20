using DiceGame.Combat.Entities.CharacterAggregate;
using System;
using UnityEngine;

namespace DiceGame.UI
{
    public class TargetSelector : MonoBehaviour
    {
        public const string Tag = "TargetSelector";
        private void Update()
        {
            UpdateTarget();
        }

        private void UpdateTarget()
        {
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.down, 1f, LayerMask.GetMask(EnemyComponent.LayerMaskName));
            if (hit.collider == null)
            {
                OnUnfocusEnemy();
            }
            else
            {
                var enemyComponent = hit.collider.GetComponent<EnemyComponent>();
                OnFocusEnemy(enemyComponent.Character);
            }
        }

        private Character focusedEnemy;
        public bool HasFocusedTarget => focusedEnemy != null;

        public int FocusedTargetId
        {
            get
            {
                if (focusedEnemy == null)
                {
                    throw new InvalidOperationException("Before calling TargetId, you need to call HasTarget");
                }
                return focusedEnemy.Id;
            }
        }

        public void OnFocusEnemy(Character character)
        {
            if (focusedEnemy != null)
            {
                if (focusedEnemy.Id == character.Id)
                {
                    // Didn't changed ...
                    return;
                }

            }

            focusedEnemy = character;
        }

        public void OnUnfocusEnemy()
        {
            if (focusedEnemy != null)
            {
                var id = focusedEnemy.Id;
                focusedEnemy = null;
            }
        }
    }
}
