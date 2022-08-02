using UnityEngine;

namespace DiceGame
{
    public class DragDropZonesManager : MonoBehaviour
    {
        [SerializeField] DragDropBaseComponent diceBagZone;
        [SerializeField] DragDropBaseComponent sourceDiceZone;
        [SerializeField] DragDropBaseComponent targetDiceZone;
        private int dragKey;
        private DragDropBaseComponent dragFrom;
        private Transform dragTransform;
        private bool dropNextFrame;

        private void Start()
        {
            diceBagZone.onDrag.AddListener(OnDrag);
            diceBagZone.onDrop.AddListener(OnDrop);
            sourceDiceZone.onDrag.AddListener(OnDrag);
            sourceDiceZone.onDrop.AddListener(OnDrop);
            targetDiceZone.onDrag.AddListener(OnDrag);
            targetDiceZone.onDrop.AddListener(OnDrop);
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
                dragKey = 0;
                dragFrom = null;
                dragTransform = null;
            }
        }
    }
}
