/*
 * EasingCore (https://github.com/setchi/EasingCore)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/EasingCore/blob/master/LICENSE)
 */

using System;
using UnityEngine;


public enum Ease {
    Linear,
    InBack,
    InBounce,
    InCirc,
    InCubic,
    InElastic,
    InExpo,
    InQuad,
    InQuart,
    InQuint,
    InSine,
    OutBack,
    OutBounce,
    OutCirc,
    OutCubic,
    OutElastic,
    OutExpo,
    OutQuad,
    OutQuart,
    OutQuint,
    OutSine,
    InOutBack,
    InOutBounce,
    InOutCirc,
    InOutCubic,
    InOutElastic,
    InOutExpo,
    InOutQuad,
    InOutQuart,
    InOutQuint,
    InOutSine,
}

public delegate float EasingFunction(float t);

public static class Easing {
    public static bool IsIn(this Ease type) {
        switch (type) {
            case Ease.InBack:
            case Ease.InBounce:
            case Ease.InCirc:
            case Ease.InCubic:
            case Ease.InElastic:
            case Ease.InExpo:
            case Ease.InQuad:
            case Ease.InQuart:
            case Ease.InQuint:
            case Ease.InSine:
            case Ease.InOutBack:
            case Ease.InOutBounce:
            case Ease.InOutCirc:
            case Ease.InOutCubic:
            case Ease.InOutElastic:
            case Ease.InOutExpo:
            case Ease.InOutQuad:
            case Ease.InOutQuart:
            case Ease.InOutQuint:
            case Ease.InOutSine: return true;
            default: return false;
        }
    }

    public static bool IsOut(this Ease type) {
        switch (type) {
            case Ease.OutBack:
            case Ease.OutBounce:
            case Ease.OutCirc:
            case Ease.OutCubic:
            case Ease.OutElastic:
            case Ease.OutExpo:
            case Ease.OutQuad:
            case Ease.OutQuart:
            case Ease.OutQuint:
            case Ease.OutSine:
            case Ease.InOutBack:
            case Ease.InOutBounce:
            case Ease.InOutCirc:
            case Ease.InOutCubic:
            case Ease.InOutElastic:
            case Ease.InOutExpo:
            case Ease.InOutQuad:
            case Ease.InOutQuart:
            case Ease.InOutQuint:
            case Ease.InOutSine: return true;
            default: return false;
        }
    }

    public static Ease ToOut(this Ease type) {
        switch (type) {
            case Ease.InBack:
            case Ease.OutBack:
            case Ease.InOutBack: return Ease.OutBack;
            case Ease.InBounce:
            case Ease.OutBounce:
            case Ease.InOutBounce: return Ease.OutBounce;
            case Ease.InCirc:
            case Ease.OutCirc:
            case Ease.InOutCirc: return Ease.OutCirc;
            case Ease.InCubic:
            case Ease.OutCubic:
            case Ease.InOutCubic: return Ease.OutCubic;
            case Ease.InElastic:
            case Ease.OutElastic:
            case Ease.InOutElastic: return Ease.OutElastic;
            case Ease.InExpo:
            case Ease.OutExpo:
            case Ease.InOutExpo: return Ease.OutExpo;
            case Ease.InQuad:
            case Ease.OutQuad:
            case Ease.InOutQuad: return Ease.OutQuad;
            case Ease.InQuart:
            case Ease.OutQuart:
            case Ease.InOutQuart: return Ease.OutQuart;
            case Ease.InQuint:
            case Ease.OutQuint:
            case Ease.InOutQuint: return Ease.OutQuint;
            case Ease.InSine:
            case Ease.OutSine:
            case Ease.InOutSine: return Ease.OutSine;
            default: return type;
        }
    }

