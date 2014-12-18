using UnityEngine;
using UnityEditor;
using System.Collections;

namespace UnityRoad {
	[CustomEditor(typeof(Road))]
	public class RoadEditor : Editor {

		SerializedProperty root;
		SerializedProperty roadMesh;
		SerializedProperty meshQuality; // 0 - 99. 50=default
		SerializedProperty roadWidth;
		SerializedProperty defaultMaterial;
		Road m_target;

		void OnEnable () {
			root   		= serializedObject.FindProperty ("m_root");
			roadMesh    = serializedObject.FindProperty ("m_roadMesh");
			meshQuality = serializedObject.FindProperty ("m_meshQuality");
			roadWidth   = serializedObject.FindProperty ("defaultWidth");
			defaultMaterial   = serializedObject.FindProperty ("m_defaultMaterial");
			m_target = target as Road;

			RoadRoot rr = RoadUtility.GetRoadRoot();
			if(!serializedObject.isEditingMultipleObjects) {
				rr.SetActiveRoad(m_target);
			} else {
				rr.SetActiveRoad(null);
			}
		}
		
		public override void OnInspectorGUI() {

			serializedObject.Update ();

			EditorGUILayout.IntSlider (meshQuality, 0, 99, new GUIContent ("Mesh Quality"));
			EditorGUILayout.Slider (roadWidth, 1.0f, 100.0f, new GUIContent ("Base Road Width"));

			bool bLast = m_target.closed;
			bool bClosed = EditorGUILayout.Toggle("Closed", bLast);
			if(bLast != bClosed) {
				m_target.Close(bClosed);
			}

			EditorGUILayout.PropertyField(defaultMaterial);

			if( GUILayout.Button("Build Mesh") ) {
				RoadMeshBuilder.BuildMeshForRoad(m_target);
			}

			EditorUtility.SetDirty(m_target); 

			serializedObject.ApplyModifiedProperties ();
		}


//		public void OnSceneGUI() {
//
//	//		Waypoint last = null;
//	//		for(int i = 0; i < waypoints.arraySize; ++i) {
//	//			SerializedProperty sp = waypoints.GetArrayElementAtIndex(i);
//	//			Waypoint w = sp.objectReferenceValue as Waypoint;
//	//			if( last != null ) {
//	//				float width = HandleUtility.GetHandleSize(Vector3.zero) * 0.1f;
//	//				
//	//				Handles.DrawBezier(last.transform.position, 	// start pos
//	//				                   w.transform.position, 		// end pos
//	//				                   Vector3.up, 					// start tangent
//	//				                   -Vector3.up,					// end tangent
//	//				                   Color.white, 					// color
//	//				                   null,						// Texture
//	//				                   width);						// width
//	//			}
//	//			last = w;
//	//		}
//		}

		public void OnSceneGUI() {
			RoadEditorGUIUtility.DoSceneGUI();
		}

