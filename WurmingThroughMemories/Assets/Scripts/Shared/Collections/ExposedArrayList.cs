using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoaBonanza.Core
{
	public class ExposedArrayList<T> : IList<T> {

		private T[] arr;

		private int count = 0;


		public ExposedArrayList() : this(8) {
		}

		public ExposedArrayList(int initialSize) {
			arr = new T[initialSize];
		}

		public T this[int index] {
			get {
				if (index >= count) return default(T);
				return arr[index];
			}

			set {
				ExpandTo(index);
				arr[index] = value;
				if (count <= index) count = index+1;
			}
		}

		public int Capacity {
			set {
				if (value > arr.Length) Resize(value);
				else if (value < arr.Length) Resize(value);
			}
			get {
				return arr.Length;
			}
		}

		public void ExpandTo(int index) {
			int n = Mathf.Max(1, arr.Length);
			while (index >= n) {
				n *= 2;
			}
			if (n != arr.Length) Resize(n);
		}

		public void Resize(int newSize) {
			T[] newArr = new T[newSize];
			for (int i = 0; i < count; ++i) {
				newArr[i] = arr[i];
			}
			arr = newArr;
			if (count > arr.Length) count = arr.Length;
		}

		public int Count {
			get {
				return count;
			}
		}

		public bool IsReadOnly {
			get {
				return false;
			}
		}

		public void Add(T item) {
			ExpandTo(count);
			arr[count] = item;
			++count;
		}

		public void AddRange(IList<T> range) {
			ExpandTo(count+range.Count);
			for (int i = 0; i < range.Count; ++i) {
				Add(range[i]);
			}
			count += range.Count;
		}
		public void AddRange(T[] range) {
			ExpandTo(count+range.Length);
			for (int i = 0; i < range.Length; ++i) {
				Add(range[i]);
			}
			count += range.Length;
		}

		public void Clear() {
			count = 0;
		}

		public bool Contains(T item) {
			for (int i = 0; i < count; ++i) {
				if (arr[i].Equals(item)) return true;
			}
			return false;
		}
	
		public int IndexOf(T item) {
			for (int i = 0; i < count; ++i) {
				if (arr[i].Equals(item)) return i;
			}
			return -1;
		}

		public T[] ToArray() {
			return arr;
		}

		public void AppendToArray(IList<T> arr, int offset) {
			for (int i = 0; i < Count; ++i) {
				arr[offset+i] = this.arr[i];
			}
		}

		public void Insert(int index, T item) {
			for (int i = index; i < count; ++i) {
				arr[i+1] = arr[i];
			}
			arr[index] = item;
		}

		public void RemoveAt(int index) {
			for (int i = index; i < count-1; ++i) {
				arr[i] = arr[i+1];
			}
			--count;
		}

		public void CopyTo(T[] array, int arrayIndex) {
			for (int i = 0; i < count; ++i) {
				array[i+arrayIndex] = arr[i];
			}
		}

		public bool Remove(T item) {
			for (int i = 0; i < count; ++i) {
				if (arr[i].Equals(item)) {
					RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		public IEnumerator<T> GetEnumerator() {
			return new Enumerator(arr, count);
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return new Enumerator(arr, count);
		}

		public class Enumerator : IEnumerator<T> {
			private T[] arr;
			private int count;
			private int cursor = 0;

			public Enumerator(T[] arr, int count) {
				this.arr = arr;
				this.count = count;
			}

			public T Current {
				get {
					return arr[cursor];
				}
			}

			object IEnumerator.Current {
				get {
					return arr[cursor];
				}
			}

			public void Dispose() {
			}

			public bool MoveNext() {
				++cursor;
				return cursor < count;
			}

			public void Reset() {
				cursor = 0;
			}
		}
	}
}
