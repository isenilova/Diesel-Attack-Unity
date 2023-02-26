//----------------------------------------------
// Flowmap Generator
// Copyright © 2013 Superposition Games
// http://www.superpositiongames.com
// support@superpositiongames.com
//----------------------------------------------
using UnityEngine;
using System.Collections;
using Flowmap;

[ExecuteInEditMode]
[RequireComponent(typeof(FlowmapGenerator))]
public class FlowHeightmap : MonoBehaviour {

	public virtual Texture HeightmapTexture {get; set;}
	public virtual Texture PreviewHeightmapTexture {get; set;}
	
	public bool previewHeightmap;
	public bool drawPreviewPlane;
	bool wantsToDrawHeightmap;
	
	[SerializeField]
	GameObject previewGameObject;
	[SerializeField]
	Material previewMaterial;
	FlowmapGenerator generator;
	protected FlowmapGenerator Generator{
		get{
			if(!generator)
				generator = GetComponent<FlowmapGenerator>();
			return generator;
		}
	}
	
	protected virtual void OnDrawGizmosSelected (){
		DisplayPreviewHeightmap (true);	
		UpdatePreviewHeightmap ();
	}
	
	public void DisplayPreviewHeightmap (bool state){
		wantsToDrawHeightmap = state;
		UpdatePreviewHeightmap ();
	}
	
	public void UpdatePreviewHeightmap (){
		if(previewGameObject == null || previewMaterial == null){
			Cleanup ();
			previewGameObject = new GameObject("Preview Heightmap");
			previewGameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.NotEditable;
			MeshFilter meshFilter = previewGameObject.AddComponent<MeshFilter>();
			meshFilter.sharedMesh = Primitives.PlaneMesh;
			MeshRenderer meshRenderer = previewGameObject.AddComponent<MeshRenderer>();
			previewMaterial = new Material(Shader.Find ("Hidden/HeightmapFieldPreview"));
			previewMaterial.hideFlags = HideFlags.HideAndDontSave;			
			meshRenderer.sharedMaterial = previewMaterial;
		}
		
		if(previewHeightmap && wantsToDrawHeightmap){
			previewMaterial.SetTexture ("_MainTex", PreviewHeightmapTexture);
			previewMaterial.SetFloat ("_Strength", 1);
			previewGameObject.GetComponent<Renderer>().enabled = true;
			previewGameObject.transform.position = transform.position;
			previewGameObject.transform.localScale = new Vector3(Generator.Dimensions.x, 1, Generator.Dimensions.y);			
		}else{
			previewGameObject.GetComponent<Renderer>().enabled = false;
		}
	}
	
	protected virtual void OnDrawGizmos (){
		DisplayPreviewHeightmap (false);	
		UpdatePreviewHeightmap ();
	}
	
	protected virtual void OnDestroy () {	
		Cleanup ();
	}
	
	void Cleanup (){
		if(previewGameObject){
			if(Application.isPlaying)
				Destroy (previewGameObject);
			else
				DestroyImmediate (previewGameObject);
		}
		if(previewMaterial){
			if(Application.isPlaying)
				Destroy (previewMaterial);
			else
				DestroyImmediate (previewMaterial);
		}	
	}
}
