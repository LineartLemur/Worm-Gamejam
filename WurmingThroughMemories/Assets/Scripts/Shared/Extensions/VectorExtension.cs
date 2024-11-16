using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

//by Pepijn Willekens
// https://github.com/peperbol
// https://twitter.com/PepijnWillekens

public static class VectorExtension {
    //Vector3

    public static Vector3 ChangeX(this Vector3 parent, float newX) {
        return new Vector3(newX, parent.y, parent.z);
    }

    public static Vector3 ChangeY(this Vector3 parent, float newY) {
        return new Vector3(parent.x, newY, parent.z);
    }

    public static Vector3 ChangeZ(this Vector3 parent, float newZ) {
        return new Vector3(parent.x, parent.y, newZ);
    }

    public static Vector3 ChangeX(this Vector3 parent, FloatEdit edit) {
        return new Vector3(edit(parent.x), parent.y, parent.z);
    }

    public static Vector3 ChangeY(this Vector3 parent, FloatEdit edit) {
        return new Vector3(parent.x, edit(parent.y), parent.z);
    }

    public static Vector3 ChangeZ(this Vector3 parent, FloatEdit edit) {
        return new Vector3(parent.x, parent.y, edit(parent.z));
    }
    //Vector2

    public static Vector2 ChangeX(this Vector2 parent, float newX) {
        return new Vector2(newX, parent.y);
    }

    public static Vector2 ChangeY(this Vector2 parent, float newY) {
        return new Vector2(parent.x, newY);
    }

    public static Vector2 ChangeX(this Vector2 parent, FloatEdit edit) {
        return new Vector2(edit(parent.x), parent.y);
    }

    public static Vector2 ChangeY(this Vector2 parent, FloatEdit edit) {
        return new Vector2(parent.x, edit(parent.y));
    }

    public delegate float FloatEdit(float input);

    public static void SetX(this Vector3 v, float newX) {
        v.x = newX;
    }

    public static void SetY(this Vector3 v, float newY) {
        v.y = newY;
    }

    public static void SetZ(this Vector3 v, float newZ) {
        v.z = newZ;
    }

    public static void SetX(this Vector2 v, float newX) {
        v.x = newX;
    }

    public static void SetY(this Vector2 v, float newY) {
        v.y = newY;
    }

    public static string ToDetailedString(this Vector2 v) {
        return v.ToString("F5");
    }

    public static string ToDetailedString(this Vector3 v) {
        return v.ToString("F5");
    }

    public static Vector3 AddX(this Vector3 parent, float changeX) {
        return new Vector3(parent.x + changeX, parent.y, parent.z);
    }

    public static Vector3 AddY(this Vector3 parent, float changeY) {
        return new Vector3(parent.x, parent.y + changeY, parent.z);
    }

    public static Vector3 AddZ(this Vector3 parent, float changeZ) {
        return new Vector3(parent.x, parent.y, parent.z + changeZ);
    }

    public static Vector2 AddX(this Vector2 parent, float changeX) {
        return new Vector2(parent.x + changeX, parent.y);
    }

    public static Vector2 AddY(this Vector2 parent, float changeY) {
        return new Vector2(parent.x, parent.y + changeY);
    }

