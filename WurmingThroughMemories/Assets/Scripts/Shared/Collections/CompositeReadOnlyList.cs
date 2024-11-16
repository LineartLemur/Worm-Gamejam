using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.PlayerLoop;

namespace Collections {
	public class CompositeReadOnlyList<T> : IReadOnlyList<T> {

		private List<IReadOnlyList<T>> list = new List<IReadOnlyList<T>>();
		public IEnumerator<T> GetEnumerator() {
			foreach (var le in list) {
				foreach (var e in le) {
					yield return e;
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		public int Count {
			get {
				return list.Select(e => e.Count).Sum();
			}
		}

		public T this[int index] {
			get {
				foreach (var e in list) {
					if (index < e.Count) return e[index];
					index -= e.Count;
				}

				throw new IndexOutOfRangeException();
			}
		}

		public void Clear() {
			this.list.Clear();
		}
		public void AddList(IReadOnlyList<T> list) {
			this.list.Add(list);
		}
	}
}
