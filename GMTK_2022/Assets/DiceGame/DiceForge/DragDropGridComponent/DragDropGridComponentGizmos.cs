using UnityEngine;

public class DragDropGridComponentGizmos
{
    private float screenToWorldConversion;
    private float spawnzoneGridSizePx;

    public void Init(float screenToWorldConversion, float spawnzoneGridSizePx)
    {
        this.screenToWorldConversion = screenToWorldConversion;
        this.spawnzoneGridSizePx = spawnzoneGridSizePx;
    }

    public void DrawGizmos(Rect spawnZone, Vector2 hoverGridCell)
    {
        var topRight = new Vector2(spawnZone.xMax, spawnZone.yMax);
        var bottomRight = new Vector2(spawnZone.xMax, spawnZone.yMin);
        var topLeft = new Vector2(spawnZone.xMin, spawnZone.yMax);
        var bottomLeft = new Vector2(spawnZone.xMin, spawnZone.yMin);

        DrawVerticalLines(spawnZone, topRight, topLeft, bottomLeft);
        DrawHorizontalLines(spawnZone, topRight, topLeft, bottomLeft);
        DrawPoints(spawnZone, topRight, topLeft, bottomLeft, hoverGridCell);
        DrawBounds(topRight, bottomRight, topLeft, bottomLeft);
    }

    private void DrawVerticalLines(Rect spawnZone, Vector2 topRight, Vector2 topLeft, Vector2 bottomLeft)
    {
        var stepSize = screenToWorldConversion * spawnzoneGridSizePx;
        float count = (int)(spawnZone.width / stepSize);
        var xStep = spawnZone.width / count;
        if (xStep > 0)
        {
            for (float lineX = topLeft.x; lineX < topRight.x; lineX += xStep)
            {
                var gridTopPos = new Vector2(lineX, topLeft.y);
                var gridBottomPos = new Vector2(lineX, bottomLeft.y);
                Debug.DrawLine(gridTopPos, gridBottomPos, Color.green);
            }
        }
    }

    private void DrawHorizontalLines(Rect spawnZone, Vector2 topRight, Vector2 topLeft, Vector2 bottomLeft)
    {
        var stepSize = screenToWorldConversion * spawnzoneGridSizePx;
        float count = (int)(spawnZone.height / stepSize);
        var yStep = spawnZone.height / count;
        if (yStep > 0)
        {
            for (float lineY = bottomLeft.y; lineY < topLeft.y; lineY += yStep)
            {
                var gridLeftPos = new Vector2(topLeft.x, lineY);
                var gridRightPos = new Vector2(topRight.x, lineY);
                Debug.DrawLine(gridLeftPos, gridRightPos, Color.green);
            }
        }
    }

    private void DrawPoints(Rect spawnZone, Vector2 topRight, Vector2 topLeft, Vector2 bottomLeft, Vector2 hoverGridCell)
    {
        var stepSize = screenToWorldConversion * spawnzoneGridSizePx;
        float xCount = (int)(spawnZone.width / stepSize);
        var xStep = spawnZone.width / xCount;
        float yCount = (int)(spawnZone.height / stepSize);
        var yStep = spawnZone.height / yCount;
        if (xStep > 0 && yStep > 0)
        {
            int xIndex = 0;
            for (float lineX = topLeft.x; lineX < topRight.x; lineX += xStep)
            {
                int yIndex = 0;
                for (float lineY = bottomLeft.y; lineY < topLeft.y; lineY += yStep)
                {
                    var pointX = lineX + xStep / 2;
                    var pointY = lineY + yStep / 2;

                    var point = new Vector2(pointX, pointY);
                    if (point.x < topRight.x && point.y < topRight.y)
                    {
                        if (hoverGridCell.x == xIndex && hoverGridCell.y == yIndex)
                        {
                            Gizmos.color = Color.red;
                        }
                        else
                        {
                            Gizmos.color = Color.green;
                        }
                        Gizmos.DrawSphere(point, screenToWorldConversion * 3);
                    }
                    yIndex++;
                }
                xIndex++;
            }
        }
    }

    private static void DrawBounds(Vector2 topRight, Vector2 bottomRight, Vector2 topLeft, Vector2 bottomLeft)
    {
        Debug.DrawLine(topLeft, topRight, Color.green);
        Debug.DrawLine(topLeft, bottomLeft, Color.green);
        Debug.DrawLine(topRight, bottomRight, Color.green);
        Debug.DrawLine(bottomLeft, bottomRight, Color.green);
    }
}
