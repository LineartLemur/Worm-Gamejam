using System;
using System.Collections.Generic;
using System.Reflection;

public static class EnumLists {
	private static MethodInfo makeEnumListInfo;

	static EnumLists() {
		makeEnumListInfo = typeof(EnumLists).GetMethod("MakeEnumList", BindingFlags.Public | BindingFlags.Static);
	}

	private static AutoGeneratingDictionary<Type, object> enumLists = new AutoGeneratingDictionary<Type, object>(
		(type) => {
			makeEnumListInfo = typeof(EnumLists).GetMethod("MakeEnumList", BindingFlags.Public | BindingFlags.Static);
			MethodInfo genericMethod = makeEnumListInfo.MakeGenericMethod(new[] {type});
			return genericMethod.Invoke(null, null);
		});

	public static List<T> GetAllValues<T>() {
		if(enumLists == null) return MakeEnumList<T>(); //removing this line can cause compilation issues on AOT platforms
		return enumLists[typeof(T)] as List<T>;
	}
	public static T ToEnum<T>(this string str) {
		var values = GetAllValues<T>();
		for (int i = 0; i < values.Count; i++) {
			if (str.ToUpper() == values[i].ToString().ToUpper()) return values[i];
		}
		throw new KeyNotFoundException();
	}
	public static bool TryToEnum<T>(this string str, out T result) {
		try {
			result = str.ToEnum<T>();
			return true;
		} catch (KeyNotFoundException e) {
			result = default;
			return false;
		}
	}

	public static List<T> MakeEnumList<T>() {
		List<T> list = new List<T>();
		foreach (T item in Enum.GetValues(typeof(T))) {
			list.Add(item);
		}

		return list;
	}
}