    public static Ease ToIn(this Ease type) {
        switch (type) {
            case Ease.InBack:
            case Ease.OutBack:
            case Ease.InOutBack: return Ease.InBack;
            case Ease.InBounce:
            case Ease.OutBounce:
            case Ease.InOutBounce: return Ease.InBounce;
            case Ease.InCirc:
            case Ease.OutCirc:
            case Ease.InOutCirc: return Ease.InCirc;
            case Ease.InCubic:
            case Ease.OutCubic:
            case Ease.InOutCubic: return Ease.InCubic;
            case Ease.InElastic:
            case Ease.OutElastic:
            case Ease.InOutElastic: return Ease.InElastic;
            case Ease.InExpo:
            case Ease.OutExpo:
            case Ease.InOutExpo: return Ease.InExpo;
            case Ease.InQuad:
            case Ease.OutQuad:
            case Ease.InOutQuad: return Ease.InQuad;
            case Ease.InQuart:
            case Ease.OutQuart:
            case Ease.InOutQuart: return Ease.InQuart;
            case Ease.InQuint:
            case Ease.OutQuint:
            case Ease.InOutQuint: return Ease.InQuint;
            case Ease.InSine:
            case Ease.OutSine:
            case Ease.InOutSine: return Ease.InSine;
            default: return type;
        }
    }

    public static Ease ToInOut(this Ease type) {
        switch (type) {
            case Ease.InBack:
            case Ease.OutBack:
            case Ease.InOutBack: return Ease.InOutBack;
            case Ease.InBounce:
            case Ease.OutBounce:
            case Ease.InOutBounce: return Ease.InOutBounce;
            case Ease.InCirc:
            case Ease.OutCirc:
            case Ease.InOutCirc: return Ease.InOutCirc;
            case Ease.InCubic:
            case Ease.OutCubic:
            case Ease.InOutCubic: return Ease.InOutCubic;
            case Ease.InElastic:
            case Ease.OutElastic:
            case Ease.InOutElastic: return Ease.InOutElastic;
            case Ease.InExpo:
            case Ease.OutExpo:
            case Ease.InOutExpo: return Ease.InOutExpo;
            case Ease.InQuad:
            case Ease.OutQuad:
            case Ease.InOutQuad: return Ease.InOutQuad;
            case Ease.InQuart:
            case Ease.OutQuart:
            case Ease.InOutQuart: return Ease.InOutQuart;
            case Ease.InQuint:
            case Ease.OutQuint:
            case Ease.InOutQuint: return Ease.InOutQuint;
            case Ease.InSine:
            case Ease.OutSine:
            case Ease.InOutSine: return Ease.InOutSine;
            default: return type;
        }
    }

    /// <summary>
    /// Gets the easing function
    /// </summary>
    /// <param name="type">Ease type</param>
    /// <returns>Easing function</returns>
    public static EasingFunction Get(Ease type) {
        switch (type) {
            case Ease.Linear: return linear;
            case Ease.InBack: return inBack;
            case Ease.InBounce: return inBounce;
            case Ease.InCirc: return inCirc;
            case Ease.InCubic: return inCubic;
            case Ease.InElastic: return inElastic;
            case Ease.InExpo: return inExpo;
            case Ease.InQuad: return inQuad;
            case Ease.InQuart: return inQuart;
            case Ease.InQuint: return inQuint;
            case Ease.InSine: return inSine;
            case Ease.OutBack: return outBack;
            case Ease.OutBounce: return outBounce;
            case Ease.OutCirc: return outCirc;
            case Ease.OutCubic: return outCubic;
            case Ease.OutElastic: return outElastic;
            case Ease.OutExpo: return outExpo;
            case Ease.OutQuad: return outQuad;
            case Ease.OutQuart: return outQuart;
            case Ease.OutQuint: return outQuint;
            case Ease.OutSine: return outSine;
            case Ease.InOutBack: return inOutBack;
            case Ease.InOutBounce: return inOutBounce;
            case Ease.InOutCirc: return inOutCirc;
            case Ease.InOutCubic: return inOutCubic;
            case Ease.InOutElastic: return inOutElastic;
            case Ease.InOutExpo: return inOutExpo;
            case Ease.InOutQuad: return inOutQuad;
            case Ease.InOutQuart: return inOutQuart;
            case Ease.InOutQuint: return inOutQuint;
            case Ease.InOutSine: return inOutSine;
            default: return linear;
        }
    }

