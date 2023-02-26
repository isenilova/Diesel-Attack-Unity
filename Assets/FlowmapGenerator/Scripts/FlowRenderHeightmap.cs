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
[AddComponentMenu("Flowmaps/Heightmap/Render From Scene")]
public class FlowRenderHeightmap : FlowHeightmap {
	
	public static bool Supported{
		get{
			bool supported = SystemInfo.supportsRenderTextures;
			#if UNITY_EDITOR
			if(!UnityEditorInternal.InternalEditorUtility.HasPro ())
				supported = false;
			#endif
			return supported;
		}
	}
	
	public static string UnsupportedReason{
		get{
			string reason = "";
			if(!SystemInfo.supportsRenderTextures){
				reason = "System doesn't support RenderTextures.";	
			}
			#if UNITY_EDITOR
			if(!UnityEditorInternal.InternalEditorUtility.HasPro ())
				reason = "Only supported for Unity Pro.";
			#endif
			return reason;
		}
	}
	
	#region settings
	public int resolutionX = 256;
	public int resolutionY = 256;
	public FluidDepth fluidDepth;
	public float heightMax = 1;
	public float heightMin = 1;
	public LayerMask cullingMask = 1<<0;
	public bool dynamicUpdating;
	#endregion
		
	Camera renderingCamera;
	RenderTexture heightmap;
	public override Texture HeightmapTexture {
		get {
			return heightmap;
		}
		set {
			Debug.LogWarning ("Can't set HeightmapTexture.");
		}
	}
	public override Texture PreviewHeightmapTexture {
		get {
			return HeightmapTexture;
		}
	}
	private Shader ClippedHeightShader{
		get{
			return Shader.Find ("Hidden/DepthToHeightClipped");
		}
	}
	private Shader HeightShader{
		get{
			return Shader.Find ("Hidden/DepthToHeight");
		}
	}
	
