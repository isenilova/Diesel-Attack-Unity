//----------------------------------------------
// Flowmap Generator
// Copyright © 2013 Superposition Games
// http://www.superpositiongames.com
// support@superpositiongames.com
//----------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Flowmap;
using System.Linq;

namespace Flowmap{
	/** Sets which pass the field is applied to. Add and remove are added first. Force is added when calculating the fluid flow. */
	public enum FieldPass { AddFluid, RemoveFluid, Force, Heightmap }	
}

[ExecuteInEditMode]
public class FlowSimulationField : MonoBehaviour {
	
	public static bool DrawFalloffTextures = true;
	public static bool DrawFalloffUnselected = false;
	
	public virtual FieldPass Pass{get{ return FieldPass.Force;}}
		
	#region common_settings
	public float strength = 1;
	public Texture2D falloffTexture;
	#endregion
	
	protected Transform cachedTransform;
	/* Threadsafe position. */
	protected Vector3 cachedPosition;
	/* Threadsafe rotation. */
	protected Quaternion cachedRotation;
	/* Threadsafe lossy scale. */
	protected Vector3 cachedScale;
	/* Threadsafe falloff texture width and height. */
	protected Vector2 falloffTextureDimensions;
	/* Threadsafe falloff texture pixels. */
	protected Color[] falloffTexturePixels;
	
	#region status_members
	private bool initialized;
	protected bool wantsToDrawPreviewTexture;
	protected bool hasFalloffTexture;
	#endregion
	
	#region GPUpath
	protected virtual Shader RenderShader{
		get{
			return null;
		}
	}
	
	private Material falloffMaterial;
	public Material FalloffMaterial{
		get{
			if(!falloffMaterial){				
				falloffMaterial = new Material(RenderShader);
				falloffMaterial.hideFlags = HideFlags.HideAndDontSave;
				falloffMaterial.name = "FlowFieldFalloff";
			}
			if(falloffMaterial.shader != RenderShader)
				falloffMaterial.shader = RenderShader;
			return falloffMaterial;
		}
	}
	[SerializeField]
	[HideInInspector]
	protected GpuRenderPlane renderPlane;
	public GpuRenderPlane RenderPlane{
		get{
			return renderPlane;
		}
	}
	
	protected void CreateMesh () {
		if(renderPlane && renderPlane.gameObject){
			if(Application.isPlaying)
				Destroy (renderPlane.gameObject);	
			else
				DestroyImmediate (renderPlane.gameObject);
		}
		if(this == null)
			return;
		if(renderPlane == null){
			GameObject renderPlaneGO = new GameObject(name +" render plane");
			renderPlaneGO.hideFlags = HideFlags.HideInHierarchy;
			renderPlaneGO.layer = FlowmapGenerator.GpuRenderLayer;
			renderPlane = renderPlaneGO.AddComponent<GpuRenderPlane>();
			renderPlane.field = this;
		}
		MeshFilter filter = renderPlane.GetComponent<MeshFilter>();
		if(!filter) {
			filter = renderPlane.gameObject.AddComponent<MeshFilter>();
		}
		filter.sharedMesh = Primitives.PlaneMesh;
		MeshRenderer meshRenderer = renderPlane.GetComponent<MeshRenderer>();
		if(!meshRenderer) {
			meshRenderer = renderPlane.gameObject.AddComponent<MeshRenderer>();
		}
		meshRenderer.material = FalloffMaterial;
		meshRenderer.enabled = false;
		#if UNITY_EDITOR
		UnityEditor.EditorUtility.SetSelectedWireframeHidden (meshRenderer, false);	
		#endif
	}
	#endregion
		
	void Awake (){
		Init ();
	}
		
	protected virtual void Update (){
		if(!initialized)
			Init ();
		if(Application.isPlaying)
			UpdateRenderPlane ();
	}
	
	public void DisableRenderPlane (){
		if(renderPlane)
			renderPlane.GetComponent<Renderer>().enabled = false;
	}
	
	public void DrawFalloffTextureEnabled (bool state){
		wantsToDrawPreviewTexture = state;	
	}
	
	/** Make sure the render plane is in the correct position and has the correct falloff texture. (GPU path only) */
	public virtual void UpdateRenderPlane ()
	{
		if(renderPlane == null || renderPlane.field != this){
			CreateMesh ();	
		}
//		Workaround: Hidden gameobjects crash unity if they're the child of a non-hidden object
		renderPlane.transform.position = transform.position;
		renderPlane.transform.localScale = transform.lossyScale;
		renderPlane.transform.rotation = transform.rotation;
		FalloffMaterial.SetTexture ("_MainTex", falloffTexture);
		FalloffMaterial.SetFloat ("_Strength", strength);
		renderPlane.GetComponent<Renderer>().enabled = DrawFalloffTextures && (wantsToDrawPreviewTexture || DrawFalloffUnselected) && enabled;
	}
	
	public virtual void Init (){
		if(initialized)
			return;
		cachedTransform = transform;
		CreateMesh ();
		renderPlane.GetComponent<Renderer>().enabled = wantsToDrawPreviewTexture;
		
		cachedTransform = transform;
		cachedPosition = cachedTransform.position;
		cachedRotation = cachedTransform.rotation;
		cachedScale = cachedTransform.lossyScale;
		hasFalloffTexture = falloffTexture != null;
		if(falloffTexture){
			falloffTextureDimensions = new Vector2(falloffTexture.width, falloffTexture.height);
			falloffTexturePixels = falloffTexture.GetPixels ();	
		}
		else
			falloffTextureDimensions = Vector2.zero;
		
		initialized = true;
	}
	
