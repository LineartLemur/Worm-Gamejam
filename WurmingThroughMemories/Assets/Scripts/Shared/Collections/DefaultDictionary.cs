using System;
using System.Collections;
using System.Collections.Generic;

public class DefaultDictionary<TKey, TValue> : IDictionary<TKey, TValue> {

	private readonly IDictionary<TKey, TValue> dictionary;
	private readonly TValue defaultValue;

	public DefaultDictionary(TValue defaultValue) {
		this.dictionary = new Dictionary<TKey, TValue>();
		this.defaultValue = defaultValue;
	}
	public DefaultDictionary() {
		this.dictionary = new Dictionary<TKey, TValue>();
		this.defaultValue = default;
	}

	public DefaultDictionary(IDictionary<TKey, TValue>  dictionary, TValue defaultValue) {
		this.dictionary = dictionary;
		this.defaultValue = defaultValue;
	}

	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
		return dictionary.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator() {
		return GetEnumerator();
	}

	public void Add(KeyValuePair<TKey, TValue> item) {
		dictionary.Add(item);
	}

	public void Clear() {
		dictionary.Clear();
	}

	public bool Contains(KeyValuePair<TKey, TValue> item) {
		return dictionary.Contains(item);
	}

	public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
		dictionary.CopyTo(array, arrayIndex);
	}

	public bool Remove(KeyValuePair<TKey, TValue> item) {
		return dictionary.Remove(item);
	}

	public int Count {
		get { return dictionary.Count; }
	}

	public bool IsReadOnly {
		get { return dictionary.IsReadOnly; }
	}

	public bool ContainsKey(TKey key) {
		return dictionary.ContainsKey(key);
	}

	public void Add(TKey key, TValue value) {
		dictionary.Add(key, value);
	}

	public bool Remove(TKey key) {
		return dictionary.Remove(key);
	}

	public bool TryGetValue(TKey key, out TValue value) {
		if (!dictionary.TryGetValue(key, out value)) {
			value = defaultValue;
		}

		return true;
	}

	public TValue this[TKey key] {
		get {
			TValue value;
			bool found = TryGetValue(key, out value);
			if (!found) value = defaultValue;
			return value;
		}
		set {
			dictionary[key] = value;
		}
	}

	public ICollection<TKey> Keys {
		get {
			return dictionary.Keys;
		}
	}

	public ICollection<TValue> Values {
		get {
			return dictionary.Values;
		}
	}
}

public static class DefaultableDictionaryExtensions {
	public static IDictionary<TKey, TValue> WithDefaultValue<TValue, TKey>(this IDictionary<TKey, TValue> dictionary, TValue defaultValue) {
		return new DefaultDictionary<TKey, TValue>(dictionary, defaultValue);
	}

	public static T[] SubArray<T>(this T[] data, int index, int length) {
		T[] result = new T[length];
		Array.Copy(data, index, result, 0, length);
		return result;
	}
}