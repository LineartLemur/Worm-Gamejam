using UnityEngine;

namespace PepijnWillekens {
    public class ButtonPress {
        public bool Pressed = false;
        public bool Up = false;
        public bool Down = false;
        private float lastDown;
        private float lastUp;

        public void SetPress(bool pressed) {
            if (Pressed) {
                Down = false;
                if (!pressed) {
                    lastUp = Time.unscaledTime;
                    Up = true;
                }
            } else {
                Up = false;
                if (pressed) {
                    lastDown = Time.unscaledTime;
                    Down = true;
                }
            }

            Pressed = pressed;
        }

        public float GetPressDuration() {
            if (Pressed) {
                return Time.unscaledTime - lastDown;
            } else {
                return lastUp - lastDown;
            }
        }

        public override string ToString() {
            return (Up) ? "Up" : ((Down) ? "Down" : Pressed.ToString());
        }
    }
}