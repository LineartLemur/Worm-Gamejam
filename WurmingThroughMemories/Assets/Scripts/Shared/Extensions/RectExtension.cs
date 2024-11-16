using UnityEngine;

//by Pepijn Willekens
// https://github.com/peperbol
// https://twitter.com/PepijnWillekens

public static class RectExtension
{

    public static Rect ChangeX(this Rect parent, float newX) {
        parent.x = newX;
        return parent;
    }

    public static Rect ChangeY(this Rect parent, float newY)
    {
        parent.y = newY;
        return parent;
    }
    
    public static Rect ChangeWidth(this Rect parent, float newWidth){
        parent.width = newWidth;
        return parent;
    }

    public static Rect ChangeHeight(this Rect parent, float newHeight){
        parent.height = newHeight;
        return parent;
    }

    public static Rect ChangeX(this Rect parent, VectorExtension.FloatEdit edit) {
        parent.x = edit(parent.x);
        return parent;
    }

    public static Rect ChangeY(this Rect parent, VectorExtension.FloatEdit edit)
    {
        parent.y = edit(parent.y);
        return parent;
    }
    
    public static Rect ChangeWidth(this Rect parent, VectorExtension.FloatEdit edit){
        parent.width = edit(parent.width);
        return parent;
    }

    public static Rect ChangeHeight(this Rect parent, VectorExtension.FloatEdit edit){
        parent.height = edit(parent.height);
        return parent;
    }
    public static Vector2 PosToAnchor(this Rect parent, Vector2 pos) {
        return (pos - parent.min) / parent.size;
    }
    
    public static void DrawGizmos(this Rect bounds, float z = 0, bool cross = false) {
        Gizmos.DrawLine(new Vector3(bounds.xMin, bounds.yMin,z), new Vector3(bounds.xMax, bounds.yMin,z));
        Gizmos.DrawLine(new Vector3(bounds.xMin, bounds.yMax,z), new Vector3(bounds.xMax, bounds.yMax,z));
        Gizmos.DrawLine(new Vector3(bounds.xMin, bounds.yMin,z), new Vector3(bounds.xMin, bounds.yMax,z));
        Gizmos.DrawLine(new Vector3(bounds.xMax, bounds.yMin,z), new Vector3(bounds.xMax, bounds.yMax,z));
        if (cross) {
            Gizmos.DrawLine(new Vector3(bounds.xMin, bounds.yMin,z), new Vector3(bounds.xMax, bounds.yMax,z));
            Gizmos.DrawLine(new Vector3(bounds.xMax, bounds.yMin,z), new Vector3(bounds.xMin, bounds.yMax,z));
        }
    }
    public static void DrawDebug(this Rect bounds, float z = 0, bool cross = false) {
        Debug.DrawLine(new Vector3(bounds.xMin, bounds.yMin,z), new Vector3(bounds.xMax, bounds.yMin,z));
        Debug.DrawLine(new Vector3(bounds.xMin, bounds.yMax,z), new Vector3(bounds.xMax, bounds.yMax,z));
        Debug.DrawLine(new Vector3(bounds.xMin, bounds.yMin,z), new Vector3(bounds.xMin, bounds.yMax,z));
        Debug.DrawLine(new Vector3(bounds.xMax, bounds.yMin,z), new Vector3(bounds.xMax, bounds.yMax,z));
        if (cross) {
            Debug.DrawLine(new Vector3(bounds.xMin, bounds.yMin,z), new Vector3(bounds.xMax, bounds.yMax,z));
            Debug.DrawLine(new Vector3(bounds.xMax, bounds.yMin,z), new Vector3(bounds.xMin, bounds.yMax,z));
        }
    }

}