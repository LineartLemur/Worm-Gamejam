using UnityEngine;

public static class FloatExtensions {
    public static bool IsEqualTo(this float target, float other) {
        return Mathf.Abs(target - other) <= Mathf.Epsilon;
    }

    public static bool IsNotEqualTo(this float target, float other) {
        return Mathf.Abs(target - other) > Mathf.Epsilon;
    }

    public static int Salt(this int target, int salt) {
        unchecked {
            return (target * 397) ^ (salt);
        }
    }
    
    public static float DTToLerpT(this float dt, float lerpLambda) {
        return 1f - Mathf.Exp(-lerpLambda * dt);
    }
}
