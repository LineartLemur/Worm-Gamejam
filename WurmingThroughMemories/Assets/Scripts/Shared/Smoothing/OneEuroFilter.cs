using UnityEngine;

namespace PepijnWillekens.Shared {
    public abstract class OneEuroFilter<Data> {
        (float t, Data x, Data dx) _prev;

        private const float dCutOff = 1;
        public float minCutOff = 0.01f;
        public float beta = 0.005f;
        
        public Data Step(float currentTime, Data x)
        {
            var dt = currentTime - _prev.t;

            // Do nothing if the time difference is too small.
            if (dt < 1e-5f) return _prev.x;

            var dx = Difference(x,_prev.x, dt);
            var dx_res = Lerp(_prev.dx, dx, Alpha(dt, dCutOff));

            var cutoff = minCutOff + beta * Length(dx_res);
            var x_res = Lerp(_prev.x, x, Alpha(dt, cutoff));

            _prev = (currentTime, x_res, dx_res);

            return x_res;
        }
        public Data Snap(float currentTime, Data x)
        {
            var dt = currentTime - _prev.t;

            _prev = (currentTime, x, _prev.dx);

            return x;
        }
        
        static float Alpha(float t_e, float cutoff)
        {
            var r = 2 * Mathf.PI * cutoff * t_e;
            return r / (r + 1);
        }

        protected abstract Data Lerp(Data a, Data b, float t);
        protected abstract Data Difference(Data a, Data b, float dt);
        protected abstract float Length(Data a);
        
    }

    public class OneEuroFilterVector3 : OneEuroFilter<Vector3> {
        protected override Vector3 Lerp(Vector3 a, Vector3 b, float t) {
            return Vector3.Lerp(a, b, t);
        }

        protected override Vector3 Difference(Vector3 a, Vector3 b, float dt) {
            return (a - b) / dt;
        }

        protected override float Length(Vector3 a) {
            return a.magnitude;
        }
    }
    public class OneEuroFilterQuaternion : OneEuroFilter<Quaternion> {
        protected override Quaternion Lerp(Quaternion a, Quaternion b, float t) {
            return Quaternion.Lerp(a, b, t);
        }

        protected override Quaternion Difference(Quaternion a, Quaternion b, float dt) {
            return Quaternion.Lerp(Quaternion.identity, Quaternion.Inverse(b) * a, 1f/dt);
        }

        protected override float Length(Quaternion a) {
           return Quaternion.Angle(Quaternion.identity, a);
        }
    }
    public class OneEuroFilterFloat : OneEuroFilter<float> {
        protected override float Lerp(float a, float b, float t) {
            return Mathf.Lerp(a, b, t);
        }

        protected override float Difference(float a, float b, float dt) {
            return (a - b) / dt;
        }

        protected override float Length(float a) {
            return a;
        }
    }
}