//----------------------------------------------
// Flowmap Generator
// Copyright © 2013 Superposition Games
// http://www.superpositiongames.com
// support@superpositiongames.com
//----------------------------------------------
using UnityEngine;
using System.Collections;

namespace Flowmap{
	public static class Primitives {
	
		static Mesh planeMesh;
		/** Creates a shared plane mesh. */
		public static Mesh PlaneMesh{
			get{
				if(!planeMesh) {
					planeMesh = new Mesh();
					planeMesh.name = "Plane";
					planeMesh.vertices = new Vector3[] {new Vector3(-0.5f,0,-0.5f), new Vector3(0.5f,0,-0.5f), new Vector3(-0.5f,0,0.5f), new Vector3(0.5f,0,0.5f)};
					planeMesh.uv = new Vector2[] {new Vector2(0,0), new Vector2(1,0), new Vector2(0,1), new Vector2(1,1)};
					planeMesh.normals = new Vector3[] {Vector3.up, Vector3.up, Vector3.up, Vector3.up};
					planeMesh.triangles = new int[] {2,1,0, 3,1,2};
					planeMesh.tangents = new Vector4[]{new Vector4(1,0,0,1), new Vector4(1,0,0,1), new Vector4(1,0,0,1), new Vector4(1,0,0,1)};
					planeMesh.hideFlags = HideFlags.HideAndDontSave;
				}
				return planeMesh;
			}
		}
	}
}