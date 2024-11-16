
using UnityEngine;
using System.Collections.Generic;

public class RandomUtility {

	public interface Weighted {
		float getWeight();
	}

	private static SeedRandom rand = new SeedRandom();

	public static float GetQuadRand(float average, float variance) {
		return rand.GetQuadRand(average, variance);
	}
	public static T Pick<T>(IEnumerable<T> arr) {
		return rand.Pick(arr);
	}

	public static List<T> Shuffle<T>(IList<T> list) {
		return rand.Shuffle(list);
	}

	public static bool Roll(float chance) {
		return rand.Roll(chance);
	}

	public static bool GetBool() {
		return rand.GetBool();
	}

	public static int GetInt() {
		return rand.GetInt();
	}

	public static int GetInt(int min, int max) {
		return rand.GetInt(min, max);
	}

	public static float GetAngle() {
		return rand.GetAngle();
	}

	public static int GetSign() {
		return rand.GetSign();
	}

	public static T PickWeighted<T>(IList<T> arr, IList<float> weights) {
		return rand.PickWeighted(arr, weights);
	}

	public static int PickWeighted(IList<float> weights) {
		return rand.PickWeighted(weights);
	}

	public static T PickWeighted<T>(IList<T> arr) where T : RandomUtility.Weighted {
		return rand.PickWeighted(arr);
	}

	public static float Range(float min, float max) {
		return rand.Range(min, max);
	}

	public static int Range(int min, int max) {
		return rand.Range(min, max);
	}

	public static float value => Range(0.0f, 1.0f);

	public Vector3 onUnitSphere {
		get {
			return rand.onUnitSphere;
		}
	}
	
	
	public static float ValueWithSeed(int seed) {
		var prev = Random.state;
		Random.InitState(seed);
		var f = Random.value;
		Random.state = prev;
		return f;
	}
}