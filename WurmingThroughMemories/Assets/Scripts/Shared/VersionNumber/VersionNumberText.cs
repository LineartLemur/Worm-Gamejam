using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class VersionNumberText :MonoBehaviour {
        public TextAsset textAsset;
        private void Awake() {
            GetComponent<TMP_Text>().text = textAsset.text;
        }
        #if UNITY_EDITOR
        private void OnValidate() {
            textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/version.txt");
        }
        #endif
    }