using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shared.Bezier {
	[Serializable]
	public class Vector3Curve : Curve<Vector3> {
		private static Color[] c = new[]
			{ Color.magenta, Color.yellow, Color.green, Color.cyan, Color.blue, Color.red };
		public void DrawDebugLine(Func<Vector3,float,Vector3> localToWorld, float duration = 0.0f, bool depthTest = false) {
			var color = c.Random();
			for (int i = 0; i < checkpoints.Count-1; i++) {
				var pos = localToWorld(checkpoints[i].value, checkpoints[i].t);
				Debug.DrawLine(pos, localToWorld( checkpoints[i+1].value, checkpoints[i+1].t), color, duration, depthTest);
			}
		}
		public void DrawDebugGizmo(Func<Vector3,float,Vector3> localToWorld) {
			var color = c.Random();
			for (int i = 0; i < checkpoints.Count-1; i++) {
				var pos = localToWorld(checkpoints[i].value, checkpoints[i].t);
				Gizmos.DrawLine(pos, localToWorld( checkpoints[i+1].value, checkpoints[i+1].t));
			}
		}

		public Vector3Curve() { }
		public Vector3Curve(int nPoints) : base(nPoints) { }
		public Vector3Curve(IEnumerable<(float t, Vector3 value)> value) : base(value) { }

		protected override Vector3 Lerp(Vector3 @from, Vector3 to, float t) {
			return Vector3.Lerp(from, to, t);
		}

		public new int GetValueHashCode() {
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

		public bool IsZero(float threshHold = 0.01f) {
			return checkpoints.All(e => e.value.sqrMagnitude < threshHold*threshHold);
		}

		public void AddOffset(Vector3 offset) {
			for (int i = 0; i < checkpoints.Count; i++) {
				var checkpoint = checkpoints[i];
				checkpoint.value += offset;
				checkpoints[i] = checkpoint;
			}
		}
	}
}