	Material compareMaterial;
	Material CompareMaterial{
		get{
			if(!compareMaterial){
				compareMaterial = new Material(Shader.Find ("Hidden/DepthCompare"));
				compareMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return compareMaterial;
		}
	}
	
	Material resizeMaterial;
	Material ResizeMaterial{
		get{
			if(!resizeMaterial){
				resizeMaterial = new Material(Shader.Find ("Hidden/RenderHeightmapResize"));
				resizeMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return resizeMaterial;
		}
	}
	
	void Awake (){
		UpdateHeightmap ();	
	}
	
	public void UpdateHeightmap (){
		UnityEngine.Profiling.Profiler.BeginSample ("Render heightmap");
		if(heightmap == null || heightmap.width != resolutionX || heightmap.height != resolutionY){
			heightmap = new RenderTexture (resolutionX, resolutionY, 24, FlowmapGenerator.GetSingleChannelRTFormat, RenderTextureReadWrite.Linear);
			heightmap.hideFlags = HideFlags.HideAndDontSave;
		}
		
		if(renderingCamera == null){
			renderingCamera = (new GameObject("Render Heightmap", typeof(Camera))).GetComponent<Camera>();
			renderingCamera.gameObject.hideFlags = HideFlags.HideAndDontSave;
			renderingCamera.enabled = false;
			renderingCamera.renderingPath = RenderingPath.Forward;
			renderingCamera.clearFlags = CameraClearFlags.SolidColor;
			renderingCamera.backgroundColor = Color.black;
			renderingCamera.orthographic = true;			
		}
		
		renderingCamera.cullingMask = cullingMask;
		renderingCamera.transform.rotation = Quaternion.identity;
		renderingCamera.orthographicSize = Mathf.Max (Generator.Dimensions.x, Generator.Dimensions.y) * 0.5f;
		renderingCamera.transform.position = Generator.transform.position + Vector3.up * heightMax;
		renderingCamera.transform.rotation = Quaternion.LookRotation (Vector3.down, Vector3.forward);
		
		switch(fluidDepth){
		case FluidDepth.DeepWater:
			RenderTexture overhangMask = RenderTexture.GetTemporary (resolutionX, resolutionY, 24, FlowmapGenerator.GetSingleChannelRTFormat, RenderTextureReadWrite.sRGB);	
			RenderTexture heightBelowSurface = RenderTexture.GetTemporary (resolutionX, resolutionY, 24, FlowmapGenerator.GetSingleChannelRTFormat, RenderTextureReadWrite.sRGB);	
			RenderTexture heightIntersecting = RenderTexture.GetTemporary (resolutionX, resolutionY, 24, FlowmapGenerator.GetSingleChannelRTFormat, RenderTextureReadWrite.sRGB);
								
			Shader.SetGlobalFloat ("_HeightmapRenderDepthMin", Generator.transform.position.y);
			Shader.SetGlobalFloat ("_HeightmapRenderDepthMax", Generator.transform.position.y - heightMin);
			renderingCamera.targetTexture = overhangMask;
			renderingCamera.nearClipPlane = 0.01f;
			renderingCamera.farClipPlane = 100;
			renderingCamera.RenderWithShader (ClippedHeightShader, "RenderType");
			
			Shader.SetGlobalFloat ("_HeightmapRenderDepthMin", Generator.transform.position.y);
			Shader.SetGlobalFloat ("_HeightmapRenderDepthMax", Generator.transform.position.y - heightMin);
			renderingCamera.nearClipPlane = heightMax;
			renderingCamera.farClipPlane = heightMin + heightMax;
			renderingCamera.targetTexture = heightBelowSurface;
			renderingCamera.RenderWithShader (HeightShader, "RenderType");
			
			Shader.SetGlobalFloat ("_HeightmapRenderDepthMin", Generator.transform.position.y);
			Shader.SetGlobalFloat ("_HeightmapRenderDepthMax", Generator.transform.position.y - heightMin);
			renderingCamera.nearClipPlane = 0.01f;
			renderingCamera.farClipPlane = heightMin + heightMax;
			renderingCamera.targetTexture = heightIntersecting;
			renderingCamera.RenderWithShader (HeightShader, "RenderType");
			
			CompareMaterial.SetTexture ("_OverhangMaskTex", overhangMask);
			CompareMaterial.SetTexture ("_HeightBelowSurfaceTex", heightBelowSurface);
			CompareMaterial.SetTexture ("_HeightIntersectingTex", heightIntersecting);
			Graphics.Blit (null, heightmap, CompareMaterial);
			RenderTexture.ReleaseTemporary (overhangMask);
			RenderTexture.ReleaseTemporary (heightBelowSurface);
			RenderTexture.ReleaseTemporary (heightIntersecting);
			break;
		case FluidDepth.Surface:
			Shader.SetGlobalFloat ("_HeightmapRenderDepthMax", Generator.transform.position.y - heightMin);
			Shader.SetGlobalFloat ("_HeightmapRenderDepthMin", Generator.transform.position.y + heightMax);
			renderingCamera.nearClipPlane = 0.001f;
			renderingCamera.farClipPlane = heightMin + heightMax;
			renderingCamera.targetTexture = heightmap;
			renderingCamera.RenderWithShader (HeightShader, "RenderType");
			break;
		}
		
		if(Generator.Dimensions.x != Generator.Dimensions.y){			
			RenderTexture buffer = RenderTexture.GetTemporary (resolutionX, resolutionY, 24, FlowmapGenerator.GetSingleChannelRTFormat, RenderTextureReadWrite.Linear);
			ResizeMaterial.SetTexture ("_Heightmap", heightmap);
			if(Generator.Dimensions.y > Generator.Dimensions.x)
				ResizeMaterial.SetVector ("_AspectRatio", new Vector4((Generator.Dimensions.x / Generator.Dimensions.y), 1, 0,0));
			else
				ResizeMaterial.SetVector ("_AspectRatio", new Vector4(1, 1/(Generator.Dimensions.x / Generator.Dimensions.y), 0,0));
			Graphics.Blit (null, buffer, ResizeMaterial, 0);
			Graphics.Blit (buffer, heightmap);
			RenderTexture.ReleaseTemporary (buffer);
		}
		
		UnityEngine.Profiling.Profiler.EndSample ();
	}
	
	protected override void OnDrawGizmosSelected (){
		base.OnDrawGizmosSelected ();
		
		Gizmos.DrawWireCube (transform.position + Vector3.up * (heightMax - heightMin) / 2,
			new Vector3(Generator.Dimensions.x, (heightMax + heightMin), Generator.Dimensions.y));	
	}
}