    public static float Do(Ease type, float t) { //exists to avoid garbage

        switch (type) {
            case Ease.Linear: return linear(t);
            case Ease.InBack: return inBack(t);
            case Ease.InBounce: return inBounce(t);
            case Ease.InCirc: return inCirc(t);
            case Ease.InCubic: return inCubic(t);
            case Ease.InElastic: return inElastic(t);
            case Ease.InExpo: return inExpo(t);
            case Ease.InQuad: return inQuad(t);
            case Ease.InQuart: return inQuart(t);
            case Ease.InQuint: return inQuint(t);
            case Ease.InSine: return inSine(t);
            case Ease.OutBack: return outBack(t);
            case Ease.OutBounce: return outBounce(t);
            case Ease.OutCirc: return outCirc(t);
            case Ease.OutCubic: return outCubic(t);
            case Ease.OutElastic: return outElastic(t);
            case Ease.OutExpo: return outExpo(t);
            case Ease.OutQuad: return outQuad(t);
            case Ease.OutQuart: return outQuart(t);
            case Ease.OutQuint: return outQuint(t);
            case Ease.OutSine: return outSine(t);
            case Ease.InOutBack: return inOutBack(t);
            case Ease.InOutBounce: return inOutBounce(t);
            case Ease.InOutCirc: return inOutCirc(t);
            case Ease.InOutCubic: return inOutCubic(t);
            case Ease.InOutElastic: return inOutElastic(t);
            case Ease.InOutExpo: return inOutExpo(t);
            case Ease.InOutQuad: return inOutQuad(t);
            case Ease.InOutQuart: return inOutQuart(t);
            case Ease.InOutQuint: return inOutQuint(t);
            case Ease.InOutSine: return inOutSine(t);
            default: return linear(t);
        }
    }

    static float linear(float t) => t;

    static float inBack(float t) {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1;

        return c3 * t * t * t - c1 * t * t;
        // return t * t * t - t * Mathf.Sin(t * Mathf.PI);
    }

    static float outBack(float t) => 1f - inBack(1f - t);

    static float inOutBack(float t) =>
        t < 0.5f
            ? 0.5f * inBack(2f * t)
            : 0.5f * outBack(2f * t - 1f) + 0.5f;

    static float inBounce(float t) => 1f - outBounce(1f - t);

    static float outBounce(float t) =>
        t < 4f / 11.0f ? (121f * t * t) / 16.0f :
        t < 8f / 11.0f ? (363f / 40.0f * t * t) - (99f / 10.0f * t) + 17f / 5.0f :
        t < 9f / 10.0f ? (4356f / 361.0f * t * t) - (35442f / 1805.0f * t) + 16061f / 1805.0f :
        (54f / 5.0f * t * t) - (513f / 25.0f * t) + 268f / 25.0f;

    static float inOutBounce(float t) =>
        t < 0.5f
            ? 0.5f * inBounce(2f * t)
            : 0.5f * outBounce(2f * t - 1f) + 0.5f;

    static float inCirc(float t) => 1f - Mathf.Sqrt(1f - (t * t));

    static float outCirc(float t) => Mathf.Sqrt((2f - t) * t);

    static float inOutCirc(float t) =>
        t < 0.5f
            ? 0.5f * (1 - Mathf.Sqrt(1f - 4f * (t * t)))
            : 0.5f * (Mathf.Sqrt(-((2f * t) - 3f) * ((2f * t) - 1f)) + 1f);

    static float inCubic(float t) => t * t * t;

