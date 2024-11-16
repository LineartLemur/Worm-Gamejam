using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;

namespace PepijnWillekens.ManagerSystem {
	public class Managers : MonoBehaviour {
		public class ManagerNotFoundException : Exception {
			public ManagerNotFoundException(Type t) : base($"Manager of type {t} not found.") {
			}

			public ManagerNotFoundException(Type type, Scene scene, object o) : base(
				$"failed to find manager of type {type} for scene {scene.name} for object {o}" ?? "null") {
			}
		}

		public bool dontDestroyOnLoad;

		public Dictionary<Type, MonoBehaviour> managers = new Dictionary<Type, MonoBehaviour>();
		private Dictionary<MonoBehaviour, bool> inited = new Dictionary<MonoBehaviour, bool>();


		private bool initedSelf;

		private void Init() {
			if(initedSelf) return;
			initedSelf = true;
			foreach (var addons in GetComponents<IInitable<Managers>>()) {
				addons.Init(this);
			}
		}
		public MonoBehaviour GetManager(Type type) {
			Init();
			Profiler.BeginSample("dictlookup");
			if (!managers.TryGetValue(type, out var man)) {
				Profiler.EndSample(); // dictlookup

				Profiler.BeginSample("find unknown manager");
				Profiler.BeginSample("get component");
				man = GetComponentInChildren(type, true) as MonoBehaviour;
				Profiler.EndSample(); // get component

				managers[type] = man;
				if (man == null) {
					return null;
				}

				if (!inited.ContainsKey(man)) {
					inited[man] = true;
					if (Application.isPlaying) {
						man.gameObject.SetActive(true);
					}

					if (man is IInitable) {
						Profiler.BeginSample("init unknown manager");
						try {
							((IInitable) man).Init();
						} catch (Exception e) {
							Debug.LogException(e, man);
						}

						Profiler.EndSample(); //init unknown manager
					}
				}

				Profiler.EndSample(); // find unknown manager
			} else {
				Profiler.EndSample(); // dictlookup
			}

			return man;
		}

		public T GetManager<T>() where T : MonoBehaviour {
			return GetManager(typeof(T)) as T;
		}

		public bool IsDoubleDDOLManager() {
			// Debug.Log($"{_instanceDDOL}  {_instanceDDOL != null}  {_instanceDDOL != this}", this);
			// Debug.Log($"checked", _instanceDDOL);
			return dontDestroyOnLoad && _instanceDDOL != null && _instanceDDOL != this;
		}

		private void Awake() {
			if (dontDestroyOnLoad) {
				if (IsDoubleDDOLManager()) {
					Destroy(gameObject);
				} else {
					_instanceDDOL = this;
					transform.SetParent(null);
					DontDestroyOnLoad(gameObject);
				}
			} else {
				if (_instances.ContainsKey(gameObject.scene.handle) && _instances[gameObject.scene.handle] != null &&
				    _instances[gameObject.scene.handle] != this) {
					Destroy(gameObject);
				} else {
					_instances[gameObject.scene.handle] = this;
				}
			}
		}

		private static Dictionary<int, Managers> _instances = new Dictionary<int, Managers>();
		private static Managers _instanceDDOL;


		private static Managers GetInstance(Scene scene) {
			if (!_instances.TryGetValue(scene.handle, out var s) || _instances[scene.handle] == null) {
				s = LookForManager(scene);
			}

			return s;
		}

		private static Managers LookForManager(Scene scene) {
			return _instances[scene.handle] = FindObjectsOfType<Managers>()
				.First((e) => !e.dontDestroyOnLoad && e.gameObject.scene == scene);
		}

		private static int _lastActiveScene = -1;

		public static Managers instanceDDOL {
			get {
				if (_instanceDDOL == null && (Time.timeSinceLevelLoad < 1 ||
				                              SceneManager.GetActiveScene().handle != _lastActiveScene)) {
					_lastActiveScene = SceneManager.GetActiveScene().handle;
					var m = FindObjectsOfType<Managers>();
					if (!m.Any(e => e.dontDestroyOnLoad)) return null;

					_instanceDDOL = m.First((e) => e.dontDestroyOnLoad);
					if (Application.isPlaying) {
						_instanceDDOL.transform.SetParent(null);
						DontDestroyOnLoad(_instanceDDOL.gameObject);
					}
				}

				return _instanceDDOL;
			}
		}

		public static T Get<T>() where T : MonoBehaviour {
			return Get(typeof(T)) as T;
		}

		public static T Get<T>(Scene scene, object o = null) where T : MonoBehaviour {
			return Get(typeof(T), scene, o) as T;
		}
		public static T TryGet<T>() where T : MonoBehaviour {
			try {
				return Get<T>();
			} catch (ManagerNotFoundException) {
			}

			return null;
		}

		public static T TryGet<T>(Scene scene, object o = null) where T : MonoBehaviour {
			try {
				return Get<T>(scene, o);
			} catch (ManagerNotFoundException) {
			}

			return null;
		}

		public static MonoBehaviour Get(Type type, Scene scene, object o = null) {
			Profiler.BeginSample("Get Manager");
			try {
				MonoBehaviour m = null;
				try {
					if (instanceDDOL != null) {
						Profiler.BeginSample("getM1");
						m = instanceDDOL.GetManager(type);
						Profiler.EndSample();
					}

					if (m == null) {
						Profiler.BeginSample("getM2");
						m = GetInstance(scene).GetManager(type);
						Profiler.EndSample();
					}
				} catch (InvalidOperationException) {
					Profiler.EndSample();
					Profiler.BeginSample("getM3");
					m = GetInstance(scene).GetManager(type);
					Profiler.EndSample();
				}

				Profiler.EndSample();

				if (m == null) {
					throw new ManagerNotFoundException(type);
				}

				return m;
			} catch (InvalidOperationException e) {
				Profiler.EndSample();
				throw new ManagerNotFoundException(type, scene, o);
			}
		}

		public static MonoBehaviour Get(Type type) {
			return Get(type, SceneManager.GetActiveScene());
		}
	}

	public static class M {
		public static T Get<T>() where T : MonoBehaviour {
			return Managers.Get<T>();
		}
		public static T Get<T>(Scene scene, object o = null) where T : MonoBehaviour {
			return Managers.Get<T>( scene, o );
		}
		public static T TryGet<T>() where T : MonoBehaviour {
			try {
				return Managers.TryGet<T>();
			} catch (Managers.ManagerNotFoundException e) {
				return null;
			}
		}
		public static T TryGet<T>(Scene scene, object o = null) where T : MonoBehaviour {
			try {
				return Managers.TryGet<T>( scene, o );
			} catch (Managers.ManagerNotFoundException e) {
				return null;
			}
		}
	}
}
