using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Utility {
	public static class EditorExtension {
		public static T FindAndLoadAsset<T>(string query) where  T: Object{
#if UNITY_EDITOR
			var a = AssetDatabase.FindAssets($"t:{typeof(T).Name} {query}");
			if (a.Length <= 0) return default;
			Debug.Log(query + "");
			return AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GUIDToAssetPath(a[0])).First(e => e is T t && t.name.Contains(query)) as T;
			#else
			return default;
#endif
		}
	}
}
