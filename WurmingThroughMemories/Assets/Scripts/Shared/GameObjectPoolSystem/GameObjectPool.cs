using System;
using System.Collections.Generic;
using UnityEngine;

namespace PepijnWillekens.Shared {
	/// <summary>
	/// maintains many unity GameObjects pools, based on prefabs.
	/// </summary>
	public class GameObjectPool : MonoBehaviour {
		Dictionary<GameObject, CachedList<GameObject>> pools = new Dictionary<GameObject, CachedList<GameObject>>();
		Dictionary<GameObject, GameObject> prefabLookupTable = new Dictionary<GameObject, GameObject>();

		Dictionary<GameObject, List<IResettable>>
			resetableLookupTable = new Dictionary<GameObject, List<IResettable>>();

		private List<MonoBehaviour> cachedMonoBehaviours = new List<MonoBehaviour>();

		private bool isApplicationQuitting;

		public T GetInstance<T>(T prefab) where T : Component {
			T instance = GetInstance(prefab.gameObject).GetComponent<T>();
			return instance;
		}

		public void EnsurePoolExists(GameObject prefab) {
			if (pools.ContainsKey(prefab)) return;
			Transform parent = new GameObject(prefab.name).transform;
			parent.SetParent(transform, false);
			pools[prefab] = new CachedList<GameObject>(
				newitem: () => {
					var newInstance = Instantiate(prefab, null, false);
					newInstance.SetActive(false);
					return newInstance;
				},
				enabler: (instance) => {
					instance.transform.SetParent(null, false);
					instance.SetActive(true);
				},
				disabler: (instance) => {
					instance.SetActive(false);
					if(!isApplicationQuitting && parent && instance) instance.transform.SetParent(parent, false);
				});
		}
		public GameObject GetInstance(GameObject prefab, bool reset = true) {
			EnsurePoolExists(prefab);

			GameObject result = pools[prefab].GetItem();
			if (!prefabLookupTable.ContainsKey(result)) {
				prefabLookupTable[result] = prefab;
			}

			if (reset) {
				ResetInstance(result);
			}

			return result;
		}

		public void ReleaseAllChildren(Transform transform) {
			for (int i = 0; i < transform.childCount; i++) {
				Release(transform.GetChild(i));
			}
		}
		public void Release<T>(List<T> list) where T : Component {
			foreach (var instance in list) {
				Release(instance);
			}
			list.Clear();
		}

		public void Release(List<GameObject> list) {
			foreach (var instance in list) {
				Release(instance);
			}
			list.Clear();
		}

		public void Release<T>(T instance) where T : Component {
			if(instance) Release(instance.gameObject);
		}

		public void Release(GameObject instance) {
			//ResetInstance(instance);
			if (!prefabLookupTable.ContainsKey(instance)) {
				//Debug.Log($"[{gameObject.name}] Could not find from which prefab {instance} was made.");
				Destroy(instance);
				return;
			}

			pools[prefabLookupTable[instance]].Release(instance);
		}

		private void ResetInstance(GameObject instance) {
			if (!resetableLookupTable.ContainsKey(instance)) {
				cachedMonoBehaviours.Clear();
				instance.GetComponents(cachedMonoBehaviours);
				List<IResettable> list = new List<IResettable>();
				for (int i = 0; i < cachedMonoBehaviours.Count; i++) {
					if (cachedMonoBehaviours[i] is IResettable) {
						list.Add(cachedMonoBehaviours[i] as IResettable);
					}
				}

				resetableLookupTable[instance] = list;
			}

			List<IResettable> resetables = resetableLookupTable[instance];
			for (int i = 0; i < resetables.Count; i++) {
				resetables[i].Reset();
			}
		}

		private void OnDestroy() {
			isApplicationQuitting = true;
		}

		private void OnApplicationQuit() {
			isApplicationQuitting = true;
		}

		public void GrowPoolTo(int n, GameObject prefab) {
			EnsurePoolExists(prefab);
			pools[prefab].GrowPoolTo(n);
		}
	}
}
