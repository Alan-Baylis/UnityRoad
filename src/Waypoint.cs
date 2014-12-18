using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityRoad {
	public class Waypoint : MonoBehaviour {

		[SerializeField]
		[Range(0.0f, 2.0f)]
		private float m_width = 1.0f;
		
		[Range(0.0f,1.0f)]
		public float centerRatio = 0.5f; // 0.0f -> left, 1.0f -> right

		[Range(-360.0f,360.0f)]
		public float angle = 0.0f; // 0.0f -> left, 1.0f -> right

 		[SerializeField]
		private Path m_last;
	
		[SerializeField]
		private Path m_next;
		
		[SerializeField]
		private  Junction m_junction;

		[SerializeField]
		private Road m_parent;
		 
		public Path last {
			get {
				return m_last;
			}
		}
		public Path next {
			get {
				return m_next;
			}
		}
		public Road  parent {
			get {
				return m_parent;
			}
		}
		public bool isIsolated {
			get {
				return m_last == null  && m_next == null;
			}
		}

		public float width {
			get {
				return m_parent.defaultWidth * m_width;
			}
		}

		public Waypoint lastWaypoint {
			get {
				return (m_last != null) ?  m_last.Other (this) : null;
			}
		} 
		public Waypoint nextWaypoint {
			get { 
				return (m_next != null) ?  m_next.Other (this) : null;
			}
		}

		public Vector3 left {
			get {
				return transform.TransformPoint( Vector3.left * (1.0f - centerRatio) * width );
			}
		}
		
		public Vector3 right {
			get {
				return transform.TransformPoint( Vector3.right * (centerRatio) * width );
			}
		}

		public void Initialize(Road r) { 
			m_parent = r;
 		}

		void OnDrawGizmos() {
			Gizmos.DrawIcon(transform.position, "UnityRoad/Waypoint.psd", true);
		}

		public void ConnectTo(Waypoint next) {
			if( m_next ) {
				DestroyImmediate(m_next.gameObject);
			}
			if( next != null ) {
				if( next.m_last != null ) {
					DestroyImmediate(next.m_last.gameObject);
				}

				m_next = RoadUtility.CreatePath(this, next);
				next.m_last = m_next;
			}

			UpdateWaypointDirection(true);
		}
		
 		public void ConnectFrom(Waypoint last) {
			if( m_last ) {
				DestroyImmediate(m_last.gameObject);
			}
			if( last != null ) {
				if( last.m_next != null ) {
					DestroyImmediate(last.m_next.gameObject);
				}
				
				m_last = RoadUtility.CreatePath(last, this);
				last.m_next = m_last;
			}

			UpdateWaypointDirection(true);
		}
		
		public void UpdateWaypointDirection(bool propagate) {
			// direct lerp 0.5 of last - next
			
			Vector3 dir_fromLast = Vector3.forward;
			Vector3 dir_toNext   = Vector3.forward;

			Waypoint last = lastWaypoint;
 			Waypoint next = nextWaypoint;

			if( last != null ) {
				dir_fromLast = transform.position - last.transform.position;
				dir_fromLast = dir_fromLast.normalized * 3.0f;
			}
			
			if( next != null ) {
				dir_toNext = next.transform.position - transform.position;
				dir_toNext = dir_toNext.normalized * 3.0f;
			} 

			if( last == null ) {
				// lookat position + dir_tonext
				transform.LookAt(transform.position + dir_toNext);
			} else if (next == null) {
				// lookat position + dir_fromlast
				transform.LookAt(transform.position + dir_fromLast);
			} else {
				Vector3 mid = Vector3.Slerp (dir_fromLast, dir_toNext, 0.5f);
				transform.LookAt(transform.position + mid);
			}

			if( this.last ) {
				this.last.UpdatePosition();
			}

			if( this.next ) {
				this.next.UpdatePosition();
			}

			if( propagate ) {
				if( last != null ) {
					last.UpdateWaypointDirection(false);
				}
				if( next != null ) {
					next.UpdateWaypointDirection(false);
				} 
			}
		}		
	} 
}
