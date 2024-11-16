using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shared.Bezier {
	[Serializable]
	public class Vector2Curve : Curve<Vector2> {
		public Vector2Curve() { }
		public Vector2Curve(int nPoints) : base(nPoints) { }
		public Vector2Curve(IEnumerable<(float t, Vector2 value)> value) : base(value) { }

		protected override Vector2 Lerp(Vector2 @from, Vector2 to, float t) {
			return Vector2.Lerp(from, to, t);
		}
		public bool IsZero(float threshHold = 0.0001f) {
			return checkpoints.All(e => e.value.sqrMagnitude < threshHold*threshHold);
		}
	}
}
