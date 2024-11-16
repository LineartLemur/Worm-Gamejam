using System;
using System.Collections;
using System.Collections.Generic;

public class AutoGeneratingDictionary<TKey, TValue> : IDictionary<TKey, TValue> {

	private readonly IDictionary<TKey, TValue> dictionary;
	private readonly Func<TKey, TValue> generator;

	public AutoGeneratingDictionary(Func<TKey, TValue> generator) {
		this.dictionary = new Dictionary<TKey, TValue>();
		this.generator = generator;
	}

	public AutoGeneratingDictionary(IDictionary<TKey, TValue>  dictionary, Func<TKey, TValue> generator) {
		this.dictionary = dictionary;
		this.generator = generator;
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
		value = GetValue(key);
		return true;
	}

	public TValue GetValue(TKey key) {
		TValue value;
		if (!dictionary.TryGetValue(key, out value)) {
			value = generator(key);
			dictionary[key] = value;
		}

		return value;
	}

	public TValue this[TKey key] {
		get {
			return GetValue(key);
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