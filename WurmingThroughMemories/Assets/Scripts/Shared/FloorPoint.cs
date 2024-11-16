using PepijnWillekens.ManagerSystem;
using UnityEngine;

namespace PepijnWillekens.Helper {
	public class FloorPoint : AutoInit {
        public LayerMask layerMask = int.MaxValue;
        public Color gizmoColor = Color.green;
        private Vector3 cachedTransformPosition;
        private Vector3 cachedPosition;
        public Vector3 position {
            get {
                var transformPosition = transform.position;
                if (!cachedTransformPosition.EqualsVector(transformPosition)) {
                    cachedTransformPosition = transformPosition;
                    
                    //raycast point to floor.
                    cachedPosition = transform.position;
                    if (gameObject.scene.GetPhysicsScene().Raycast(cachedPosition + Vector3.up * 0.5f, Vector3.down, out var hit, 1,layerMask)) {
                        cachedPosition = hit.point;
                    }
                }

                return cachedPosition;
            }
        }
        public Quaternion rotation {
            get {
                return transform.rotation;
            }
        }
        private void OnDrawGizmos() {
            Color prev = Gizmos.color;
            Gizmos.color = gizmoColor;
            Gizmos.DrawRay(transform.position + Vector3.up * 0.5f, Vector3.down);
            var pos = position;
            var rot = rotation;
            for (int i = 5; i >1; i--) {
                
                Gizmos.DrawSphere(pos +  rot * (Vector3.forward * 0.02f*i),0.05f-(0.009f*i));
            }
            Gizmos.DrawSphere(pos,0.05f);
            
            Gizmos.color = prev;
        }
    }
}