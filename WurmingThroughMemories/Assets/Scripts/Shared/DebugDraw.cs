using UnityEngine;

    public static class DebugDraw {
        public static void DrawCross(Vector3 position,  Color color, float size , float duration, Quaternion rotation ) {
            Debug.DrawLine(position + rotation * Vector3.up * size/2f, position + rotation * Vector3.down * size/2f, color,duration);
            Debug.DrawLine(position + rotation * Vector3.left * size/2f, position + rotation * Vector3.right * size/2f, color,duration);
            Debug.DrawLine(position + rotation * Vector3.forward * size/2f, position + rotation * Vector3.back * size/2f, color,duration);
        }
        public static void DrawCross(Vector3 position,  Color color, float size =0.5f, float duration = 0 ) {
            DrawCross(position, color, size, duration, Quaternion.identity);
        }public static void DrawCross(Vector3 position  ) {
            DrawCross(position, Color.white);
        }
    }