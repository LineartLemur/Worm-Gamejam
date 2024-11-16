using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;
using PepijnWillekens.ManagerSystem;
using UnityEngine.SceneManagement;

public static class DependancyCache {

	private static Dictionary<Type, ServiceTypeInfo> servicesInfo = new Dictionary<Type, ServiceTypeInfo>();

	static DependancyCache() {
		var autoinitclasses = AppDomain.CurrentDomain.GetAssemblies()
		         .SelectMany(assembly => assembly.GetTypes())
		         .Where(type => type.IsSubclassOf(typeof(AutoInit)));

		foreach (var type in autoinitclasses) {
			AddInfo(type);
		}
	}

	public static ServiceTypeInfo GetServiceFields(Type type) {
		if (!servicesInfo.ContainsKey(type)) AddInfo(type);
		return servicesInfo[type];
    }
	private static void AddInfo(Type type) {
		Type originalType = type;

		List<FieldInfo> fieldsToFill = new List<FieldInfo>();

		do {
			fieldsToFill.AddRange(type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
				.Where(e => Attribute.IsDefined(e, typeof(InjectAttribute))/* && typeof(Service).IsAssignableFrom(e.FieldType)*/));
			type = type.BaseType;
		} while (type != null);

		List<FieldInfo> fieldsToTryFill = new List<FieldInfo>();

		type = originalType;
		do {
			fieldsToTryFill.AddRange(type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
				.Where(e => Attribute.IsDefined(e, typeof(TryInjectAttribute))/* && typeof(Service).IsAssignableFrom(e.FieldType)*/));
			type = type.BaseType;
		} while (type != null);

		servicesInfo[originalType] = new ServiceTypeInfo() {
			injectFields = fieldsToFill,
			tryInjectFields = fieldsToTryFill,
		};
	}

	public static void InjectDependencies(object o) {
		InjectDependencies(o, SceneManager.GetActiveScene());
	}

	public static void InjectDependencies(object o, Scene scene) {
		UnityEngine.Profiling.Profiler.BeginSample("InjectDependencies");
		ServiceTypeInfo typeInfo = DependancyCache.GetServiceFields(o.GetType());
		List<FieldInfo> fieldsToFill = typeInfo.injectFields;

		for (int i = 0; i < fieldsToFill.Count; i++) {
			fieldsToFill[i].SetValue(o, Managers.Get(fieldsToFill[i].FieldType, scene, o));
		}

		List<FieldInfo> fieldsToTryFill = typeInfo.tryInjectFields;

		for (int i = 0; i < fieldsToTryFill.Count; i++) {
			try {
				fieldsToTryFill[i].SetValue(o, Managers.Get(fieldsToTryFill[i].FieldType, scene,o));
			} catch (Managers.ManagerNotFoundException _) {

			}
		}

		UnityEngine.Profiling.Profiler.EndSample();
	}
}
