using UnityEngine;
using UnityEditor;
using System.Collections;

namespace UnityRoad {
	[CustomEditor(typeof(Path))]
	public class PathEditor : Editor {

		private Path m_target;

		void OnEnable () {

			m_target = target as Path;

			RoadRoot rr = RoadUtility.GetRoadRoot();
			if(!serializedObject.isEditingMultipleObjects) {
				rr.SetActivePath(m_target);
			} else {
				rr.SetActivePath(null);
			}
		}
		
		public override void OnInspectorGUI() {
		}

		public void OnSceneGUI() {

			m_target.UpdatePosition();

			Rect screenRect = new Rect();

			screenRect.position = HandleUtility.WorldToGUIPoint(m_target.transform.position);
			screenRect.size = new Vector2(80.0f, 50.0f);
			screenRect.position -= new Vector2(80.0f, 0.0f);

			Handles.BeginGUI();

			GUILayout.BeginArea(screenRect);
			m_target.hidden = GUILayout.Toggle(m_target.hidden, new GUIContent("Hidden") );
			GUILayout.EndArea();

			Handles.EndGUI();

			
			//		if (Event.current.type == EventType.layout) {
			//			HandleUtility.AddDefaultControl(GUIUtility.GetControlID(GetHashCode(), FocusType.Passive));
			//		}
			//public delegate void DrawCapFunction (int controlID, Vector3 position, Quaternion rotation, float size);
//			
//			Vector3 st = startTangent.vector3Value;
//			Vector3 et = endTangent.vector3Value;
//			
//			if( st == Vector3.zero ) {
//				st = Vector3.left;
//				et = Vector3.right;
//			}
//			
//			st = m_target.transform.TransformPoint(st);
//			et = m_target.transform.TransformPoint(et);
//			
//			Handles.color = Color.blue;
//			Vector3 s = Handles.FreeMoveHandle(st, 
//			                                   Quaternion.identity,
//			                                   1.0f,
//			                                   Vector3.zero, 
//			                                   _DrawTangentControl);
//			
//			Handles.color = Color.red;
//			Vector3 e = Handles.FreeMoveHandle(et, 
//			                                   Quaternion.identity,
//			                                   1.0f,
//			                                   Vector3.zero, 
//			                                   _DrawTangentControl);
//			
//			startTangent.vector3Value = m_target.transform.InverseTransformPoint(s);
//			endTangent.vector3Value   = m_target.transform.InverseTransformPoint(e);
//			
//			Vector3 dir_fromLast = Vector3.forward;
//			Vector3 dir_toNext   = Vector3.forward;
//			
//			if( m_target.last != null ) {
//				dir_fromLast = m_target.transform.position - m_target.last.transform.position;
//				dir_fromLast = dir_fromLast.normalized * 3.0f;
//			}
//			
//			if( m_target.next != null ) {
//				dir_toNext = m_target.next.transform.position - m_target.transform.position;
//				dir_toNext = dir_toNext.normalized * 3.0f;
//			} else {
//				//dir_toNext = dir_fromLast;
//			}
//			
//			Handles.color = Color.cyan;
//			Handles.DrawLine (m_target.transform.position, m_target.transform.position + dir_fromLast );
//			
//			Vector3 mid = Vector3.Slerp (dir_fromLast, dir_toNext, 0.5f);
//			
//			Handles.color = Color.yellow;
//			Handles.DrawLine (m_target.transform.position, m_target.transform.position + mid );
//			
//			
//			serializedObject.ApplyModifiedProperties ();
//			
//			m_target.UpdateWaypointDirection(true);
		}

//		private void _DrawTangentControl (int controlID, Vector3 controlPosition, Quaternion rotation, float size) {
//			
//			if( m_target.last != null ) {
//				Vector3 startPosition = Vector3.Lerp (m_target.last.transform.position, m_target.transform.position, 0.5f);
//				
//				float controlSize = HandleUtility.GetHandleSize(startPosition) * .2f;
//				Handles.DrawLine(startPosition, controlPosition);
//				Handles.DrawWireDisc(controlPosition, m_target.transform.TransformDirection(Vector3.up), controlSize);
//			}
//		}
//

		[DrawGizmo (GizmoType.NotSelected|GizmoType.SelectedOrChild|GizmoType.Active)]
		static void RenderCustomGizmo(Transform objectTransform, GizmoType gizmoType)
		{
			Path p = objectTransform.gameObject.GetComponent<Path>();
			if( p != null ) {
				Waypoint to = p.next;
				Waypoint from = p.last;

				Vector3 tangent = Vector3.Lerp (p.startTangent, p.endTangent, 0.5f);
				Handles.DrawBezier(from.transform.position, 	// start pos
				                   to.transform.position, 		// end pos
					               tangent, 					// start tangent
					               tangent,						// end tangent
				                   Color.white, 				// color
				                   null,						// Texture
				                   3.0f);						// width

				if( !p.hidden ) {
					Handles.DrawBezier(from.left, 					// start pos
					                   to.left, 					// end pos
					                   p.startTangent, 				// start tangent
					                   p.startTangent,				// end tangent
					                   Color.blue, 					// color
					                   null,						// Texture
					                   3.0f);						// width
					
					Handles.DrawBezier(from.right, 						// start pos
					                   to.right,   						// end pos
					                   p.endTangent, 					// start tangent
					                   p.endTangent,					// end tangent
					                   Color.blue, 						// color
					                   null,							// Texture
					                   3.0f);							// width

					// Direction Arrow
					float offsetAmount = HandleUtility.GetHandleSize(Vector3.zero) * .5f;
					Vector3 offset = Vector3.right * offsetAmount;
					
					Vector3 start = from.right + from.transform.TransformDirection(offset);
					Vector3 end   = to.right + to.transform.TransformDirection(offset);

					Handles.color = Color.gray;
	//				float arrowSize = HandleUtility.GetHandleSize(Vector3.zero) * 0.1f;
					Vector3 arrowStart = Vector3.Lerp(start, end, 0.3f);
					Vector3 arrowEnd   = Vector3.Lerp (start,end, 0.8f);
					Vector3 arrowv   = Vector3.Lerp (start,end, 0.7f);
					Vector3 aD = arrowEnd - arrowv;
					float x = aD.x;
					aD.x = aD.z; 
					aD.z = x;
					Handles.DrawLine(arrowStart, arrowEnd);
					Handles.DrawLine(arrowv + aD, arrowEnd);
					Handles.DrawLine(arrowv - aD, arrowEnd);

//					if( p.mesh != null ) {
//						int controlID = GUIUtility.GetControlID(FocusType.Passive);
//						float pointSize = HandleUtility.GetHandleSize(Vector3.zero) * .03f;
//						Handles.color = Color.red;
//						
//						foreach(Vector3 point in p.mesh.vertices) {
//							Handles.DotCap(controlID, p.transform.TransformPoint(point), Quaternion.identity, pointSize);
//						}
//					}
				}
			}
		}
	}
}
