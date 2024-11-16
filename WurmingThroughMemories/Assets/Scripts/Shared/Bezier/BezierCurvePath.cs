using System;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurvePath {

	public class NotEnoughPositionsException : Exception { }


	private List<CubicBezierCurve> beziers;

	private List<float> distances;

	private float totalDistance = 0.0f;

	private readonly float stepsPerBezier = 20;


	public BezierCurvePath(IList<Vector3> positions, float smoothingStrengthDistance) {
		if (positions.Count < 2) throw new NotEnoughPositionsException();

		beziers = new List<CubicBezierCurve>(positions.Count - 1);
		List<Vector3> directions = new List<Vector3>();
		directions.Add((positions[1]-positions[0]).normalized/ 2f);
		for (int i = 1; i < positions.Count; i++) {
			Vector3 prevPoint = positions[i-1];
			Vector3 nextPoint = positions[i];
			if (i + 1 < positions.Count) {
				Vector3 cross = Vector3.Cross((positions[i - 1] - positions[i]), (positions[i + 1] - positions[i]));
				Vector3 avg = ((positions[i - 1] - positions[i]) + (positions[i + 1] - positions[i]) )/ 2f;
				Vector3 rotated = Quaternion.AngleAxis(90, cross) * avg;


				directions.Add(rotated.normalized * (positions[i + 1] - positions[i - 1]).magnitude/2);
			} else {
				directions.Add((positions[i] - positions[i - 1] ) / 2f);
			}

			beziers.Add(new CubicBezierCurve(
				prevPoint,
				prevPoint + (directions[i-1] * smoothingStrengthDistance),
				nextPoint + (directions[i] * -smoothingStrengthDistance),
				nextPoint
			));
		}

		UpdateDistances();
	}

	private void UpdateDistances() {
		distances = new List<float>(beziers.Count);
		totalDistance = 0.0f;

		for (int i = 0; i < beziers.Count; ++i) {
			CubicBezierCurve curve = beziers[i];

			float distance = 0.0f;
			for (int step = 1; step < stepsPerBezier; ++step) {
				float t1 = (float)(step-1) / (float)(stepsPerBezier - 1);
				float t2 = (float)step / (float)(stepsPerBezier - 1);
				distance += (curve.Get(t2) - curve.Get(t1)).magnitude;
			}

			distances.Add(distance);
			totalDistance += distance;
		}
	}

	public float GetTotalDistance() {
		return totalDistance;
	}

	public Vector3 Get(float distance) {
		if (distance > totalDistance) distance = totalDistance;
		int curveIndex = 0;
		while (distance > distances[curveIndex] && curveIndex < distances.Count-1) {
			distance -= distances[curveIndex];
			++curveIndex;
		}

		float t = distance / distances[curveIndex];

		return beziers[curveIndex].GetUniformly(t);
	}

	public void DrawGizmos(Func<Vector3,Vector3> localToWorld) {
		for (int i = 0; i < beziers.Count; i++) {
			beziers[i].DrawGizmos(localToWorld);
		}
	}
}

