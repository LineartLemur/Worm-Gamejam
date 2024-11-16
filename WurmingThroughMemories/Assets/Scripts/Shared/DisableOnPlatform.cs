using UnityEngine;

public class DisableOnPlatform : MonoBehaviour {
	public bool unityEditor;
	public bool xbox;
	public bool nintendoSwitch;
	public bool pc;

	private void Awake() {
		if (shouldDisable) {
			gameObject.SetActive(false);
		}
	}

	private bool shouldDisable {
		get {
#if UNITY_EDITOR
			return unityEditor;
#elif UNITY_XBOXONE
            return xbox;
#elif UNITY_SWITCH
            return nintendoSwitch;
#else
            return pc;
#endif
		}
	}
}