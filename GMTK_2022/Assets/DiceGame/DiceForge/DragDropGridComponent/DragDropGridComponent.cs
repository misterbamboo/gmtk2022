using System.Collections.Generic;
using UnityEngine;

namespace DiceGame
{
    public class DragDropGridComponent : DragDropBaseComponent
    {
        [SerializeField] float spawnzoneHeightPx = 125;
        [SerializeField] float spawnzoneSidePaddingPx = 25;
        [SerializeField] float spawnzoneBottomPaddingPx = 25;
        [SerializeField] float spawnzoneGridSizePx = 25;

        DragDropGridComponentGizmos gizmos = new DragDropGridComponentGizmos();

        Dictionary<int, Transform> placedObject = new Dictionary<int, Transform>();

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
            var obj = placedObject[objectKey];
            placedObject.Remove(objectKey);
            return obj;
        }

        public override bool TryDropObjectByKey(int dragKey, Transform transform)
        {
            if (placedObject.ContainsKey(dragKey))
            {
                return false;
            }
            placedObject[dragKey] = transform;
            transform.SetParent(this.transform);
            return true;
        }

        public Rect GetSpawnZone()
        {
            var spawnPos = Camera.main.ScreenToWorldPoint(new Vector2(spawnzoneSidePaddingPx, spawnzoneBottomPaddingPx));
            var spanwSizePos = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth - spawnzoneSidePaddingPx, spawnzoneBottomPaddingPx + spawnzoneHeightPx));
            var spawnSize = spanwSizePos - spawnPos;

            return new Rect(spawnPos, spawnSize);
        }

        private Vector2 hoverGridCell;

        private void OnDrawGizmosSelected()
        {
            Rect spawnZone = GetSpawnZone();
            gizmos.Init(GetScreenToWorldConversion(), spawnzoneGridSizePx);
            gizmos.DrawGizmos(spawnZone, hoverGridCell);
        }

        private void Update()
        {
            hoverGridCell = -Vector2.one;
            Rect spawnZone = GetSpawnZone();
            PlaceObjects(spawnZone);
            CheckDragDrop(spawnZone);
        }

        private void PlaceObjects(Rect spawnZone)
        {
            var stepSize = screenToWorldConversion * spawnzoneGridSizePx;
            float xCount = (int)(spawnZone.width / stepSize);
            var xStep = spawnZone.width / xCount;
            float yCount = (int)(spawnZone.height / stepSize);
            var yStep = spawnZone.height / yCount;

            foreach (var objectKey in placedObject.Keys)
            {
                var position = objectKey;
                var xIndex = position % (int)xCount;
                var yIndex = (int)(position / xCount);

                var x = xIndex * xStep + (xStep / 2);
                var y = yIndex * yStep + (yStep / 2);
                placedObject[objectKey].position = spawnZone.position + new Vector2(x, y);
            }
        }

        private void CheckDragDrop(Rect spawnZone)
        {
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (spawnZone.Contains(mouseWorldPos))
            {
                UpdateHoverGridCell(mouseWorldPos, spawnZone);

                if (Input.GetMouseButtonDown(0))
                {
                    Vector2 dragPos = GetHoverPos(spawnZone);
                    int objectKey = GetHoverObjectOrder(spawnZone);
                    if (placedObject.ContainsKey(objectKey))
                    {
                        onDrag?.Invoke(this, objectKey, dragPos);
                    }

                }
                else if (Input.GetMouseButtonUp(0))
                {
                    Vector2 dropPos = GetHoverPos(spawnZone);
                    int objectKey = GetHoverObjectOrder(spawnZone);
                    onDrop?.Invoke(this, objectKey, dropPos);
                }
            }
        }

        private int GetHoverObjectOrder(Rect spawnZone)
        {
            var stepSize = screenToWorldConversion * spawnzoneGridSizePx;
            int xCount = (int)(spawnZone.width / stepSize);

            var objectKey = (int)hoverGridCell.y * xCount + (int)hoverGridCell.x;
            return objectKey;
        }

        private Vector2 GetHoverPos(Rect spawnZone)
        {
            var stepSize = screenToWorldConversion * spawnzoneGridSizePx;
            float xCount = (int)(spawnZone.width / stepSize);
            var xStep = spawnZone.width / xCount;
            float yCount = (int)(spawnZone.height / stepSize);
            var yStep = spawnZone.height / yCount;
            return hoverGridCell * new Vector2(xStep, yStep);
        }

        private void UpdateHoverGridCell(Vector3 mouseWorldPos, Rect spawnZone)
        {
            var stepSize = screenToWorldConversion * spawnzoneGridSizePx;
            float xCount = (int)(spawnZone.width / stepSize);
            var xStep = spawnZone.width / xCount;
            float yCount = (int)(spawnZone.height / stepSize);
            var yStep = spawnZone.height / yCount;

            var mouseGridPos = (Vector2)mouseWorldPos - spawnZone.position;

            var xIndex = (int)(mouseGridPos.x / xStep);
            var yIndex = (int)(mouseGridPos.y / yStep);

            hoverGridCell = new Vector2(xIndex, yIndex);
        }
    }
}
