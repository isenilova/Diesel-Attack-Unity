//----------------------------------------------
// Flowmap Generator
// Copyright © 2013 Superposition Games
// http://www.superpositiongames.com
// support@superpositiongames.com
//----------------------------------------------
using UnityEngine;
using System.Collections;
using Flowmap;

namespace Flowmap{
	public enum FluidForce { Attract, Repulse, VortexCounterClockwise, VortexClockwise, Directional, Calm }
}

[AddComponentMenu("Flowmaps/Fields/Force")]
public class FlowForceField : FlowSimulationField {
		
	#region common_settings
	public FluidForce force;
	#endregion
	
	#region GpuPath_members
	/** This texture contains vectors which are splatted to the force render texture. */
	[SerializeField] private Texture2D vectorTexture;
	#endregion
	
	private Vector2 vectorTextureDimensions;
	private Color[] vectorTexturePixels;
	private Vector3 cachedForwardVector;
	
	#region editor_preview_textures
	[HideInInspector]
	public Texture2D attractVectorPreview;
	[HideInInspector]
	public Texture2D repulseVectorPreview;
	[HideInInspector]
	public Texture2D vortexClockwiseVectorPreview;
	[HideInInspector]
	public Texture2D vortexCounterClockwiseVectorPreview;
	[HideInInspector]
	public Texture2D directionalVectorPreview;
	#endregion	
	
	public override FieldPass Pass {
		get {
			return FieldPass.Force;
		}
	}
	
	protected override Shader RenderShader {
		get {
			return Shader.Find ("Hidden/ForceFieldPreview");
		}
	}
	
	public override void Init ()
	{
		base.Init ();
		UpdateVectorTexture ();
	}
	
	protected override void Update ()
	{
		base.Update ();
		
//		some force types shouldn't be rotated, the vector texture doesn't update for those
//		FIX scaling can create situations where you'd want to scale
//		switch(force){
//		case FluidForce.Attract:
//		case FluidForce.Repulse:
//		case FluidForce.VortexClockwise:
//		case FluidForce.VortexCounterClockwise:
//			transform.rotation = Quaternion.identity;
//			break;
//		}
	}
	
	/** Called when the force mode changes to a mode that uses a different vector texture. */
	public void UpdateVectorTexture ()
	{
		int resolutionX = 64;
		int resolutionY = 64;
		vectorTexture = new Texture2D(resolutionX, resolutionY, TextureFormat.ARGB32, false, true);
		vectorTexture.hideFlags = HideFlags.HideAndDontSave;
		vectorTexture.name = "VectorTexture";
		Color[] colors = new Color[resolutionX * resolutionY];
		for(int y = 0; y<resolutionY; y++){
			for(int x = 0; x<resolutionX; x++){
				Vector2 vector = Vector2.zero;
				float falloff = 1-Mathf.Clamp01(vector.magnitude);
				Color pixelColor = Color.white;
				
				switch(force){
				case FluidForce.Repulse:
				case FluidForce.Attract:
					vector = new Vector2( ((x/(float)resolutionX) - 0.5f) * 2, ((y/(float)resolutionY) - 0.5f) * 2 );
					vector = vector.normalized;
					vector = new Vector2(vector.x * 0.5f + 0.5f, vector.y * 0.5f + 0.5f);
					pixelColor = new Color(vector.x, vector.y, 0, falloff);
					break;
				case FluidForce.VortexClockwise:
				case FluidForce.VortexCounterClockwise:
					vector = new Vector2( ((x/(float)resolutionX) - 0.5f) * 2, ((y/(float)resolutionY) - 0.5f) * 2 );
					vector = vector.normalized;
					Vector3 crossVector = Vector3.Cross (new Vector3(vector.x, 0, vector.y), Vector3.down);
					vector = new Vector2(crossVector.x * 0.5f + 0.5f, crossVector.z * 0.5f + 0.5f);
					pixelColor = new Color(vector.x, vector.y, 0, falloff);
					break;
				case FluidForce.Directional:
					vector = Vector2.one;
					pixelColor = new Color(vector.x, vector.y, 0, falloff);
					break;
				case FluidForce.Calm:
					pixelColor = new Color(0.5f, 0.5f, 1, falloff);
					break;
				}
				colors[x + y * resolutionX] = pixelColor;
			}
		}
		vectorTexture.SetPixels (colors);
		vectorTexture.Apply (false);
		vectorTexturePixels = vectorTexture.GetPixels ();
		vectorTextureDimensions = new Vector2(vectorTexture.width, vectorTexture.height);
	}
	
