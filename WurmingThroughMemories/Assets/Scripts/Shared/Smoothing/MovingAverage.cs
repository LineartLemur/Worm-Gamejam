using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PepijnWillekens.Shared {
    public abstract class MovingAverage<Data> {
        private float lastTime;
        protected Queue<(Data val, float t)> history = new Queue<(Data x, float t)>();

        public float timeWindow = 5;

        public Data Step(float currentTime, Data x) {

            history.Enqueue((x, currentTime));
            while (history.Peek().t < currentTime - timeWindow) {
                history.Dequeue();
            }

            lastTime = currentTime;

            return Average( history);
        }
        
        public Data Snap(float currentTime, Data x) {
            history.Clear();
            history.Enqueue((x, currentTime));

            lastTime = currentTime;

            return x;
        }

        protected abstract Data Average(Queue<(Data val, float t)> history);
        protected abstract float GetStd(Queue<(Data val, float t)> history);
        protected abstract float GetMedianOrder(Data val);

        public float GetStd() {
            return GetStd(history);
        }
        public int GetN() {
            return history.Count;
        }
        public float GetTrackDuration(float currentTime) {
            if (history.Count <= 0) return 0;
            return currentTime - history.Peek().t ;
        }
        public float Average(IEnumerable<float> series) {
            float cumulative = 0;
            int i = 0;
            foreach (var val in series) {
                cumulative += val;
                i++;
            }

            return cumulative / i;
        }

        public Data GetMedian(float percent) {
            if (history.Count <= 1) return default;
            var duration = history.Max(e=>e.t) -history.Min(e=>e.t);
            var t = duration * percent;
            foreach(var el in history.Sides().Select(e=>(e.Item2.val, e.Item2.t - e.Item1.t)).OrderBy(e=>GetMedianOrder(e.val))) {
                t -= el.Item2;
                if (t <= 0) return el.val;
            }

            throw new ArgumentException("did you enter a percent bigger than 1?");
        }
    }

    public class MovingAverageQuaternion : MovingAverage<Quaternion> {
        protected override Quaternion Average(Queue<(Quaternion val, float t)> history) {

            if (history.Count < 2) return history.Peek().val;
            
            Vector4 cumulative = Vector4.zero;
            Quaternion first = Quaternion.identity;
            Quaternion avg = Quaternion.identity;
            int i = 0;
            foreach (var rot in history) {
                if (i == 0) {
                    first = rot.val;
                    cumulative = new Vector4(first.x, first.y, first.z, first.w);
                } else {
                    avg = AverageQuaternion(ref cumulative, rot.val, first, i);
                }

                i++;
            }

            return avg;
        }

        protected override float GetStd(Queue<(Quaternion val, float t)> history) {
            var avg = Average(history);
            var sqDiff = history.Select(e=> {

                float a = Quaternion.Angle(e.val, avg);
                return a * a;
            });
            return Average(sqDiff);
        }

        protected override float GetMedianOrder(Quaternion val) {
            return Quaternion.Angle(Quaternion.identity, val);
        }


        //Get an average(mean) from more then two quaternions(with two, slerp would be used).
        //Note: this only works if all the quaternions are relatively close together.
        //Usage: 
        //-Cumulative is an external Vector4 which holds all the added x y z and w components.
        //-newRotation is the next rotation to be added to the average pool
        //-firstRotation is the first quaternion of the array to be averaged
        //-addAmount holds the total amount of quaternions which are currently added
        //This function returns the current average quaternion
        public static Quaternion AverageQuaternion(
            ref Vector4 cumulative,
            Quaternion newRotation,
            Quaternion firstRotation,
            int addAmount
        ) {
            float w = 0.0f;
            float x = 0.0f;
            float y = 0.0f;
            float z = 0.0f;

            //Before we add the new rotation to the average (mean), we have to check whether the quaternion has to be inverted. Because
            //q and -q are the same rotation, but cannot be averaged, we have to make sure they are all the same.
            if (!AreQuaternionsClose(newRotation, firstRotation)) {
                newRotation = InverseSignQuaternion(newRotation);
            }

            //Average the values
            float addDet = 1f / (float) addAmount;
            cumulative.w += newRotation.w;
            w = cumulative.w * addDet;
            cumulative.x += newRotation.x;
            x = cumulative.x * addDet;
            cumulative.y += newRotation.y;
            y = cumulative.y * addDet;
            cumulative.z += newRotation.z;
            z = cumulative.z * addDet;

            //note: if speed is an issue, you can skip the normalization step
            return new Quaternion(x, y, z, w);
        }

        public static Quaternion NormalizeQuaternion(float x, float y, float z, float w) {
            float lengthD = 1.0f / (w * w + x * x + y * y + z * z);
            w *= lengthD;
            x *= lengthD;
            y *= lengthD;
            z *= lengthD;

            return new Quaternion(x, y, z, w);
        }

        //Changes the sign of the quaternion components. This is not the same as the inverse.           
        public static Quaternion InverseSignQuaternion(Quaternion q) {
            return new Quaternion(-q.x, -q.y, -q.z, -q.w);
        }

        //Returns true if the two input quaternions are close to each other. This can
        //be used to check whether or not one of two quaternions which are supposed to
        //be very similar but has its component signs reversed (q has the same rotation as
        //-q)
        public static bool AreQuaternionsClose(Quaternion q1, Quaternion q2) {
            float dot = Quaternion.Dot(q1, q2);

            if (dot < 0.0f) {
                return false;
            } else {
                return true;
            }
        }

    }
    public class MovingAverageVector2 : MovingAverage<Vector2> {
        protected override Vector2 Average(Queue<(Vector2 val, float t)> history) {
            Vector2 cumulative = Vector2.zero;
            foreach (var val in history) {
                cumulative += val.val;
            }

            return cumulative / history.Count;
        }

        protected override float GetStd(Queue<(Vector2 val, float t)> history) {
            var avg = Average(history);
            var sqDiff = history.Select(e=> (e.val- avg).sqrMagnitude);
            return Average(sqDiff);
        }

        protected override float GetMedianOrder(Vector2 val) {
            return val.x + val.y;
        }
    }
    public class MovingAverageVector3 : MovingAverage<Vector3> {
        protected override Vector3 Average(Queue<(Vector3 val, float t)> history) {
            Vector3 cumulative = Vector3.zero;
            foreach (var val in history) {
                cumulative += val.val;
            }

            return cumulative / history.Count;
        }

        protected override float GetStd(Queue<(Vector3 val, float t)> history) {
            var avg = Average(history);
            var sqDiff = history.Select(e=> (e.val- avg).sqrMagnitude);
            return Average(sqDiff);
        }

        protected override float GetMedianOrder(Vector3 val) {
            return val.x + val.y +val.z;
        }
    }
    public class MovingAverageFloat : MovingAverage<float> {
        protected override float Average(Queue<(float val, float t)> history) {
            float cumulative = 0;
            foreach (var val in history) {
                cumulative += val.val;
            }

            return cumulative / history.Count;
        }

        protected override float GetStd(Queue<(float val, float t)> history) {
            var avg = Average(history);
            var sqDiff = history.Select(e=> (e.val- avg)*(e.val- avg));
            return Average(sqDiff);
        }

        protected override float GetMedianOrder(float val) {
            return val;
        }

        public float GetMax() {
            return history.Select(e=>e.val).Max();
        }
        public float GetMin() {
            return history.Select(e=>e.val).Min();
        }
    }
}