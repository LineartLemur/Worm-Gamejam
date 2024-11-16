using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;

public static class MonoBehaviourExtension {

	public static T AddMissingComponent<T>(this GameObject gameObject) where T : Component {
		T comp = gameObject.GetComponent<T>();
		if (comp == null) comp = gameObject.AddComponent<T>();
		return comp;
	}
	public static T GetComponentInParent<T>(this Component component, bool inactiveObjects) {
		if (!inactiveObjects) return component.GetComponentInParent<T>();
		else {
			T comp = default(T);
			Transform parent = component.transform;
			while (parent != null) {
				comp = parent.GetComponent<T>();
				if (comp != null) return comp;
				parent = parent.parent;
			}
			return comp;
		}
	}

	public static void SetParentLocal(this Component component, Transform parent) {
		Transform transform = component.transform;
		RectTransform rectTransform = transform as RectTransform;
		Vector2 offsetMin = Vector2.zero, offsetMax = Vector2.zero;
		bool hasRectTransform = (rectTransform != null);
		if (hasRectTransform) {
			offsetMin = rectTransform.offsetMin;
			offsetMax = rectTransform.offsetMax;
		}
		Vector3 pos = transform.localPosition;
		Vector3 scale = transform.localScale;
		Quaternion rotation = transform.localRotation;
		transform.SetParent(parent);
		transform.localPosition = pos;
		transform.localScale = scale;
		transform.localRotation = rotation;
		if (hasRectTransform) {
			rectTransform.offsetMin = offsetMin;
			rectTransform.offsetMax = offsetMax;
		}
	}
	public static void DestroyAllChildren(this Transform transform) {
		if (Application.isPlaying) {
			for (int i = 0; i < transform.childCount; i++) {
				GameObject.Destroy(transform.GetChild(i).gameObject);
			}
		} else {
			while (transform.childCount > 0) {
				GameObject.DestroyImmediate(transform.GetChild(0).gameObject);
			}
		}
	}
	public static void DestroySafe(this Object go) {
		if (Application.isPlaying) {
			Object.Destroy(go);
		} else {
			Object.DestroyImmediate(go);
		}
	}

	public static void Reset(this Transform transform) {
		transform.localPosition = Vector3.zero;
		transform.localScale = Vector3.one;
		transform.localRotation = Quaternion.identity;
	}

	public static void SetLayerRecusive(this GameObject gameObject, int layer) {
		gameObject.layer = layer;
		for (int i = 0; i < gameObject.transform.childCount; i++) {
			gameObject.transform.GetChild(i).gameObject.SetLayerRecusive(layer);
		}
	}


	public static Coroutine RunWhen(this MonoBehaviour behaviour, System.Func<bool> condition, System.Action action) {
		return behaviour.StartCoroutine(RunWhenRoutine(condition, action));
	}

	private static IEnumerator RunWhenRoutine( System.Func<bool> condition, System.Action action) {
		yield return new WaitUntil(condition);
		action();
	}
	public static Coroutine RunNextFrame(this MonoBehaviour behaviour, System.Action action) {
		return behaviour.StartCoroutine(RunNextFrameRoutine( 1,action));
	}
	public static Coroutine RunAfterFrames(this MonoBehaviour behaviour,int nframes, System.Action action) {
		return behaviour.StartCoroutine(RunNextFrameRoutine(nframes, action));
	}

	private static IEnumerator RunNextFrameRoutine(int nframes, System.Action action) {
		yield return null;
		for (int i = 1; i < nframes; i++)
		{
			yield return null;
		}
		action();
	}
	public static Coroutine RunDelayed(this MonoBehaviour behaviour, float delay, System.Action action) {
		return behaviour.StartCoroutine(RunDelayedRoutine(delay, action));
	}

	private static IEnumerator RunDelayedRoutine(float delay, System.Action action) {
		yield return new WaitForSeconds(delay);
		action();
	}
	public static Coroutine RunDelayedUnscaled(this MonoBehaviour behaviour, float delay, System.Action action) {
		return behaviour.StartCoroutine(RunDelayedUnscaledRoutine(delay, action));
	}

	private static IEnumerator RunDelayedUnscaledRoutine(float delay, System.Action action) {
		yield return new WaitForSecondsRealtime(delay);
		action();
	}

	public static void SetEnabled<T>(this IEnumerable<T> components, bool isEnabled) where T : Behaviour {
		foreach (var component in components) {
			component.enabled = isEnabled;
		}
	}
	
	public static void SetEnabled(this  IEnumerable<Collider> components, bool isEnabled)  {
		foreach (var component in components) {
			component.enabled = isEnabled;
		}
	}
}
