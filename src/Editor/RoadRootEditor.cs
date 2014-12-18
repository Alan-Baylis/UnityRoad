 using UnityEngine;
using UnityEditor;
using System.Collections;

namespace UnityRoad {
	[CustomEditor(typeof(RoadRoot))]
	public class RoadRootEditor : Editor {

		RoadRoot m_target;

		void OnEnable () {
			m_target = target as RoadRoot;
		}
		 
		public void OnSceneGUI() {
			RoadEditorGUIUtility.DoSceneGUI();
		}

//		[DrawGizmo (GizmoType.NotSelected|GizmoType.SelectedOrChild|GizmoType.Active)]
//		static void RenderCustomGizmo(Transform objectTransform, GizmoType gizmoType)
//		{
//
//		}	
	}
	
	internal class RoadEditorGUIUtility {

		public static void DoSceneGUI() {
			int controlID = GUIUtility.GetControlID(FocusType.Passive);
			Event e = Event.current;
			RoadRoot root = RoadUtility.GetRoadRoot();

			switch(root.selectedTool) {
			case RoadRoot.ToolType.Select:
				break;
			case RoadRoot.ToolType.Road:
				_EditRoad(root, e, controlID);
				break;
			case RoadRoot.ToolType.Decal:
				break;
			case RoadRoot.ToolType.SidePoints:
				break;
			case RoadRoot.ToolType.Wall:
				break;
			}
		}
		
		private static void _EditRoad(RoadRoot root, Event e , int controlID) {
			
			switch(root.selectedRoadToolMode) {
			case RoadRoot.RoadToolType.Create:
				_EditRoad_Create(root, e, controlID);
				break;
			case RoadRoot.RoadToolType.AddWaypoint:
				_EditRoad_AddWaypoint(root, e, controlID);
				break;
			case RoadRoot.RoadToolType.RemoveWaypoint:
				_EditRoad_RemoveWaypoint(root, e, controlID);
				break;
			}
		}

		private static void _EditRoad_Create_DrawNextPathPreview(RoadRoot root, Event e, int controlID) {

			RoadCreateOperation op = root.currentOperation as RoadCreateOperation;
			if( op == null ) {
				return;
			}
			
			Ray r = HandleUtility.GUIPointToWorldRay(e.mousePosition);
			RaycastHit hit;
			if( Physics.Raycast(r, out hit) ) {
				
				Waypoint from = op.lastWaypoint;
				Vector3 curPos = hit.point;

				Vector3 tangent = Vector3.Lerp (from.transform.position, curPos, 0.5f);
				Handles.DrawBezier(from.transform.position, 	// start pos
				                   curPos, 		// end pos
				                   tangent, 					// start tangent
				                   tangent,						// end tangent
				                   Color.magenta, 				// color
				                   null,						// Texture
				                   3.0f);						// width
			}
		}

		/// <summary>
		/// HACK: force invoke Repaint Event in SceneView.
		/// OnSceneGUI() will not be redrawn unless Repaint Event is kicked,
		/// so moving last waypoint by zero to force make repaint happen.
		/// </summary>
		private static void _EditRoad_Create_ForceInvokeRepaint(RoadRoot root) {
			RoadCreateOperation op = root.currentOperation as RoadCreateOperation;
			if( op != null ) {
				op.lastWaypoint.transform.Translate(new Vector3(0,0,0));
			}
		}
		
		private static void _EditRoad_Create(RoadRoot root, Event e, int controlID) {
 
			switch(e.GetTypeForControl(controlID)) {
			case EventType.mouseMove:
				// to draw preview line in scene, invoke repaint event when mouse is moved.
				_EditRoad_Create_ForceInvokeRepaint(root);
				break;
			case EventType.Repaint:
				_EditRoad_Create_DrawNextPathPreview(root, e, controlID);
				break;
			case EventType.MouseDown:
				GUIUtility.hotControl = controlID;
				if( e.clickCount == 2 ) {
					RoadCreateOperation op = root.currentOperation as RoadCreateOperation;
					if( op != null ) {
						Selection.activeGameObject = op.End();
					}
				}
				Event.current.Use();
				break;
			case EventType.MouseUp:
				GUIUtility.hotControl = 0;
				Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
				RaycastHit hit;
				if( Physics.Raycast (ray, out hit) ) {
					RoadCreateOperation op = root.currentOperation as RoadCreateOperation;
					if( op == null ) {
						root.BeginCreateRoadOperation(hit.point);
					} else {
						op.Continue(hit.point);
					}
				}
				Event.current.Use();
				break;
			}
		}

		private static void _EditRoad_AddWaypoint(RoadRoot root, Event e, int controlID) {
			
			switch(e.GetTypeForControl(controlID)) {
			case EventType.MouseDown:
				GUIUtility.hotControl = controlID;
				Debug.Log ("[AddWaypoint]Mouse Down" + e.mousePosition);
				Event.current.Use();
				break;
			case EventType.MouseUp:
				GUIUtility.hotControl = 0;
				Debug.Log ("[AddWaypoint]Mouse Up" + e.mousePosition);
				Event.current.Use();
				break;
			}
		}
		
		private static void _EditRoad_RemoveWaypoint(RoadRoot root, Event e, int controlID) {
			
			switch(e.GetTypeForControl(controlID)) {
			case EventType.MouseDown:
				GUIUtility.hotControl = controlID;
				Debug.Log ("[RemoveWaypoint]Mouse Down" + e.mousePosition);
				Event.current.Use();
				break;
			case EventType.MouseUp:
				GUIUtility.hotControl = 0;
				Debug.Log ("[RemoveWaypoint]Mouse Up" + e.mousePosition);
				Event.current.Use();
				break;
			}
		}
	}
}
