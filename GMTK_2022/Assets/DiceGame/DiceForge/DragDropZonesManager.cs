using System;
using UnityEngine;

namespace DiceGame
{
    public class DragDropZonesManager : MonoBehaviour
    {
        public event Action<Transform> OnSourceChanged;
        public event Action<Transform> OnTargetChanged;

        [SerializeField] DragDropBaseComponent bagZone;
        [SerializeField] DragDropBaseComponent sourceZone;
        [SerializeField] DragDropBaseComponent targetZone;

        private int dragKey;
        private DragDropBaseComponent dragFrom;
        private Transform dragTransform;
        private bool dropNextFrame;

        private void Start()
        {
            bagZone.onDrag.AddListener(OnDrag);
            bagZone.onDrop.AddListener(OnDrop);
            sourceZone.onDrag.AddListener(OnDrag);
            sourceZone.onDrop.AddListener(OnDrop);
            targetZone.onDrag.AddListener(OnDrag);
            targetZone.onDrop.AddListener(OnDrop);
        }

        private void Update()
        {
            if (dragTransform == null)
            {
                dropNextFrame = false;
            }
            else
            {
                var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                dragTransform.position = (Vector2)pos;

                if (!Input.GetMouseButton(0))
                {
                    if (dropNextFrame)
                    {
                        TryDrop(dragFrom, dragKey);
                    }
                    dropNextFrame = true;
                }
            }
        }

        private void OnDrag(DragDropBaseComponent component, int objectKey, Vector2 dragPos)
        {
            dragKey = objectKey;
            dragFrom = component;
            dragTransform = component.DragObjectByKey(objectKey);
            dragTransform.SetParent(null);

            RaiseTransformChanged(component, null);
        }

        private void OnDrop(DragDropBaseComponent component, int objectKey, Vector2 dropPos)
        {
            if (dragTransform != null)
            {
                TryDrop(component, objectKey);
            }
        }

        private void TryDrop(DragDropBaseComponent component, int key)
        {
            if (component.TryDropObjectByKey(key, dragTransform))
            {
                var dropTransform = dragTransform;

                dragKey = 0;
                dragFrom = null;
                dragTransform = null;

                RaiseTransformChanged(component, dropTransform);
            }
        }

        private void RaiseTransformChanged(DragDropBaseComponent component, Transform transform)
        {
            if (component == sourceZone)
            {
                OnSourceChanged?.Invoke(transform);
            }
            if (component == targetZone)
            {
                OnTargetChanged?.Invoke(transform);
            }
        }
    }
}
