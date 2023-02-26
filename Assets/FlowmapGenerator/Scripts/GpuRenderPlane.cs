//----------------------------------------------
// Flowmap Generator
// Copyright © 2013 Superposition Games
// http://www.superpositiongames.com
// support@superpositiongames.com
//----------------------------------------------
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
/** A plane for previewing fields and, on the GPU path, rendering them to rendertextures. */
public class GpuRenderPlane : MonoBehaviour {

	public FlowSimulationField field;
	
	/** Check to make sure this plane is correctly connected to a field, if not, delete it. */
	void Update (){
		if(field == null){
			if(Application.isPlaying && gameObject)
				Destroy(gameObject);
			else
				DestroyImmediate (gameObject);
		}else{
			if(field.RenderPlane != this){
				if(Application.isPlaying && gameObject)
					Destroy(gameObject);
				else
					DestroyImmediate (gameObject);
			}
		}
	}
}
