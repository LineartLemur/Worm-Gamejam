using UnityEngine;
using System;
using UnityEngine.Profiling;

public class CubicBezierCurve {

	private struct Checkpoint {
		public float t;
		public float tTransformed;
		public float distance;
	}

	private Vector3 p1;
	private Vector3 c1;
	private Vector3 c2;
	private Vector3 p2;

	private bool checkpointsInitialized = false;
	private Checkpoint[] checkpoints = null;

	private float totalDistance = 0.0f;

	private int nCheckpoints = 20;

	public CubicBezierCurve(Vector3 p1, Vector3 c1, Vector3 c2, Vector3 p2) {
		Init(p1, c1, c2, p2);
	}

	public void Init(Vector3 p1, Vector3 c1, Vector3 c2, Vector3 p2) {
		this.p1 = p1;
		this.c1 = c1;
		this.c2 = c2;
		this.p2 = p2;
		this.checkpointsInitialized = false;
	}

	public Vector3 Get(float t) {
		return Get(t,p1,c1,c2,p2);
	}
	public static Vector3 Get(float t, Vector3 p1,Vector3 c1,Vector3 c2,Vector3 p2) {
		float u = 1.0f - t;
		float tt = t*t;
		float uu = u*u;
		float uuu = uu * u;
		float ttt = tt * t;

		Vector3 p = uuu * p1; //first term
		p += 3 * uu * t * c1; //second term
		p += 3 * u * tt * c2; //third term
		p += ttt * p2; //fourth term

		return p;
	}

	public Vector3 GetUniformly(float t) {
		FillCheckpoints();

		if (t > 1.0f) t = 1.0f;
		if (t < 0.0f) t = 0.0f;

		for (int i = 0; i < nCheckpoints-1; ++i) {
			if (checkpoints[i].tTransformed <= t && t <= checkpoints[i+1].tTransformed) {
				float realT = Mathf.Lerp(checkpoints[i].t, checkpoints[i+1].t, Mathf.InverseLerp(checkpoints[i].tTransformed, checkpoints[i+1].tTransformed, t));
				return Get(realT);
			}
		}
		throw new ArgumentOutOfRangeException();
	}

	public Vector3 GetStartPoint() {
		return p1;
	}

	public Vector3 GetEndPoint() {
		return p2;
	}

	public float GetDistance() {
		FillCheckpoints();
		return totalDistance;
	}

	public float GetDistance(int amountOfCheckpoints) {
		Profiler.BeginSample("Bezier: GetDistance");
		FillCheckpoints(amountOfCheckpoints);
		Profiler.EndSample();
		return totalDistance;
	}

	private void FillCheckpoints() {
		FillCheckpoints(nCheckpoints);
    }

    private void FillCheckpoints(int amount) {

		// if the amount changed, we reset
		if (checkpoints != null && amount != checkpoints.Length) {

			Debug.Log(Time.frameCount+" "+checkpoints +" "+amount+" != "+ ((checkpoints != null)?checkpoints.Length.ToString(): "" ));
			Profiler.BeginSample("Bezier: reset Checkpoints");
			checkpointsInitialized = false;
			checkpoints = null;
			Profiler.EndSample();
		}

		// already initialized to the correct amount - we skip
		if (checkpointsInitialized) return;


		Profiler.BeginSample("Bezier: Fill Checkpoints");

		nCheckpoints = amount;

		// generate the array only if it wasn't done before
		if (checkpoints == null) checkpoints = new Checkpoint[amount];

		int n = amount;
		float distance = 0.0f;
		Vector3 prevLoc = Get(0.0f);
		for (int i = 0; i < n; ++i) {
			float t = (float)i / (float)(n-1);

			Checkpoint point = new Checkpoint();
			point.t = t;

			Vector3 loc = Get(t);
			distance += (loc - prevLoc).magnitude;
			prevLoc = loc;

			point.distance = distance;

			checkpoints[i] = point;
		}

		totalDistance = checkpoints[checkpoints.Length-1].distance;
		for (int i = 0; i < n; ++i) {
			checkpoints[i].tTransformed = checkpoints[i].distance / totalDistance;
		}

		// only do this once
		checkpointsInitialized = true;

		Profiler.EndSample(); //"Fill Checkpoints"
	}

    public void DrawGizmos(Func<Vector3,Vector3> localToWorld) {
	    Color prev = Gizmos.color;
	    Gizmos.color = Color.green;
	    Gizmos.DrawSphere(localToWorld(p1), 0.02f);
	    Gizmos.DrawSphere(localToWorld(p2), 0.02f);
	    Gizmos.DrawCube(localToWorld(c1), Vector3.one*0.015f);
	    Gizmos.DrawCube(localToWorld(c2), Vector3.one*0.015f);
	    Gizmos.DrawLine(localToWorld(p1),localToWorld(c1));
	    Gizmos.DrawLine(localToWorld(p2),localToWorld(c2));

	    Gizmos.color = Color.yellow;
	    for (int i = 0; i < checkpoints.Length-1; i++) {
		    Gizmos.DrawLine(localToWorld( Get(checkpoints[i].t)),localToWorld( Get(checkpoints[i+1].t)));
	    }

	    Gizmos.color = prev;
    }
}