		// Custom GUILayout progress bar.
	//	function ProgressBar (value : float, label : String) {
	//		// Get a rect for the progress bar using the same margins as a textfield:
	//		var rect : Rect = GUILayoutUtility.GetRect (18, 18, "TextField");
	//		EditorGUI.ProgressBar (rect, value, label);
	//		EditorGUILayout.Space ();
	//	}

//		[DrawGizmo (GizmoType.NotSelected|GizmoType.SelectedOrChild|GizmoType.Active)]
//		static void RenderCustomGizmo(Transform objectTransform, GizmoType gizmoType)
//		{
//			Path p = objectTransform.gameObject.GetComponent<UnityRoad.Path>();
//			if( p != null ) {
//				
//				MeshFilter mf = r.gameObject.GetComponent<MeshFilter>();
//				Mesh m = mf.sharedMesh;
//				if( m != null ) {
//					Vector3[] verts = m.vertices;
//					foreach(Vector3 v in verts) {
//						float dotSize = HandleUtility.GetHandleSize(Vector3.zero) * 0.015f;
//						Handles.color = Color.green;
//						Handles.DotCap(0,
//						               r.transform.TransformPoint(v),
//						               Quaternion.identity,
//						               dotSize);
//					}
//				}
//				
//				for(int i = 0; i < r.waypoints.Length; ++i) {
//					Waypoint w = r.waypoints[i];
//					Waypoint last = w.last;
//					if( last != null ) {
//						Vector3 tangentStart = w.transform.TransformPoint(Vector3.zero);
//						Vector3 tangentEnd = w.transform.TransformPoint(Vector3.zero);
//						Handles.DrawBezier(last.transform.position, 	// start pos
//						                   w.transform.position, 		// end pos
//						                   tangentStart, 				// start tangent
//						                   tangentEnd,					// end tangent
//						                   Color.white, 				// color
//						                   null,						// Texture
//						                   3.0f);						// width
//						
//						tangentStart = w.transform.TransformPoint(w.startTangent);
//						tangentEnd = w.transform.TransformPoint(w.startTangent);
//						Handles.DrawBezier(last.left, 					// start pos
//						                   w.left, 						// end pos
//						                   tangentStart, 				// start tangent
//						                   tangentEnd,					// end tangent
//						                   Color.blue, 					// color
//						                   null,						// Texture
//						                   3.0f);						// width
//						
//						tangentStart = w.transform.TransformPoint(w.endTangent);
//						tangentEnd = w.transform.TransformPoint(w.endTangent);
//						Handles.DrawBezier(last.right, 						// start pos
//						                   w.right, 						// end pos
//						                   tangentStart, 					// start tangent
//						                   tangentEnd,						// end tangent
//						                   Color.blue, 						// color
//						                   null,							// Texture
//						                   3.0f);							// width
//						
//						// Direction Arrow
//						float offsetAmount = HandleUtility.GetHandleSize(Vector3.zero) * .5f;
//						Vector3 offset = Vector3.right * offsetAmount;
//						
//						//					tangentStart = w.transform.TransformPoint(w.endTangent + offset);
//						//					tangentEnd = w.transform.TransformPoint(w.endTangent  + offset);
//						Vector3 start = last.right + last.transform.TransformDirection(offset);
//						Vector3 end   = w.right + w.transform.TransformDirection(offset);
//						//					Handles.DrawBezier(Vector3., 			// start pos
//						//					                   end, 			 // end pos
//						//					                   tangentStart, 					// start tangent
//						//					                   tangentEnd,						// end tangent
//						//					                   Color.gray, 						// color
//						//					                   null,							// Texture
//						//					                   1.0f);							// width
//						
//						Handles.color = Color.gray;
//						float arrowSize = HandleUtility.GetHandleSize(Vector3.zero) * 0.1f;
//						Vector3 arrowStart = Vector3.Lerp(start, end, 0.3f);
//						Vector3 arrowEnd   = Vector3.Lerp (start,end, 0.8f);
//						Vector3 arrowv   = Vector3.Lerp (start,end, 0.7f);
//						Vector3 aD = arrowEnd - arrowv;
//						float x = aD.x;
//						aD.x = aD.z;
//						aD.z = x;
//						Handles.DrawLine(arrowStart, arrowEnd);
//						Handles.DrawLine(arrowv + aD, arrowEnd);
//						Handles.DrawLine(arrowv - aD, arrowEnd);
//					}
//				}
//				
//				for(int i = 0; i < r.waypoints.Length; ++i) {
//					Waypoint w = r.waypoints[i];
//					if( w != null ) {
//						float dotSize = HandleUtility.GetHandleSize(w.transform.position) * 0.05f;
//						Handles.color = Color.red;
//						Handles.DotCap(0,
//						               w.transform.position,
//						               w.transform.rotation,
//						               dotSize);
//					}
//				}
//			}
//		}


	}
}