	public override void UpdateRenderPlane ()
	{
		base.UpdateRenderPlane ();
		
		switch(FlowmapGenerator.SimulationPath){
		case SimulationPath.GPU:
	//		update offscreen render
			FalloffMaterial.SetTexture ("_VectorTex", vectorTexture);
//			modify vectors, attract is an inverted repulse, counter clockwise vortex is an inverted clockwise and directional takes the forward axis of the field
			switch(force){
			case FluidForce.Repulse:
				FalloffMaterial.SetVector ("_VectorScale", Vector2.one);
				FalloffMaterial.SetFloat ("_VectorInvert", 0);
				break;
			case FluidForce.Attract:
				FalloffMaterial.SetVector ("_VectorScale", Vector2.one);
				FalloffMaterial.SetFloat ("_VectorInvert", 1);
				break;
			case FluidForce.VortexClockwise:
				FalloffMaterial.SetVector ("_VectorScale", Vector2.one);
				FalloffMaterial.SetFloat ("_VectorInvert", 0);
				break;
			case FluidForce.VortexCounterClockwise:
				FalloffMaterial.SetVector ("_VectorScale", Vector2.one);
				FalloffMaterial.SetFloat ("_VectorInvert", 1);
				break;
			case FluidForce.Directional:
				Vector2 directionalVector = new Vector2(transform.forward.x * 0.5f + 0.5f, transform.forward.z * 0.5f + 0.5f);
				FalloffMaterial.SetVector ("_VectorScale", directionalVector);
				FalloffMaterial.SetFloat ("_VectorInvert", 0);
				break;
			case FluidForce.Calm:
				FalloffMaterial.SetVector ("_VectorScale", Vector2.one);
				FalloffMaterial.SetFloat ("_VectorInvert", 0);
				break;
			}
			break;
		}
		
//		update preview material
		if((wantsToDrawPreviewTexture || DrawFalloffUnselected) && enabled){
			switch(force){
			case FluidForce.Attract:
				FalloffMaterial.SetTexture ("_VectorPreviewTex", attractVectorPreview);
				break;
			case FluidForce.Repulse:
				FalloffMaterial.SetTexture ("_VectorPreviewTex", repulseVectorPreview);
				break;
			case FluidForce.VortexClockwise:
				FalloffMaterial.SetTexture ("_VectorPreviewTex", vortexClockwiseVectorPreview);
				break;
			case FluidForce.VortexCounterClockwise:
				FalloffMaterial.SetTexture ("_VectorPreviewTex", vortexCounterClockwiseVectorPreview);
				break;
			case FluidForce.Directional:
				FalloffMaterial.SetTexture ("_VectorPreviewTex", directionalVectorPreview);
				break;
			}
		}else{
			FalloffMaterial.SetTexture ("_VectorPreviewTex", null);
		}
	}
	
	public override void TickStart ()
	{
		base.TickStart ();
		switch(FlowmapGenerator.SimulationPath){
		case SimulationPath.CPU:
			if(vectorTexturePixels == null)
				Init ();
//			cache for threaded operations
			cachedForwardVector = transform.forward;
			break;
		}
	}
	
	public Vector3 GetForceCpu (FlowmapGenerator generator, Vector2 uv){
		Vector2 samplePos = TransformSampleUv (generator, uv, (force == FluidForce.Attract || force == FluidForce.VortexCounterClockwise));		
		Color directionColor = (FlowmapGenerator.ThreadCount > 1) ? TextureUtilities.SampleColorBilinear (vectorTexturePixels, (int)vectorTextureDimensions.x, (int)vectorTextureDimensions.y, samplePos.x, samplePos.y) :
			vectorTexture.GetPixelBilinear (samplePos.x, samplePos.y);
		if(force == FluidForce.Directional){
			directionColor = new Color(cachedForwardVector.x * 0.5f + 0.5f, cachedForwardVector.z * 0.5f + 0.5f, 0,0);
		}
		Vector3 vector = new Vector3(directionColor.r * 2 - 1, directionColor.g * 2 - 1, directionColor.b);
		return strength * vector * ((samplePos.x >= 0 && samplePos.x <= 1 && samplePos.y >= 0 && samplePos.y <= 1) ? 1 : 0) * (hasFalloffTexture ? 
			((FlowmapGenerator.ThreadCount > 1) ? TextureUtilities.SampleColorBilinear (falloffTexturePixels, (int)falloffTextureDimensions.x, (int)falloffTextureDimensions.y, samplePos.x, samplePos.y).r :
			falloffTexture.GetPixelBilinear (samplePos.x, samplePos.y).r) : 1);
	}
	
	protected override void Cleaup ()
	{
		base.Cleaup ();
		if(vectorTexture)
		{
			if(Application.isPlaying)
				Destroy (vectorTexture);
			else
				DestroyImmediate (vectorTexture);
		}
	}
}