    static float outCubic(float t) => inCubic(t - 1f) + 1f;

    static float inOutCubic(float t) =>
        t < 0.5f
            ? 4f * t * t * t
            : 0.5f * inCubic(2f * t - 2f) + 1f;

    static float inElastic(float t) {
        const float c4 = (Mathf.PI) / 3;

        return t <= 0
            ? 0
            : t >= 1
                ? 1
                : -Mathf.Pow(2, 10 * t - 10) * Mathf.Sin((t * 10f - 10.75f) * c4);
        // return Mathf.Sin(13f * (Mathf.PI * 0.5f) * t) * Mathf.Pow(2f, 10f * (t - 1f));
    }

    static float outElastic(float t) {
        const float c4 = (Mathf.PI) / 3;

        return t <= 0
            ? 0
            : t >= 1
                ? 1
                : Mathf.Pow(2, -10 * t) * Mathf.Sin((t * 10 - 0.75f) * c4) + 1;
        // return Mathf.Sin(-13f * (Mathf.PI * 0.5f) * (t + 1)) * Mathf.Pow(2f, -10f * t) + 1f;
    }

    static float inOutElastic(float x) {
        const float c5 = (Mathf.PI) / 4.5f;

        return x <= 0
            ? 0
            : x >= 1
                ? 1
                : x < 0.5
                    ? -(Mathf.Pow(2, 20 * x - 10) * Mathf.Sin((20 * x - 11.125f) * c5)) / 2
                    : (Mathf.Pow(2, -20 * x + 10) * Mathf.Sin((20 * x - 11.125f) * c5)) / 2 + 1;
        // return t < 0.5f
        //  ? 0.5f * Mathf.Sin(13f * (Mathf.PI * 0.5f) * (2f * t)) * Mathf.Pow(2f, 10f * ((2f * t) - 1f))
        //  : 0.5f * (Mathf.Sin(-13f * (Mathf.PI * 0.5f) * ((2f * t - 1f) + 1f)) *
        //   Mathf.Pow(2f, -10f * (2f * t - 1f)) + 2f);
    }

    static float inExpo(float t) => Mathf.Approximately(0.0f, t) ? t : Mathf.Pow(2f, 10f * (t - 1f));

    static float outExpo(float t) => Mathf.Approximately(1.0f, t) ? t : 1f - Mathf.Pow(2f, -10f * t);

    static float inOutExpo(float v) =>
        Mathf.Approximately(0.0f, v) || Mathf.Approximately(1.0f, v)
            ? v
            : v < 0.5f
                ? 0.5f * Mathf.Pow(2f, (20f * v) - 10f)
                : -0.5f * Mathf.Pow(2f, (-20f * v) + 10f) + 1f;

    static float inQuad(float t) => t * t;

    static float outQuad(float t) => -t * (t - 2f);

    static float inOutQuad(float t) =>
        t < 0.5f
            ? 2f * t * t
            : -2f * t * t + 4f * t - 1f;

    static float inQuart(float t) => t * t * t * t;

    static float outQuart(float t) {
        var u = t - 1f;
        return u * u * u * (1f - t) + 1f;
    }

    static float inOutQuart(float t) =>
        t < 0.5f
            ? 8f * inQuart(t)
            : -8f * inQuart(t - 1f) + 1f;

    static float inQuint(float t) => t * t * t * t * t;

    static float outQuint(float t) => inQuint(t - 1f) + 1f;

    static float inOutQuint(float t) =>
        t < 0.5f
            ? 16f * inQuint(t)
            : 0.5f * inQuint(2f * t - 2f) + 1f;

    static float inSine(float t) => Mathf.Sin((t - 1f) * (Mathf.PI * 0.5f)) + 1f;

    static float outSine(float t) => Mathf.Sin(t * (Mathf.PI * 0.5f));

    static float inOutSine(float t) => 0.5f * (1f - Mathf.Cos(t * Mathf.PI));
}