    public static Vector2 Abs(this Vector2 v) {
        return new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));
    }

    public static Vector3 Abs(this Vector3 v) {
        return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
    }

    public static Vector3 ChangeXY(this Vector3 v, Vector2 xy) {
        return new Vector3(xy.x, xy.y, v.z);
    }

    public static Vector3 ChangeXY(this Vector3 v, float x, float y) {
        return new Vector3(x, y, v.z);
    }

    public static Vector3 ChangeXZ(this Vector3 v, Vector2 xz) {
        return new Vector3(xz.x, v.y, xz.y);
    }

    public static Vector3 ChangeXZ(this Vector3 v, float x, float z) {
        return new Vector3(x, v.y, z);
    }

    public static Vector3 ChangeYZ(this Vector3 v, Vector2 yz) {
        return new Vector3(v.x, yz.y, yz.y);
    }

    public static Vector3 ChangeYZ(this Vector3 v, float y, float z) {
        return new Vector3(v.x, y, z);
    }

    public static Vector3 AddXY(this Vector3 v, Vector2 xy) {
        return new Vector3(v.x + xy.x, v.y + xy.y, v.z);
    }

    public static Vector3 AddXZ(this Vector3 v, Vector2 xz) {
        return new Vector3(v.x + xz.x, v.y, v.z + xz.y);
    }

    public static Vector2 GetXZ(this Vector3 v) {
        return new Vector2(v.x, v.z);
    }

    public static Vector2 GetXY(this Vector3 v) {
        return new Vector2(v.x, v.y);
    }

    public static Vector2 GetYZ(this Vector3 v) {
        return new Vector2(v.y, v.z);
    }

    public static Vector3 ToVector3(this Vector2 v) {
        return new Vector3(v.x, v.y, 0);
    }

    public static Vector3 ToVector3(this Vector2 v, float z) {
        return new Vector3(v.x, v.y, z);
    }
    
    public static Vector3 ToVector3(this Vector2Int v) {
        return new Vector3(v.x, v.y, 0);
    }

    public static Vector3 ToVector3(this Vector2Int v, float z) {
        return new Vector3(v.x, v.y, z);
    }

    public static bool EqualsVector(this Vector3 v1, Vector3 v2) {
        return Mathf.Approximately(v1.x, v2.x) && Mathf.Approximately(v1.y, v2.y) && Mathf.Approximately(v1.z, v2.z);
    }

    public static Vector2 Rotate(this Vector2 v, float degrees) {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }

    public static Vector2 Rotate90CCW(this Vector2 v) => new Vector2(-v.y, v.x);
    public static Vector2 Rotate90CW(this Vector2 v) => new Vector2(v.y, -v.x);
    public static Vector2Int Rotate90CCW(this Vector2Int v) => new Vector2Int(-v.y, v.x);
    public static Vector2Int Rotate90CW(this Vector2Int v) => new Vector2Int(v.y, -v.x);

    public static Vector2Int ToInt(this Vector2 v) {
        return new Vector2Int((int)v.x, (int)v.y);
    }

    public static Vector2 ToFloat(this Vector2Int v) {
        return new Vector2(v.x, v.y);
    }

    public static int GetDominantAxis(this Vector3 v) {
        v.x = Mathf.Abs(v.x);
        v.y = Mathf.Abs(v.y);
        v.z = Mathf.Abs(v.z);
        if (v.x >= v.y && v.x >= v.z) return 0;
        else if (v.y >= v.x && v.y >= v.z) return 1;
        return 2;
    }

    public static Vector3 GetAxisVector(int axis) {
        if (axis == 0) return Vector3.right;
        else if (axis == 1) return Vector3.up;
        else if (axis == 2) return Vector3.forward;
        else throw new ArgumentException($"Axis value {axis} is not valid: must be 0, 1 or 2.");
    }

    public static Vector3 GetDominantAxisVector(this Vector3 v) {
        return GetAxisVector(v.GetDominantAxis());
    }

    public static float GetAxisValue(this Vector3 v, int axis) {
        if (axis == 0) return v.x;
        else if (axis == 1) return v.y;
        else if (axis == 2) return v.z;
        else throw new ArgumentException($"Axis value {axis} is not valid: must be 0, 1 or 2.");
    }

    public static Vector3 SnapToDominantAxis(this Vector3 v) {
        v.Scale(v.GetDominantAxisVector());
        return v;
    }

    public static Vector3 SnapToAxis(this Vector3 v, int axis) {
        v.Scale(GetAxisVector(axis));
        return v;
    }

    public static Vector3 SnapToPlane(this Vector3 v, Vector3 planeNormal) {
        planeNormal.Normalize();
		return v - planeNormal * Vector3.Dot( planeNormal,v);
    }

    public static void SetAxisValue(this Vector3 v, int axis, float value) {
        if (axis == 0) v.x = value;
        else if (axis == 1) v.y = value;
        else if (axis == 2) v.z = value;
        else throw new ArgumentException($"Axis value {axis} is not valid: must be 0, 1 or 2.");
    }

	public static (float x, float y, float z) Decompose(this Vector3 eulerVector) {
        return (eulerVector.x, eulerVector.y, eulerVector.z);
    }

    public static Vector3 NormalizeEulerAngle(this Vector3 eulerVector) {
		return new Vector3(eulerVector.x.NormalizeDegreeAngle(), eulerVector.y.NormalizeDegreeAngle(), eulerVector.z.NormalizeDegreeAngle());
    }

    public static float NormalizeDegreeAngle(this float degrees) {
        if (degrees > 180) degrees -= 360f;
        return degrees;
    }

    public static Vector3 Inverse(this Vector3 v) {
        return new Vector3(1 / v.x, 1 / v.y, 1 / v.z);
	}

    public static float GetSignedDistanceFromInfiniteLine(this Vector2 p, Vector2 p1, Vector2 p2) {
        return Vector2.Dot((p2 - p1).Rotate90CW().normalized, p - p1);
    }

    public static float GetDistanceFromFiniteLine(this Vector2 p, Vector2 p1, Vector2 p2) {
        var l = Vector2.Dot((p2 - p1), p - p1) / (p2 - p1).sqrMagnitude;

        if (l < 0) return (p - p1).magnitude;
        if (l > 1) return (p - p2).magnitude;
        return Mathf.Abs(p.GetSignedDistanceFromInfiniteLine(p1, p2));
    }

    public static Vector2 GetClosestPointOnFiniteLine(this Vector2 p, Vector2 p1, Vector2 p2) {
        var l = Vector2.Dot((p2 - p1), p - p1) / (p2 - p1).sqrMagnitude;

        if (l < 0) return p1;
        if (l > 1) return p2;
        return Vector2.Lerp(p1, p2, l);
    }

    public static float GetInverseLerpOnFiniteLine(this Vector2 p, Vector2 p1, Vector2 p2) {
        var l = Vector2.Dot((p2 - p1), p - p1) / (p2 - p1).sqrMagnitude;

        return Mathf.Clamp01(l);
    }
    public static IEnumerable<Vector2Int> GetNeighbours(this Vector2Int p) {
        yield return p + Vector2Int.up;
        yield return p + Vector2Int.right;
        yield return p + Vector2Int.down;
        yield return p + Vector2Int.left;
    }
    public static IEnumerable<Vector2Int> GetNeighbours(this Vector2Int p, Vector2Int dir) {
        yield return p + dir;
        dir = dir.Rotate90CW();
        yield return p + dir;
        dir = dir.Rotate90CW();
        yield return p + dir;
        dir = dir.Rotate90CW();
        yield return p + dir;
    }
    // public static IEnumerable<Vector2Int> GetNeighbours(this Vector2Int p, GridDirection dir) {
    //     return GetNeighbours(p, dir.ToVector2Int());
    // }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ModEulerAroundZero(this Vector3 v) {
        v.x = Mathf.Repeat((v.x + 180), 360) - 180;
        v.y = Mathf.Repeat((v.y + 180), 360) - 180;
        v.z = Mathf.Repeat((v.z + 180), 360) - 180;
        return v;
    }

    public static Vector2 botLeft(this Vector2Int p) {
        return p + new Vector2(-0.5f, -0.5f);
    }
    public static Vector2 topLeft(this Vector2Int p) {
        return p + new Vector2(-0.5f, 0.5f);
    }
    public static Vector2 topRight(this Vector2Int p) {
        return p + new Vector2(0.5f, 0.5f);
    }
    public static Vector2 botRight(this Vector2Int p) {
        return p + new Vector2(0.5f, -0.5f);
    }
}