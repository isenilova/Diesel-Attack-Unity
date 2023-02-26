using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class SetCameraDepth : MonoBehaviour {
	
	[SerializeField]
	DepthTextureMode depthMode;
	void Update () {
		GetComponent<Camera>().depthTextureMode = depthMode;
	}
}
