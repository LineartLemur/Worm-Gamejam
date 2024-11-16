using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Profiling;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class FPSDisplay : MonoBehaviour {
    const int SHORTLISTLENGTH = 10;

    public float updateRate = 2.0f; // 2 updates per sec.
    public DisplayMode displayMode;
    
    [ShowIf(nameof(UseTMP))]
    public TextMeshProUGUI displayText;

    private bool UseTMP() => displayMode == DisplayMode.TextMeshPro;

    int frameCount = 0;
    float dtAccumulator = 0.0f;
    float fps = 0.0f;

    float minFPS = 9999999;
    float maxFPS = 0;
    public string text { get; private set; }
    

    List<float> shortListFPS = new List<float>(SHORTLISTLENGTH);
    List<float> longListFPS = new List<float>();
    public const string EditorPrefKey = "DisableInGameFPS";

    private void Awake() {
#if RELEASE
		transform.parent.gameObject.SetActive(false);
#endif
#if UNITY_EDITOR
        if (EditorPrefs.HasKey(EditorPrefKey)) gameObject.SetActive(false);
#endif
        if(displayText) displayText.text = "";
    }

    public void Update() {
        Profiler.BeginSample("updateFPS");
        UpdateFPS();

        Profiler.EndSample();
#if (UNITY_EDITOR || ((UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX || UNITY_ANDROID || UNITY_XBOXONE) && (DEVELOPMENT_BUILD || TEST_FPS)))
        Profiler.BeginSample("updateText");
        UpdateText();
        Profiler.EndSample();
#endif
    }

    private void UpdateText() {
        if ( frameCount != 0) return;
        text = $"{GetCurrentFPS()} ({1000 / fps:0.0} ms)";
        if (displayText && UseTMP()) {
            displayText.text = text;
        }
    }

    private static Rect boxRect = new Rect(10, 10, 140, 40);
    private static GUIStyle labelStyle;
    private void OnGUI() {
        if(displayMode != DisplayMode.UGUI) return;

        if (labelStyle == null) {
            labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.alignment = TextAnchor.MiddleCenter;
            labelStyle.normal.textColor = Color.white;
            labelStyle.fontSize = 24;
            labelStyle.normal.background = MakeTex(2, 2, Color.black);
            labelStyle.wordWrap = false;
        }

        // GUI.Box(boxRect, GUIContent.none, GUI.skin.box);
        GUI.Box(boxRect, text, labelStyle);
    }
    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
    private void UpdateFPS() {
        float dt = Time.deltaTime;
        frameCount++;
        dtAccumulator += dt;
        if (dtAccumulator > 1.0f / updateRate) {
            fps = frameCount / dtAccumulator;
            frameCount = 0;
            dtAccumulator -= 1.0f / updateRate;
            maxFPS = Mathf.Max(maxFPS, fps);
            minFPS = Mathf.Min(minFPS, fps);

            shortListFPS.Add(fps);
            if (shortListFPS.Count >= SHORTLISTLENGTH) {
                longListFPS.Add(shortListFPS.Average());
                shortListFPS.Clear();
            }
        }
    }

    public int GetCurrentFPS() {
        return Mathf.RoundToInt(fps);
    }

    public int GetMinFPS() {
        return Mathf.RoundToInt(minFPS);
    }

    public int GetMaxFPS() {
        return Mathf.RoundToInt(maxFPS);
    }

    public int GetAverageFPS() {
        if (longListFPS.Count <= 0) {
            return 0;
        }

        return Mathf.RoundToInt(longListFPS.Average());
    }
    public enum DisplayMode {
        TextMeshPro, UGUI
    }
#if UNITY_EDITOR
    [InitializeOnLoad]
    public static class FPSDisplayMenu {
        private const string MENU_NAME = "Debug/Hide Ingame FPS";
 
        static FPSDisplayMenu() {
            EditorApplication.delayCall += () => {
                Menu.SetChecked( MENU_NAME,EditorPrefs.HasKey(FPSDisplay.EditorPrefKey));
            };
        }
 
        [MenuItem(MENU_NAME)]
        private static void ToggleAction() {
            var enabled = EditorPrefs.HasKey(EditorPrefKey);

            enabled = !enabled;

            if (enabled) {
                EditorPrefs.SetBool(EditorPrefKey, true);
            }else{
                EditorPrefs.DeleteKey(EditorPrefKey);
            }
            Menu.SetChecked( MENU_NAME, enabled);
        }
    }
    #endif
}