
using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class SeedRandom {

	private MTRandom.MTRandom rand;

	public SeedRandom(int seed) {
		rand = new MTRandom.MTRandom(seed);
	}

	public SeedRandom() {
		rand = new MTRandom.MTRandom();
	}

	public float GetQuadRand(float average, float variance) {

		// a random number between [0,1]
		float x = rand.value();

		float t = x * 2;
		if (t > 1) {
			t = 2 - t;
			float b = average + variance;
			float c = -variance;
			return -c * t * (t-2) + b;
		}
		else {
			float c = variance;
			float b = average - variance;
			return -c * t * (t-2) + b;
		}
	}

	public T Pick<T>(IEnumerable<T> arr) {
		if (arr.Count() == 0) return default(T);
		return arr.ElementAt(rand.Range(0, arr.Count() - 1));
	}

	public List<T> Shuffle<T>(IList<T> list) {
		int n = list.Count;
		List<T> result = new List<T>();
		foreach (T t in list) result.Add (t);
		while (n > 1) {  
			n--;
			int k = rand.Range(0, n);
			T value = result[k];
			result[k] = result[n];
			result[n] = value;
		}
		return result;
	}

	public bool Roll(float chance) {
		return rand.value() < chance;
	}

	public bool GetBool() {
		return Roll(0.5f);
	}

	public int GetInt() {
		return rand.Range(0, int.MaxValue-1);
	}

	public int GetInt(int min, int max) {
		return rand.Range(min, max);
	}

	public float GetAngle() {
		return rand.Range (0.0f, Mathf.PI * 2.0f);
	}

	public int GetSign() {
		return GetBool() ? 1 : -1;
	}

	public T PickWeighted<T>(IList<T> arr, IList<float> weights) {
		return arr[PickWeighted(weights)];
	}

	public int PickWeighted(IList<float> weights) {
		float total = weights.Sum();
		float pick = rand.Range(0.0f, total);
		int idx = 0;
		while (idx < weights.Count-1 && weights[idx] < pick) pick -= weights[idx++];
		return idx;
	}

	public T PickWeighted<T>(IList<T> arr) where T:RandomUtility.Weighted {
		float total = 0.0f;
		foreach (T obj in arr) total += obj.getWeight();
		float pick = rand.Range (0.0f, total);
		int idx = 0;
		while (idx < arr.Count-1 && arr[idx].getWeight () < pick) pick -= arr[idx++].getWeight();
		return arr[idx];
	}

	// COMPATIBILITY!
	public float Range(float min, float max) {
		return rand.Range(min, max);
	}

	public int Range(int min, int max) {
		return rand.Range(min, max, false);
	}

	public float Value {
		get {
			return rand.value();
		}
	}


	public Vector2 insideUnitCircle {
		get {
			return GenerateInsideUnitCircle();
		}
	}

	private Vector2 GenerateInsideUnitCircle() {
		Vector2 v = new Vector2();
		v.x = Range(-1.0f, 1.0f);
		v.y = Range(-1.0f, 1.0f);
		while (v.sqrMagnitude > 1.0f) {
			v.x = Range(-1.0f, 1.0f);
			v.y = Range(-1.0f, 1.0f);
		}
		return v;
	}

	public Vector2 onUnitCircle {
		get {
			return GenerateOnUnitCircle();
		}
	}

	private Vector2 GenerateOnUnitCircle() {
		float angle = Range(0.0f, Mathf.PI * 2.0f);
		return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
	}

	public Vector3 onUnitSphere {
		get {
			return GenerateOnUnitSphere();
		}
	}

	private Vector3 GenerateOnUnitSphere() {
		// http://mathworld.wolfram.com/SpherePointPicking.html
		float theta = Range(0.0f, 2.0f * Mathf.PI);
		float u = Range(-1.0f, 1.0f);

		float x = Mathf.Sqrt(1.0f - u*u) * Mathf.Cos(theta);
		float y = Mathf.Sqrt(1.0f - u*u) * Mathf.Sin(theta);
		float z = u;

		return new Vector3(x, y, z);
	}

	public Vector2 GetPosInRect(Rect rectangle) {
		return new Vector2(Range(rectangle.xMin, rectangle.xMax), Range(rectangle.yMin, rectangle.yMax));
	}
}