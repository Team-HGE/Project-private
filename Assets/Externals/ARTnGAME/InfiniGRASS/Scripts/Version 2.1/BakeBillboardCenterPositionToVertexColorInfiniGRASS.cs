using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using Artngame.INfiniDy;

namespace Artngame.INfiniDy {
public class BakeBillboardCenterPositionToVertexColorInfiniGRASS { //v2.1.19 // : MonoBehaviour {

	public MeshFilter meshFilter;
	public bool bakePositions=false;

	// Use this for initialization
	void Start () {
	
	}

	public void bakeBillboardPositions(Mesh mesh){

		Vector3[] vertices = mesh.vertices;
		//Color[] VertColor0 = meshFilter.mesh.colors;
		List<Color> VertColor = new List<Color>();

		//Debug.Log ("vertices="+vertices.Length);
		//Debug.Log ("VertColors="+VertColor.Count);

		for (int i = 0; i < vertices.Length; i=i+4) {
			if (i + 3 < vertices.Length) {
				Vector3 start = new Vector3 (vertices [i].x, vertices [i].y, vertices [i].z);
				Vector3 end = new Vector3 (vertices [i + 2].x, vertices [i + 2].y, vertices [i + 2].z);

				Vector3 halfway = start + ((end - start) / 2);

				Color setColor = new Color (halfway.x,halfway.y,halfway.z,0);//new Color (vertices [i].x, vertices [i].y, vertices [i].z, 0);
				VertColor.Add (setColor);
				VertColor.Add (setColor);
				VertColor.Add (setColor);
				VertColor.Add (setColor);

				//if (i < 8) {
				//Debug.Log ("vertices Pos in " + i + "= " + vertices [i]);
				//Debug.Log ("setColor="+setColor);
				//}
			}
		}

		mesh.SetColors (VertColor);

	}

	// Update is called once per frame
	void Update () {
		if(bakePositions){
			if (meshFilter != null) {

				bakeBillboardPositions (meshFilter.mesh);

				//bake positions
//				Vector3[] vertices = meshFilter.mesh.vertices;
//				//Color[] VertColor0 = meshFilter.mesh.colors;
//				List<Color> VertColor = new List<Color>();
//
//				//Debug.Log ("vertices="+vertices.Length);
//				//Debug.Log ("VertColors="+VertColor.Count);
//
//				for (int i = 0; i < vertices.Length; i=i+4) {
//					if (i + 3 < vertices.Length) {
//						Vector3 start = new Vector3 (vertices [i].x, vertices [i].y, vertices [i].z);
//						Vector3 end = new Vector3 (vertices [i + 2].x, vertices [i + 2].y, vertices [i + 2].z);
//
//						Vector3 halfway = start + ((end - start) / 2);
//
//						Color setColor = new Color (halfway.x,halfway.y,halfway.z,0);//new Color (vertices [i].x, vertices [i].y, vertices [i].z, 0);
//						VertColor.Add (setColor);
//						VertColor.Add (setColor);
//						VertColor.Add (setColor);
//						VertColor.Add (setColor);
//
//						//if (i < 8) {
//							//Debug.Log ("vertices Pos in " + i + "= " + vertices [i]);
//							//Debug.Log ("setColor="+setColor);
//						//}
//					}
//				}
//
//				meshFilter.mesh.SetColors (VertColor);

				bakePositions = false;
			}
		}
	}
}
}