	/** Called by the simulator before running the field pass that this field belongs to. */
	public virtual void TickStart () {
		if(!enabled)
			return;
		switch(FlowmapGenerator.SimulationPath) {
		case SimulationPath.GPU:
			UpdateRenderPlane ();
			FalloffMaterial.SetFloat ("_Renderable", 1);
			renderPlane.GetComponent<Renderer>().enabled = true;
			break;
		case SimulationPath.CPU:
//			cache stuff for threaded operations
			cachedTransform = transform;
			cachedPosition = cachedTransform.position;
			cachedRotation = cachedTransform.rotation;
			cachedScale = cachedTransform.lossyScale;
			hasFalloffTexture = falloffTexture != null;
			if(falloffTexture){
				falloffTextureDimensions = new Vector2(falloffTexture.width, falloffTexture.height);
				falloffTexturePixels = falloffTexture.GetPixels ();	
			}
			else
				falloffTextureDimensions = Vector2.zero;
			break;
		}
	}
	
	/** Called by the simulator right after running the field pass that this field belongs to. */
	public virtual void TickEnd () {
		switch(FlowmapGenerator.SimulationPath) {
		case SimulationPath.GPU:
			UpdateRenderPlane ();
			FalloffMaterial.SetFloat ("_Renderable", 0);
			break;
		}
	}
	
	#region CPU_path
	public Vector2 GetUvScale (FlowmapGenerator generator){
		return new Vector2(cachedScale.x/generator.Dimensions.x, cachedScale.z/generator.Dimensions.y);
	}
	
	public Vector2 GetUvTransform (FlowmapGenerator generator){
		return new Vector2((generator.Position.x-cachedPosition.x) / generator.Dimensions.x, (generator.Position.z-cachedPosition.z)  / generator.Dimensions.y);
	}
	
	public float GetUvRotation (FlowmapGenerator generator){
		return cachedRotation.eulerAngles.y * Mathf.Deg2Rad;
	}
	
	public float GetStrengthCpu (FlowmapGenerator generator, Vector2 uv){
		Vector2 samplePos = TransformSampleUv (generator, uv, false);
		float currentStrength = strength;
		if(samplePos.x < 0 || samplePos.x > 1 || samplePos.y < 0 || samplePos.y > 1){
			currentStrength = 0;	
		}
		if(FlowmapGenerator.ThreadCount > 1){
			return currentStrength * (hasFalloffTexture ? TextureUtilities.SampleColorBilinear (falloffTexturePixels, (int)falloffTextureDimensions.x, (int)falloffTextureDimensions.y, samplePos.x, samplePos.y).r : 1);
		}else{
			return currentStrength * (hasFalloffTexture ? falloffTexture.GetPixelBilinear (samplePos.x, samplePos.y).r : 1);
		}
	}
	
	protected Vector2 TransformSampleUv (FlowmapGenerator generator, Vector2 uv, bool invertY){
		Vector2 samplePos = uv;
		samplePos = new Vector2(samplePos.x + GetUvTransform (generator).x, samplePos.y + GetUvTransform (generator).y);
		samplePos -= Vector2.one * 0.5f;
		samplePos = new Vector2(samplePos.x * Mathf.Cos (GetUvRotation (generator)) - samplePos.y * Mathf.Sin(GetUvRotation (generator)), samplePos.x * Mathf.Sin (GetUvRotation (generator)) + samplePos.y * Mathf.Cos(GetUvRotation (generator)));
		samplePos = new Vector2(samplePos.x / GetUvScale (generator).x * (invertY ? -1 : 1), samplePos.y / GetUvScale (generator).y * (invertY ? -1 : 1));
		samplePos += Vector2.one * 0.5f;
		return samplePos;
	}
	#endregion
	
	protected virtual void OnDrawGizmosSelected () {
		Vector3 bottomLeft = cachedTransform.position + cachedTransform.right * (-cachedTransform.lossyScale.x/2f) + cachedTransform.forward * (-cachedTransform.lossyScale.z/2f);
		Vector3 bottomRight = cachedTransform.position + cachedTransform.right * (cachedTransform.lossyScale.x/2f) + cachedTransform.forward * (-cachedTransform.lossyScale.z/2f);
		Vector3 topLeft = cachedTransform.position + cachedTransform.right * (-cachedTransform.lossyScale.x/2f) + cachedTransform.forward * (cachedTransform.lossyScale.z/2f);
		Vector3 topRight = cachedTransform.position + cachedTransform.right * (cachedTransform.lossyScale.x/2f) + cachedTransform.forward * (cachedTransform.lossyScale.z/2f);
		Gizmos.DrawLine (bottomLeft, bottomRight);
		Gizmos.DrawLine (topLeft, topRight);
		Gizmos.DrawLine (bottomLeft, topLeft);
		Gizmos.DrawLine (bottomRight, topRight);
		wantsToDrawPreviewTexture = true;
		UpdateRenderPlane ();
	}
	
	protected virtual void OnDrawGizmos () {
		wantsToDrawPreviewTexture = false;
		UpdateRenderPlane ();
	}
	
	void OnDisable (){
		wantsToDrawPreviewTexture = false;
		if(renderPlane)
			renderPlane.GetComponent<Renderer>().enabled = DrawFalloffTextures && wantsToDrawPreviewTexture;
	}
	
	void OnDestroy (){
		Cleaup ();
	}
	
	protected virtual void Cleaup (){
//		since the render plane isn't parented it needs to be destroyed separately
		if(renderPlane && renderPlane.gameObject){
			if(Application.isPlaying)
				Destroy (renderPlane.gameObject);
			else
				DestroyImmediate (renderPlane.gameObject);
		}
		if(falloffMaterial){
			if(Application.isPlaying)
				Destroy (falloffMaterial);
			else
				DestroyImmediate (falloffMaterial);
		}
	}
}