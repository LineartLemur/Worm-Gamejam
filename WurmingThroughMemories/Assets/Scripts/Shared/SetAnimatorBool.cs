using UnityEngine;

namespace PepijnWillekens
{
	public class SetAnimatorBool : MonoBehaviour {
        public Animator animator;
        public bool value;
        public new string name;
        private int name_hash;

        private void Awake() {
            name_hash = Animator.StringToHash(name);
        }

        private void Update() {
            animator.SetBool(name_hash,value);
        }
    }
}
