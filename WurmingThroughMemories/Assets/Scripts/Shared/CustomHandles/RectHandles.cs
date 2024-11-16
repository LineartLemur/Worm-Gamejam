#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public static class RectHandles {
    public static Rect ResizeRect(Rect rect, Handles.CapFunction capFunc, Color capCol, Color fillCol, float capSize,
        float snap) {
        Vector2 halfRectSize = new Vector2(rect.size.x * 0.5f, rect.size.y * 0.5f);

        Vector3[] rectangleCorners = {
            new Vector3(rect.xMin, rect.yMin, 0), // Bottom Left
            new Vector3(rect.xMax, rect.yMin, 0), // Bottom Right
            new Vector3(rect.xMax, rect.yMax, 0), // Top Right
            new Vector3(rect.xMin, rect.yMax, 0) // Top Left
        };

        Handles.color = fillCol;
        Handles.DrawSolidRectangleWithOutline(rectangleCorners, new Color(fillCol.r, fillCol.g, fillCol.b, 0.25f),
            capCol);

        Vector3[] handlePoints = {
            new Vector3(rect.xMin, rect.center.y, 0), // Left
            new Vector3(rect.xMax, rect.center.y, 0), // Right
            new Vector3(rect.center.x, rect.yMax, 0), // Top
            new Vector3(rect.center.x, rect.yMin, 0) // Bottom 
        };

        Handles.color = capCol;

        var newSize = rect.size;
        var newPosition = rect.position;

        var leftHandle = Handles.Slider(handlePoints[0], -Vector3.right, capSize, capFunc, snap).x - handlePoints[0].x;
        var rightHandle = Handles.Slider(handlePoints[1], Vector3.right, capSize, capFunc, snap).x - handlePoints[1].x;
        var topHandle = Handles.Slider(handlePoints[2], Vector3.up, capSize, capFunc, snap).y - handlePoints[2].y;
        var bottomHandle = Handles.Slider(handlePoints[3], -Vector3.up, capSize, capFunc, snap).y - handlePoints[3].y;

        Rect newRect = rect;
        newRect.xMin += leftHandle;
        newRect.xMax += rightHandle;
        newRect.yMin += bottomHandle;
        newRect.yMax += topHandle;
        newSize = new Vector2(
            Mathf.Max(.1f, newSize.x - leftHandle + rightHandle),
            Mathf.Max(.1f, newSize.y + topHandle - bottomHandle));

        newPosition = new Vector2(
            newPosition.x + leftHandle * .5f + rightHandle * .5f,
            newPosition.y + topHandle * .5f + bottomHandle * .5f);

        return newRect;new Rect(newPosition.x, newPosition.y, newSize.x, newSize.y);
    }
}
#endif