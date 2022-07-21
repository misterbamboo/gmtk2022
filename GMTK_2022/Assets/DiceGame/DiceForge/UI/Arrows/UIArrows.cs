using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace DiceGame
{
    public class UIArrows : MonoBehaviour
    {
        public const string LayerMaskName = "UIArrows";

        [SerializeField] float hoverSpeed = 2;
        [SerializeField] float growScale = 1.25f;

        [SerializeField] UnityEvent onLeftClicked;
        [SerializeField] UnityEvent onRightClicked;
        [SerializeField] UnityEvent onDownClicked;
        [SerializeField] UnityEvent onUpClicked;

        [SerializeField] Transform leftArrow;
        [SerializeField] Transform rightArrow;
        [SerializeField] Transform upArrow;
        [SerializeField] Transform downArrow;

        Dictionary<Transform, float> animationTimes = new Dictionary<Transform, float>();

        private Transform hoverTransform;
        private bool clicked;

        private void Start()
        {
            animationTimes[leftArrow] = 0;
            animationTimes[rightArrow] = 0;
            animationTimes[upArrow] = 0;
            animationTimes[downArrow] = 0;
        }

        private void Update()
        {
            CheckHover();
            CheckMouseDown();
            AnimateHover();
        }

        private void CheckHover()
        {
            hoverTransform = null;
            var point = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(point, Vector2.down, 1f, LayerMask.GetMask(UIArrows.LayerMaskName));
            if (hit.collider != null)
            {
                // Image is child of the collider
                hoverTransform = hit.collider.transform.GetChild(0).transform;
            }
        }

        private void CheckMouseDown()
        {
            var mouseDown = Input.GetMouseButtonDown(0);
            clicked = mouseDown || Input.GetMouseButton(0);

            if (mouseDown)
            {
                if (hoverTransform == leftArrow) onLeftClicked.Invoke();
                if (hoverTransform == rightArrow) onRightClicked.Invoke();
                if (hoverTransform == downArrow) onDownClicked.Invoke();
                if (hoverTransform == upArrow) onUpClicked.Invoke();
            }
        }

        private void AnimateHover()
        {
            foreach (var animationTimeKey in animationTimes.Keys.ToList())
            {
                if (animationTimeKey == hoverTransform)
                {
                    animationTimes[hoverTransform] += Time.deltaTime * hoverSpeed;
                    if (clicked)
                    {
                        animationTimes[hoverTransform] = 0;
                    }
                }
                else
                {
                    animationTimes[animationTimeKey] -= Time.deltaTime * hoverSpeed;
                }

                ScaleArrow(animationTimeKey, Vector2.one, Vector2.one * growScale);
            }
        }

        private void ScaleArrow(Transform arrowTransform, Vector3 minScale, Vector3 maxScale)
        {
            var p = animationTimes[arrowTransform];
            if (p > 1)
            {
                animationTimes[arrowTransform] = 1;
            }
            else if (p < 0)
            {
                animationTimes[arrowTransform] = 0;
            }

            var t = EasingFunction.EaseInOutQuint(0, 1, p);
            arrowTransform.localScale = Vector3.Lerp(minScale, maxScale, t);
        }
    }
}
