using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shared.Bezier {
	[Serializable]
	public class FloatCurve : Curve<float> {
		public FloatCurve() { }
		public FloatCurve(int nPoints) : base(nPoints) { }
		public FloatCurve(IEnumerable<(float t, float value)> value) : base(value) { }

		protected override float Lerp(float @from, float to, float t) {
			return Mathf.LerpUnclamped(from, to, t);
		}
	}
}
