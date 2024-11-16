using UnityEngine;

//by Pepijn Willekens
// https://github.com/peperbol
// https://twitter.com/PepijnWillekens

public static class TransformExtension
{
	public static Bounds TransformBounds(this Transform _transform, Bounds _localBounds)
	{
		var center = _transform.TransformPoint(_localBounds.center);

		// transform the local extents' axes
		var extents = _localBounds.extents;
		var axisX = _transform.TransformVector(extents.x, 0, 0);
		var axisY = _transform.TransformVector(0, extents.y, 0);
		var axisZ = _transform.TransformVector(0, 0, extents.z);

		// sum their absolute value to get the world extents
		extents.x = Mathf.Abs(axisX.x) + Mathf.Abs(axisY.x) + Mathf.Abs(axisZ.x);
		extents.y = Mathf.Abs(axisX.y) + Mathf.Abs(axisY.y) + Mathf.Abs(axisZ.y);
		extents.z = Mathf.Abs(axisX.z) + Mathf.Abs(axisY.z) + Mathf.Abs(axisZ.z);

		return new Bounds { center = center, extents = extents };
	}

	public static void SetAnchorX(this RectTransform rt, float x) {
		rt.anchorMin = rt.anchorMin.ChangeX(x);
		rt.anchorMax = rt.anchorMax.ChangeX(x);
	}
	public static void SetAnchorX(this RectTransform rt, float min, float max) {
		rt.anchorMin = rt.anchorMin.ChangeX(min);
		rt.anchorMax = rt.anchorMax.ChangeX(max);
	}
	public static void SetAnchorY(this RectTransform rt, float y) {
		rt.anchorMin = rt.anchorMin.ChangeY(y);
		rt.anchorMax = rt.anchorMax.ChangeY(y);
	}public static void SetAnchorY(this RectTransform rt,  float min, float max) {
		rt.anchorMin = rt.anchorMin.ChangeY(min);
		rt.anchorMax = rt.anchorMax.ChangeY(max);
	}
	public static void SetWidth(this RectTransform rt, float width) {
		rt.sizeDelta = rt.sizeDelta.ChangeX(width);
	}
	public static void SetHeight(this RectTransform rt, float height) {
		rt.sizeDelta = rt.sizeDelta.ChangeY(height);
	}
	public static void SetOffsetX(this RectTransform rt, float min, float max) {
		rt.offsetMin = rt.offsetMin.ChangeX(min);
		rt.offsetMax = rt.offsetMax.ChangeX(max);
	}
	public static void SetOffsetY(this RectTransform rt, float min, float max) {
		rt.offsetMin = rt.offsetMin.ChangeY(min);
		rt.offsetMax = rt.offsetMax.ChangeY(max);
	}
	public static void SetAnchoredPositionX(this RectTransform rt, float x) {
		rt.anchoredPosition = rt.anchoredPosition.ChangeX(x);
	}
	public static void SetAnchoredPositionY(this RectTransform rt, float y) {
		rt.anchoredPosition = rt.anchoredPosition.ChangeY(y);
	}

	public static bool CastWorldRayToLocalPos(this RectTransform rt, Ray worldRay, out Vector2 pos) {
		pos = default;
		if (!CastWorldRayToPlane(rt, worldRay, out Vector3 worldPos)) return false;
		pos = rt.InverseTransformPoint(worldPos);
		return true;
	}
	public static bool CastWorldRayToPlane(this RectTransform rt, Ray worldRay, out Vector3 worldPos) {
		worldPos = default;
		if (!new Plane(rt.forward, rt.position).Raycast(worldRay, out var dist)) {
			if (!new Plane(-rt.forward, rt.position).Raycast(worldRay, out dist)) return false;
		}

		worldPos = worldRay.GetPoint(dist);
		return true;
	}
}