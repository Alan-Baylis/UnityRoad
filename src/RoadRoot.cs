using UnityEngine;
using System.Collections;

namespace UnityRoad {
	[RequireComponent(typeof(BoxCollider))]
	public class RoadRoot : MonoBehaviour {
		
		public enum ToolType {
			Select,
			Road,
			Decal,
			SidePoints,
			Wall
		}
		 
		public enum RoadToolType {
			Create,
			AddWaypoint,
			RemoveWaypoint
		}

		public ToolType 	selectedTool;
		public RoadToolType selectedRoadToolMode;

		private Road m_activeRoad;
		private Waypoint m_activeWaypoint;
		private Path m_activePath;

		public Road activeRoad {
			get {
				return m_activeRoad;
			}
		}
		public Waypoint activeWaypoint {
			get {
				return m_activeWaypoint;
			}
		}
		public Path activePath {
			get {
				return m_activePath;
			}
		}

		public Operation currentOperation;

		public Operation BeginCreateRoadOperation(Vector3 position) {
			RoadCreateOperation r = new RoadCreateOperation(this);
			r.Begin(position);
			currentOperation = r;
			return r;
		}

		public void CompleteOperation() {
			currentOperation = null;
			selectedTool = ToolType.Select;
		}

		public void SetActiveRoad(Road r) {
			m_activeWaypoint = null;
			m_activePath = null;
			m_activeRoad = r;
		}

		public void SetActiveWaypoint(Waypoint w) {
			m_activePath = null;

			if( w == null ) {
				m_activeWaypoint = null;
				m_activeRoad = null;
			} else {
				m_activeWaypoint = w;
				m_activeRoad = w.parent;
 			}
		}

		public void SetActivePath(Path p) {
			if( p == null ) {
				m_activePath = null;
				m_activeWaypoint = null;
				m_activeRoad = null;
			} else {
				m_activePath = p;
				m_activeWaypoint = p.next;
				m_activeRoad = p.parent;
			}
		}

		void Awake() {
			Destroy(gameObject);
		}
	}

	public interface Operation {
		void Begin(Vector3 position);
		GameObject End();
	}
	
	public class RoadCreateOperation : Operation {
		public Road newRoad;
		public Waypoint lastWaypoint;

		private RoadRoot m_parent;

		public RoadCreateOperation(RoadRoot r) {
			m_parent = r;
		}

		public void Begin(Vector3 position) {
			newRoad = RoadUtility.CreateRoad(position);
			lastWaypoint = newRoad.root;
		}

		public void Continue(Vector3 position) {

			Waypoint w = RoadUtility.CreateWayPoint(newRoad, position);

			lastWaypoint.ConnectTo(w);
			lastWaypoint = w;
		}

		public GameObject End() {
			GameObject obj = newRoad.gameObject;
			m_parent.CompleteOperation();
			return obj;
		}
	}

	public class AddWaypointOperation : Operation {

		private Waypoint m_wp;
		private RoadRoot m_parent;

		public AddWaypointOperation(RoadRoot r) {
			m_parent = r;
		}

		public void Begin(Vector3 position) {

		}
		public GameObject End() {
			GameObject obj = m_wp.gameObject;
			m_parent.CompleteOperation();
			return obj;
		}
	}

	public class RemoveWaypointOperation : Operation {

		private RoadRoot m_parent;

		public RemoveWaypointOperation(RoadRoot r) {
			m_parent = r;
		}
		
		public void Begin(Vector3 position) {
		}
		public GameObject End() {
			m_parent.CompleteOperation();
			return null;
		}
	}
}
