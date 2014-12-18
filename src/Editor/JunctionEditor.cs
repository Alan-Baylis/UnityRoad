using UnityEngine;
using UnityEditor;
using System.Collections;

namespace UnityRoad {
	[CustomEditor(typeof(Junction))]
	public class JunctionEditor : Editor {

//		SerializedProperty root;

		void OnEnable () {
//			root   		= serializedObject.FindProperty ("m_root");
		}
		
		public override void OnInspectorGUI() {
//			serializedObject.Update ();
//			serializedObject.ApplyModifiedProperties ();
		}

		
//		[DrawGizmo (GizmoType.NotSelected|GizmoType.SelectedOrChild|GizmoType.Active)]
//		static void RenderCustomGizmo(Transform objectTransform, GizmoType gizmoType)
//		{
//		}

//		public void OnSceneGUI() {
//		}
	}
}
