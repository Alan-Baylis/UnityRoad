using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace UnityRoad {	
	public class Path : MonoBehaviour {

		/// <summary>
		/// The hidden path will not be used for mesh generation.
		/// </summary>
		public bool hidden;

		public int submeshIndex;

		[SerializeField]
		[Range(-100.0f, 100.0f)]
		private float m_tangent = 0.0f;

		[SerializeField]
		[Range(0.0f, 1.0f)]
		private float m_curvePos = 0.5f;

		[SerializeField]
		private Waypoint m_last;
		
		[SerializeField]
		private Waypoint m_next;

		[SerializeField]
		private Mesh m_mesh;

		[SerializeField]
		private Material m_material;

		public Waypoint last {
			get {
				return m_last;
			}
		}
		public Waypoint next {
			get {
				return m_next;
			}
		}

		public Path lastPath {
			get {
				if(m_last == null) {
					return null;
				} else {
					return m_last.last;
				}
			}
		}
		public Path nextPath {
			get {
				if(m_next == null) {
					return null;
				} else {
					return m_next.next;
				}
			}
		}

		public Road parent {
			get {
				return m_last.parent;
			}
		}
		public float tangent {
			get {
				if(hidden) {
					return 0.0f;
				} else {
					return m_tangent;
				}
			}
		}

		public Mesh mesh {
			get {
				return m_mesh;
			}
		}

		public Material material {
			get {
				if( m_material ) {
					return m_material;
				} else {
					return parent.defaultMaterial;
				}
			}
		}


		/// <summary>
		/// Gets the start tangent.
		/// </summary>
		/// <value>The start tangent.</value>
		public Vector3 startTangent {
			get {
				return Vector3.Lerp(m_last.left, m_next.left, m_curvePos) + m_last.transform.TransformDirection(Vector3.right * tangent);
				//return m_next.transform.InverseTransformPoint(s);
			}
		}

		/// <summary>
		/// Gets the end tangent.
		/// </summary>
		/// <value>The end tangent.</value>
		public Vector3 endTangent {
			get {
				return Vector3.Lerp(m_last.right, m_next.right, m_curvePos) + m_last.transform.TransformDirection(Vector3.right * tangent);
				//return m_next.transform.InverseTransformPoint(e);
			}
		}

		public Waypoint Other(Waypoint p) {
			if( p == m_last ) {
				return m_next;
			}
			if( p == m_next ) {
				return m_last;
			}
			Debug.LogError("Given waypoint is not associated with this path.");
			return null;
		}
		
		public void Init(Waypoint last, Waypoint next) {
			m_last = last;
			m_next = next;
			UpdatePosition();
		}		

		public void UpdatePosition() {
			transform.position = Vector3.Lerp (m_last.transform.position, m_next.transform.position, 0.5f);
		}

		public void SetMesh(Mesh m) {
			m_mesh = m;

			MeshFilter f = GetComponent<MeshFilter>();
			if( f == null ) {
				f = gameObject.AddComponent<MeshFilter>();
			}
			MeshRenderer r = GetComponent<MeshRenderer>();
			if( r == null ) {
				r = gameObject.AddComponent<MeshRenderer>();
			}
			MeshCollider c = gameObject.GetComponent<MeshCollider>();
			if( c == null ) {
				c = gameObject.AddComponent<MeshCollider>();
			}

			if( m != f.sharedMesh ) {
				f.sharedMesh = m;
			}
			if( m != c.sharedMesh ) {
				c.sharedMesh = m;
			}
		}

		public void UpdateRoadMaterial() {
			MeshRenderer r = GetComponent<MeshRenderer>();
			if( r != null ) {
				r.material = material;
			}
		}

		public void RemoveMesh() {
			MeshRenderer r = GetComponent<MeshRenderer>();
			if( r != null ) {
				DestroyImmediate(r);
			}
			MeshCollider c = GetComponent<MeshCollider>();
			if( c == null ) {
				DestroyImmediate(c);
			}
			MeshFilter f = GetComponent<MeshFilter>();
			if( f != null ) {
				DestroyImmediate(f);
			}

			m_mesh = null;
		}
	}
}