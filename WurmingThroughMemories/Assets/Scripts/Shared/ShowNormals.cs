using System.Linq;
using UnityEngine;

namespace PepijnWillekens.Shared
{
	
	[RequireComponent(typeof(MeshFilter))]
	public class ShowNormals : MonoBehaviour {

		public float length = 0.5f; // inspector

		public Color color = Color.magenta; // inspector

		public bool onlyWhenSelected = false;


		private MeshFilter meshFilter;

		#if UNITY_EDITOR
		public void Start() {
			meshFilter = GetComponent<MeshFilter>();

		}

		public void OnDrawGizmos() {
			if (!onlyWhenSelected) {
				DrawNormals();
			}
		}

		public void OnDrawGizmosSelected() {
			if (onlyWhenSelected) {
				DrawNormals();
			}
		}

		private void DrawNormals() {
			if (!meshFilter) meshFilter = GetComponent<MeshFilter>();
			Mesh mesh = meshFilter.sharedMesh;
			if (mesh == null) return;

			Vector3[] vertices = mesh.vertices;
			Vector3[] normals = mesh.normals;

			Gizmos.color = color;

			for (int i = 0; i < vertices.Count(); ++i) {
				Vector3 p1 = vertices[i];
				Vector3 p2 = vertices[i] + (normals[i] * length);

				p1 = transform.TransformPoint(p1);
				p2 = transform.TransformPoint(p2);

				Gizmos.DrawLine(p1, p2);
			}
		}
		#endif
	}
}
