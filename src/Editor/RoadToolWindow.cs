using UnityEngine;
using UnityEditor;
using System.Collections;

namespace UnityRoad {

	public class RoadToolWindow : EditorWindow {

		static private string[] toolLabel = {
			"Select",
			"Road",
			"Decal",
			"SidePoints",
			"Wall"
		};

		static private string[] roadToolModeLabel_NoRoadSelected = {
			"Create",
		};

		static private string[] roadToolModeLabel_RoadSelected = {
			"Create",
		};

		private RoadRoot m_root;

		[MenuItem ("Window/UnityRoad/ToolSet")]
		static void Init () {
			// Get existing open window or if none, make a new one:
			//RoadToolWindow window = (RoadToolWindow)
				EditorWindow.GetWindow (typeof (RoadToolWindow));
		}

		void OnInspectorUpdate() {
			Repaint();
		}			

		void OnGUI () {
			if( m_root == null ) {
				m_root = RoadUtility.GetRoadRoot();
			}
			if( m_root != null ) {
				EditorGUILayout.BeginVertical();
				
				GUILayout.Label ("Tool:", EditorStyles.boldLabel); 
				m_root.selectedTool = (RoadRoot.ToolType) GUILayout.SelectionGrid((int)m_root.selectedTool, toolLabel, 1);
				
				if( m_root.selectedTool == RoadRoot.ToolType.Road ) {
					Selection.activeGameObject = m_root.gameObject;
				}
				
				switch(m_root.selectedTool) {
				case RoadRoot.ToolType.Select:
					break;
				case RoadRoot.ToolType.Road:
					GUILayout.Label ("Mode:", EditorStyles.boldLabel); 
					if( m_root.activeRoad == null ) {
						m_root.selectedRoadToolMode = (RoadRoot.RoadToolType) GUILayout.SelectionGrid((int)m_root.selectedRoadToolMode, roadToolModeLabel_NoRoadSelected, 1);
					} else {
						m_root.selectedRoadToolMode = (RoadRoot.RoadToolType) GUILayout.SelectionGrid((int)m_root.selectedRoadToolMode, roadToolModeLabel_RoadSelected, 1);
					}
					break;
				case RoadRoot.ToolType.Decal:
					break;
				case RoadRoot.ToolType.SidePoints:
					break;
				case RoadRoot.ToolType.Wall:
					break;
				}
				
				EditorGUILayout.EndVertical();
			} else {
				EditorGUILayout.HelpBox("RoadTool is not available during gameplay.", MessageType.Info);
			}
		}
	}
}