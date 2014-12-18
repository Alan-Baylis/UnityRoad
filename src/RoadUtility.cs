 using UnityEngine;
using System.Collections;

namespace UnityRoad {
	public class RoadUtility {
		
		public static Vector3 BezierLerp(Vector3 startPoint, Vector3 endPoint, Vector3 tangentStart, Vector3 tangentEnd, float t)
		{
			return (((-startPoint + 3*(tangentStart-tangentEnd) + endPoint) * t +  (3*(startPoint+tangentEnd) - 6*tangentStart))* t + 
			        3*(tangentStart-startPoint))* t + startPoint;
		}

		/// <summary>
		/// Road Root is an editor helper object during authoring.
		/// </summary>
		/// <returns>The road root.</returns>
		public static RoadRoot GetRoadRoot() {

			// RoadRoot is removed during gameplay (for avoiding false collision detection)
			if( Application.isPlaying ) {
				return null;
			}

			GameObject root = GameObject.Find("__UnityRoadRoot");
			if( root != null ) {
				return root.GetComponent<RoadRoot>();
			} else {
				GameObject obj = new GameObject();
				obj.name = "__UnityRoadRoot";
				obj.hideFlags = HideFlags.HideAndDontSave;
				RoadRoot rr = obj.AddComponent<RoadRoot>();
				BoxCollider bc = obj.GetComponent<BoxCollider>();
				bc.size = new Vector3(1000000.0f, 0.0f, 1000000.0f);
				return rr;
			}
		}

		public static Road CreateRoad(Vector3 position) {
 			GameObject obj = new GameObject();
			obj.name = "Road";
			obj.transform.position = position;
			Road r = obj.AddComponent<Road>();

			Waypoint w = RoadUtility.CreateWayPoint(r, position);
			r.SetRoot(w);

			return r;
		}

		public static Path CreatePath(Waypoint from, Waypoint to) {
			GameObject g = new GameObject("Path");
			Path p = g.AddComponent<Path>();
			g.transform.parent = from.parent.transform;
			g.transform.position = Vector3.Lerp (from.transform.position, to.transform.position, 0.5f);
			g.transform.LookAt(to.transform.position);

			p.Init(from, to);
			return p;
		}

		public static Waypoint CreateWayPoint(Road road, Vector3 position) {

			GameObject g = new GameObject("Waypoint");
			Waypoint w = g.AddComponent<Waypoint>();
			g.transform.parent = road.transform;
			g.transform.position = position;
			g.transform.rotation = Quaternion.identity;

			w.Initialize(road);

			return w;
		}

		public static Waypoint InsertWaypoint(Waypoint w) {
			
			Vector3 newPos;
			
			if( w.next != null ) {
				newPos = w.next.transform.position;
			} else {
				newPos = w.transform.TransformPoint( Vector3.forward );
			}
			
			Waypoint newWaypoint = RoadUtility.CreateWayPoint(w.parent, newPos);
			
			newWaypoint.ConnectTo(w.nextWaypoint);
			w.ConnectTo(newWaypoint);
			
//			EditorUtility.SetDirty(m_target.parent);
			 
			return w;
		}
	}
}
