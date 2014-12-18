using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityRoad {
		
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshCollider))]
	[RequireComponent(typeof(MeshRenderer))]
	public class Road : MonoBehaviour {
	
		// hidden path will not be use for mesh generation
		public bool hidden;

		[SerializeField]
		private Waypoint m_root;

		[SerializeField]
		private Mesh m_roadMesh;

		[SerializeField ]
	 	private int m_meshQuality = 50;
	 
		public float defaultWidth = 5.0f;

		[SerializeField ]
		private Material m_defaultMaterial;

		public Waypoint root {
			get {
				return m_root;
			}
		}

		public Waypoint last {
			get {
				if( m_root == null ) {
					return null;
				}
				if( m_root.last != null ) {
					return m_root.last.last;
				} else {
					Waypoint w = m_root;
					while(w.next != null) {
						w = w.next.next;
					} 
					return w;
				}
			}
		}

		public bool closed {
			get {
				return m_root != null && m_root.last != null;
			}
		}
		public int meshQuality {
			get {
				return m_meshQuality;
			}
		}

		public void SetRoot(Waypoint wp) {
			m_root = wp;
		}

		public void Close(bool bClose) {
			if( bClose ) {
				if( m_root != null && !this.closed ) {
					m_root.ConnectFrom(this.last);
				}
			} else {
				if( m_root != null && this.closed ) {
					m_root.ConnectFrom(null);
				}
			}
		}

		public Material defaultMaterial {
			get {
				return m_defaultMaterial;
			}
		}
	}
}