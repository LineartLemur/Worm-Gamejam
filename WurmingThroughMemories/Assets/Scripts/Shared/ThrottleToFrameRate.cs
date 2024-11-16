using System;
using System.Threading;
using UnityEngine;

namespace PepijnWillekens {
    public class ThrottleToFrameRate : MonoBehaviour {
        public float targetFPS = 30;
        private DateTime lastFrame;


        private float TargetDeltaTime {
            get {
                return 1 / targetFPS;
            }
        }

        private void Start() {
            lastFrame = DateTime.Now;
        }

        private void Update() {
            while (DateTime.Now < lastFrame + TimeSpan.FromSeconds(TargetDeltaTime)) {
                Thread.Sleep(1);
            }

            lastFrame = DateTime.Now;
        }
    }
}
