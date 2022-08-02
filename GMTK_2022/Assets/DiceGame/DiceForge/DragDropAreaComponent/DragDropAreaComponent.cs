using UnityEngine;

namespace DiceGame
{
    public class DragDropAreaComponent : DragDropBaseComponent
    {
        [SerializeField] float areaSize = 1;
        [SerializeField] Transform followPosition;

        DragDropAreaComponentGizmos gizmos = new DragDropAreaComponentGizmos();

        Transform placedObject = null;

        bool isHoverArea;
        float screenToWorldConversion = 0;


        private void Start()
        {
            screenToWorldConversion = GetScreenToWorldConversion();
        }

        private static float GetScreenToWorldConversion()
        {
            return (Camera.main.ScreenToWorldPoint(Vector2.one) - Camera.main.ScreenToWorldPoint(Vector2.zero)).x;
        }

        public override Transform DragObjectByKey(int objectKey)
        {
            if (objectKey != 0 || placedObject == null)
            {
                throw new System.Exception($"No object with key : {objectKey}");
            }
            var returnObj = placedObject;
            placedObject = null;
            return returnObj;
        }

        public override bool TryDropObjectByKey(int dragKey, Transform transform)
        {
            if (placedObject != null)
            {
                return false;
            }

            placedObject = transform;
            transform.SetParent(this.transform);
            return true;
        }

        private Rect GetArea()
        {
            var areaZone = (Vector2)followPosition.position;
            areaZone -= new Vector2(areaSize / 2, areaSize / 2);
            return new Rect(areaZone, new Vector2(areaSize, areaSize));
        }

        private void OnDrawGizmosSelected()
        {
            Rect areaZone = GetArea();
            gizmos.Init(GetScreenToWorldConversion());
            gizmos.DrawGizmos(areaZone, isHoverArea);
        }

        private void Update()
        {
            Rect area = GetArea();
            PlaceObjects(area);
            CheckDragDrop(area);
        }

        private void PlaceObjects(Rect area)
        {
            if (placedObject == true)
            {
                placedObject.position = area.position + area.size / 2;
            }
        }

        private void CheckDragDrop(Rect area)
        {
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            isHoverArea = false;
            if (area.Contains(mouseWorldPos))
            {
                isHoverArea = true;
                if (Input.GetMouseButtonDown(0))
                {
                    if (placedObject != null)
                    {
                        onDrag?.Invoke(this, 0, area.position);
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    onDrop?.Invoke(this, 0, area.position);
                }
            }
        }
    }
}
