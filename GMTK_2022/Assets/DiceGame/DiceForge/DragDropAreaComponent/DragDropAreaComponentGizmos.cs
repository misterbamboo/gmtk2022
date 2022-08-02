using UnityEngine;

namespace DiceGame
{
    public class DragDropAreaComponentGizmos
    {
        private float screenToWorldConversion;

        public void Init(float screenToWorldConversion)
        {
            this.screenToWorldConversion = screenToWorldConversion;
        }

        public void DrawGizmos(Rect area, bool isHoverArea)
        {
            var topRight = new Vector2(area.xMax, area.yMax);
            var bottomRight = new Vector2(area.xMax, area.yMin);
            var topLeft = new Vector2(area.xMin, area.yMax);
            var bottomLeft = new Vector2(area.xMin, area.yMin);

            DrawPoint(area, isHoverArea);
            DrawBounds(topRight, bottomRight, topLeft, bottomLeft);
        }

        private void DrawPoint(Rect area, bool isHoverArea)
        {
            if (isHoverArea)
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.green;
            }
            Gizmos.DrawSphere(area.position + area.size / 2, screenToWorldConversion * 3);
        }

        private static void DrawBounds(Vector2 topRight, Vector2 bottomRight, Vector2 topLeft, Vector2 bottomLeft)
        {
            Debug.DrawLine(topLeft, topRight, Color.green);
            Debug.DrawLine(topLeft, bottomLeft, Color.green);
            Debug.DrawLine(topRight, bottomRight, Color.green);
            Debug.DrawLine(bottomLeft, bottomRight, Color.green);
        }
    }
}
