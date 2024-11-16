using UnityEngine;

public static class CameraExtensions {
  public static Vector2 GetPlaneAtDistance(this Camera camera, float distance) {
    return GetPlaneAtDistanceForCamera(camera, distance);
  }

  public static float GetDistanceThatContains(this Camera camera, Vector2 plane) {
    float nearPlaneHeight = CalculateNearPlaneHeight(camera);
    float distanceY = plane.y / nearPlaneHeight * camera.nearClipPlane;
    float distanceX = plane.x / (nearPlaneHeight * camera.aspect) * camera.nearClipPlane;
    return Mathf.Max(distanceX, distanceY);
  }

  public static float CalculateNearPlaneHeight(this Camera camera) {
    return 2.0f * Mathf.Tan(camera.fieldOfView * Mathf.Deg2Rad / 2.0f) * camera.nearClipPlane;
  }

  private static Vector2 GetPlaneAtDistanceForCamera(Camera camera, float distance) {
    var planeHeight = camera.CalculateNearPlaneHeight() * distance / camera.nearClipPlane; // TOA from SOSCASTOA
    var planeWidth = planeHeight * camera.aspect;
    return new Vector2(planeWidth, planeHeight);
  }

  private static float GetDistanceForPlaneHeight(Camera camera, float planeHeight) {
    float distance = planeHeight * camera.nearClipPlane / camera.CalculateNearPlaneHeight();
    return distance;
  }
}
