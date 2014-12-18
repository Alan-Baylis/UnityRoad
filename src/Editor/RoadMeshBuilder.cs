using UnityEngine;
using System.Collections;

namespace UnityRoad {
	public class RoadMeshBuilder : MonoBehaviour {

		public static void BuildMeshForPath(Path p) { 

			if( p == null ) {
				return;
			}

			Mesh m = p.mesh;
			if( m == null ) {
				m = new Mesh();
				m.name = "Road Mesh";
				p.SetMesh(m);
			}
			
			if( p.hidden ) {
				p.RemoveMesh();
				return;
			}

			p.UpdateRoadMaterial();

			int meshLengthValue  = p.parent.meshQuality;
			int meshDensityValue = meshLengthValue + 1; // temp solution
			int vertLength = (meshDensityValue + 1) * 3;
			Vector3[] vertices = new Vector3[vertLength];
			Vector2[] uv = new Vector2[vertLength];
			int[] indices = new int[vertLength * 3 * 2];
			
			Debug.Log ("Vertices size:" + vertices.Length);
			
			Waypoint wp0 = p.last;
			Waypoint wp1 = p.next;
			int vcount = 0;
			for(int mi = 0; mi <= meshDensityValue; ++mi) {
				float lerpamount = ((float)mi) / ((float)meshDensityValue);//TODO:
				
				Debug.Log ("Building vert for|" + mi +":" + vcount + " -> " + (vcount + 2) + " => " + lerpamount);
				
				Vector3 st = p.startTangent;
				Vector3 et = p.endTangent;
				
				Vector3 left   = RoadUtility.BezierLerp(wp0.left, wp1.left, st, st, lerpamount);
				Vector3 middle = Vector3.Lerp(wp0.transform.position, wp1.transform.position, lerpamount);
				Vector3 right  = RoadUtility.BezierLerp(wp0.right, wp1.right, et, et, lerpamount);
				vertices[vcount] = p.transform.InverseTransformPoint(left);
				uv[vcount] = new Vector2(0.0f, lerpamount); //TODO
				++vcount;
				vertices[vcount] = p.transform.InverseTransformPoint(middle);
				float mid = Vector3.Distance(left, middle) / Vector3.Distance(left, right);
				uv[vcount] = new Vector2(mid, lerpamount); //TODO
				++vcount;
				vertices[vcount] = p.transform.InverseTransformPoint(right);
				uv[vcount] = new Vector2(1.0f, lerpamount); //TODO
				++vcount;
				
				if( vcount >= 6 ) {
					int v = vcount-6;
					int c = (v) * 4;
					Debug.Log ("Building index for:" + v + " -> " + (v + 6) + " (" + c + " - " + (c+11) + " )");
					
					indices[c]   = (v + 0) % vertLength;
					indices[c+1] = (v + 4) % vertLength;
					indices[c+2] = (v + 1) % vertLength;
					
					indices[c+3] = (v + 0) % vertLength;
					indices[c+4] = (v + 3) % vertLength;
					indices[c+5] = (v + 4) % vertLength;
					
					indices[c+6] = (v + 1) % vertLength;
					indices[c+7] = (v + 4) % vertLength;
					indices[c+8] = (v + 5) % vertLength;
					
					indices[c+9]  = (v + 1) % vertLength;
					indices[c+10] = (v + 5) % vertLength;
					indices[c+11] = (v + 2) % vertLength;
				}
			}
			
			m.vertices = vertices;
			m.triangles = indices;
			m.uv = uv;
			m.RecalculateBounds();
			m.RecalculateNormals();
		} 

		public static void BuildMeshForRoad(Road r) { 
			Path[] paths = r.gameObject.GetComponentsInChildren<Path>();
			foreach(Path p in paths) {
				BuildMeshForPath(p);
			}			
		} 
	}
}