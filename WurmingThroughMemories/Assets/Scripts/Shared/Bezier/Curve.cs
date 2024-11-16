using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

public abstract class Curve<T> : IEnumerable<( float t, T value)> {

	[Serializable]
	protected struct Checkpoint {
		public float t;
		public T value;
	}

	[SerializeField, TableList]
	protected List<Checkpoint> checkpoints = null;

	public Curve() {
		checkpoints = new List<Checkpoint>() { new Checkpoint() { t = 0 }, new Checkpoint() { t = 1 } };
	}

	public Curve(int nPoints) {
		checkpoints = new List<Checkpoint>(nPoints);
		if (nPoints < 2) nPoints = 2;
		for (int i = 0; i < nPoints; i++) {
			checkpoints.Add( new Checkpoint() { t = (float)i / (nPoints - 1) });
		}
	}

	public Curve(IEnumerable<( float t, T value)> value) {
		checkpoints = value.Select(e => new Checkpoint() { value = e.value, t = e.t }).ToList();
	}

	protected abstract T Lerp(T from, T to, float t);

	public T Get(float t) {
		if (t > GetLastT()) t = GetLastT();
		if (t < GetStartT()) t = GetStartT();

		for (int i = 0; i < checkpoints.Count-1; ++i) {
			if (checkpoints[i].t <= t && t <= checkpoints[i+1].t) {
				return Lerp(checkpoints[i].value, checkpoints[i+1].value, Mathf.InverseLerp(checkpoints[i].t, checkpoints[i+1].t, t));

			}
		}

		if (checkpoints.Count == 1) return checkpoints[0].value;

		throw new ArgumentOutOfRangeException();
	}

	public T GetStartPoint() {
		return checkpoints[0].value;
	}

	public T GetEndPoint() {
		return checkpoints[checkpoints.Count-1].value;
	}

	public float GetStartT() {
		return checkpoints[0].t;
	}

	public float GetLastT() {
		return checkpoints[checkpoints.Count - 1].t;
	}

	public T this[float t] => Get(t);
	public IEnumerator<(float t, T value)> GetEnumerator() {
		return checkpoints.Select(e => (e.t, e.value)).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator() {
		return GetEnumerator();
	}

	public int GetValueHashCode() {
		unchecked {
			int hashCode = 1;

			for (int i = 0; i < checkpoints.Count; i++)
			{
				hashCode = (hashCode * 397) ^ checkpoints[i].t.GetHashCode();
				hashCode = (hashCode * 397) ^ checkpoints[i].value.GetHashCode();
			}
			return hashCode;
		}
	}

	public virtual void Clear() {
		checkpoints.Clear();
	}
}
