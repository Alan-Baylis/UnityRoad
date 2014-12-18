using UnityEngine;
using UnityEditor;
using System.Collections;

namespace UnityRoad {
	[CustomEditor(typeof(Waypoint))]
	public class WaypointEditor : Editor {
		
		SerializedProperty centerRatio;
		SerializedProperty angle;
		SerializedProperty width;

		SerializedProperty last;
		SerializedProperty next;

		private Waypoint m_target;
		private Vector3 m_mouseDownTargetPos;
		private Vector3 m_lastTargetPos;

		void OnEnable () {
			centerRatio  = serializedObject.FindProperty ("centerRatio");
			angle   	 = serializedObject.FindProperty ("angle");
			width   	 = serializedObject.FindProperty ("m_width");

			last   	 = serializedObject.FindProperty ("m_last");
			next   	 = serializedObject.FindProperty ("m_next");

			m_target 	 = serializedObject.targetObject as Waypoint;

			RoadRoot rr = RoadUtility.GetRoadRoot();
			if(!serializedObject.isEditingMultipleObjects) {
				rr.SetActiveWaypoint(m_target);
			} else {
				rr.SetActiveRoad(null);
			}
		}
		
		public override void OnInspectorGUI() {

			//		// Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
			serializedObject.Update ();

			EditorGUILayout.Slider (centerRatio, 0.0f, 1.0f, new GUIContent ("Center Ratio"));
			EditorGUILayout.Slider (width, 0.1f, 2.0f, new GUIContent ("Road Width(%)"));

			if(GUILayout.Button(new GUIContent("Add Waypoint"))) {
				Waypoint newWaypoint = RoadUtility.InsertWaypoint(m_target);
				Selection.activeGameObject = newWaypoint.gameObject;
			}

			serializedObject.ApplyModifiedProperties ();

			if(GUILayout.Button(new GUIContent("Update Mesh"))) {
				_UpdateMesh();
			}
		}

		private void _UpdateMesh() {
			if( m_target.last && m_target.last.mesh != null ) {
				RoadMeshBuilder.BuildMeshForPath(m_target.last);
				RoadMeshBuilder.BuildMeshForPath(m_target.last.lastPath);
			}
			if( m_target.next && m_target.next.mesh != null ) {
				RoadMeshBuilder.BuildMeshForPath(m_target.next);
				RoadMeshBuilder.BuildMeshForPath(m_target.next.nextPath);
			}
		}

		[DrawGizmo (GizmoType.NotSelected|GizmoType.SelectedOrChild|GizmoType.Active)]
		static void RenderCustomGizmo(Transform objectTransform, GizmoType gizmoType)
		{
			Waypoint w = objectTransform.gameObject.GetComponent<Waypoint>();
			if( w != null ) {
				// Direction Arrow
				Handles.color = Color.gray;
				float dotSize = HandleUtility.GetHandleSize(Vector3.zero) * 0.2f;
				Handles.DrawWireDisc(w.transform.position, w.transform.TransformDirection(Vector3.up), dotSize);				
			}
		}

		public void OnSceneGUI() {

			if (Event.current.commandName=="Delete") {
				Debug.Log(target.name +" delete is deleted");

				Waypoint last = m_target.lastWaypoint;
				Waypoint next = m_target.nextWaypoint;

				m_target.ConnectTo(null);
				m_target.ConnectFrom(null);

				if( last != null && next != null ) {
					last.ConnectTo(next);
				}
				if( m_target.parent.root == m_target ) {
					m_target.parent.SetRoot(next);
				}
			}

			if(Event.current.type == EventType.MouseDown) {
				m_mouseDownTargetPos = m_target.transform.position;
			}
			if(Event.current.type == EventType.MouseDrag) {
				m_lastTargetPos = m_target.transform.position;

				if( m_mouseDownTargetPos != m_lastTargetPos ) {
					if( m_target.last && m_target.last.mesh != null ) {
						if( m_target.last.mesh.vertexCount > 0 ) {
							m_target.last.mesh.Clear();
						}
					}
					if( m_target.next && m_target.next.mesh != null ) {
						if( m_target.next.mesh.vertexCount > 0 ) {
							m_target.next.mesh.Clear();
						}
					}

					m_target.UpdateWaypointDirection(true);
				}
			}

			if( Event.current.type == EventType.MouseUp ) {
				if( m_lastTargetPos != Vector3.zero && m_mouseDownTargetPos != m_lastTargetPos ) {
					_UpdateMesh();
				}
				m_lastTargetPos = Vector3.zero;
				m_mouseDownTargetPos =  Vector3.zero;
			}
		}
	}

}
