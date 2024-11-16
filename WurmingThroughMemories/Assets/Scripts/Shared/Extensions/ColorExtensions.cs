using UnityEngine;

public static class ColorExtensions
{
    public static float GetHue(this Color color)
    {
        float H, S, V;
        Color.RGBToHSV(color, out H, out S, out V);
        return H;
    }

    public static float GetSaturation(this Color color)
    {
        float H, S, V;
        Color.RGBToHSV(color, out H, out S, out V);
        return S;
    }

    public static float GetBrightness(this Color color)
    {
        float H, S, V;
        Color.RGBToHSV(color, out H, out S, out V);
        return V;
    }

    public static Color ChangeHue(this Color color, float H)
    {
        return Color.HSVToRGB(H, color.GetSaturation(), color.GetBrightness());
    }

    public static Color ChangeSaturation(this Color color, float S)
    {
        return Color.HSVToRGB(color.GetHue(), S, color.GetBrightness());
    }

    public static Color ChangeBrightness(this Color color, float V)
    {
        return Color.HSVToRGB(color.GetHue(), color.GetSaturation(), V);
    }

    public static Color ChangeRed(this Color color, float r)
    {
        return new Color(r, color.g, color.b, color.a);
    }

    public static Color ChangeGreen(this Color color, float g)
    {
        return new Color(color.r, g, color.b, color.a);
    }

    public static Color ChangeBlue(this Color color, float b)
    {
        return new Color(color.r, color.g, b, color.a);
    }

    public static Color ChangeAlpha(this Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }

    public static Color MultiplyRed(this Color color, float r)
    {
        return new Color(color.r * r, color.g, color.b, color.a);
    }

    public static Color MultiplyGreen(this Color color, float g)
    {
        return new Color(color.r, color.g * g, color.b, color.a);
    }

    public static Color MultiplyBlue(this Color color, float b)
    {
        return new Color(color.r, color.g, color.b * b, color.a);
    }

    public static Color MultiplyAlpha(this Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, color.a * alpha);
    }

    public static int R255(this Color color) {
        return Mathf.RoundToInt(color.r * 255);
    }
    public static int G255(this Color color) {
        return Mathf.RoundToInt(color.g * 255);
    }
    public static int B255(this Color color) {
        return Mathf.RoundToInt(color.b * 255);
    }
    public static int A255(this Color color) {
        return Mathf.RoundToInt(color.a * 255);
    }
    
    public static float GetActualBrightness(this Color color)
    {
        
        return color.r * 0.21f + color.g * 0.72f + color.b * 0.07f;
    }
}