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

namespace Flowmap {
	public enum FluidDepth {DeepWater, Surface}
}

[AddComponentMenu("Flowmaps/Generator")]
[ExecuteInEditMode]
public class FlowmapGenerator : MonoBehaviour {
			
	public static LayerMask GpuRenderLayer{
		get{
			return LayerMask.NameToLayer ("Default");
		}
	}
	
	public static SimulationPath SimulationPath = SimulationPath.GPU;
	public static bool SupportsGPUPath{
		get{
			#if UNITY_EDITOR
			return (SystemInfo.SupportsRenderTextureFormat (RenderTextureFormat.ARGBHalf) && SystemInfo.supportsRenderTextures && UnityEditorInternal.InternalEditorUtility.HasPro ()) ? true : false;
			#else
			return (SystemInfo.SupportsRenderTextureFormat (RenderTextureFormat.ARGBHalf) && SystemInfo.supportsRenderTextures) ? true : false;
			#endif
		}
	}
		
	static int _threadCount = 1;
	public static int ThreadCount{
		get{
			return _threadCount;
		}
		set{
			_threadCount = value;	
		}
	}
	
	public static RenderTextureFormat GetSingleChannelRTFormat{
		get{
			#if !UNITY_3_5
			return SystemInfo.SupportsRenderTextureFormat (RenderTextureFormat.RFloat) ? RenderTextureFormat.RFloat : RenderTextureFormat.ARGBHalf;
			#else
			return RenderTextureFormat.ARGBHalf;
			#endif
		}
	}
	public static RenderTextureFormat GetTwoChannelRTFormat{
		get{
			#if !UNITY_3_5
			return SystemInfo.SupportsRenderTextureFormat (RenderTextureFormat.RGFloat) ? RenderTextureFormat.RGFloat : RenderTextureFormat.ARGBHalf;
			#else
			return RenderTextureFormat.ARGBHalf;
			#endif
		}
	}
	public static RenderTextureFormat GetFourChannelRTFormat{
		get{
			#if !UNITY_3_5
			return SystemInfo.SupportsRenderTextureFormat (RenderTextureFormat.ARGBFloat) ? RenderTextureFormat.ARGBFloat : RenderTextureFormat.ARGBHalf;
			#else
			return RenderTextureFormat.ARGBHalf;
			#endif
		}
	}
		
	#region common_settings
	[SerializeField]
	private List<FlowSimulationField> fields = new List<FlowSimulationField>();
	public FlowSimulationField[] Fields {
		get {
			CleanNullFields ();
			return fields.ToArray ();
		}
	}
	public bool gpuAcceleration;
	public bool autoAddChildFields = true;
	public int maxThreadCount = 1;
	#endregion
	
	public SimulationPath GetSimulationPath(){
		return (gpuAcceleration && SupportsGPUPath) ? SimulationPath.GPU : SimulationPath.CPU;
	}
	
	[SerializeField]
	Vector2 dimensions = Vector2.one;
	public Vector2 Dimensions{
		get{
			return dimensions;
		}
		set{
			dimensions = value;
		}
	}
	
	private Vector3 cachedPosition;
	/* The transform's position, cached every frame because transform.position isn't thread safe. */
	public Vector3 Position{
		get{
			return cachedPosition;		
		}
	}
	/** Get Format from TextureUtilities.SupportedFormats */
	public int outputFileFormat = 0;
	
	#region components
	private FlowSimulator flowSimulator;
	public FlowSimulator FlowSimulator{
		get{
			if(!flowSimulator){
				flowSimulator = GetComponent<FlowSimulator>();	
			}
			return flowSimulator;
		}
	}
	
	private FlowHeightmap heightmap;
	public FlowHeightmap Heightmap{
		get{
			if(!heightmap){
				heightmap = GetComponent<FlowHeightmap>();
			}
			return heightmap;
		}
	}
	#endregion
	
	void Awake (){
		transform.rotation = Quaternion.identity;
		cachedPosition = transform.position;
		UpdateThreadCount ();		
	}
	
	void Start () {
		UpdateSimulationPath ();
		if(FlowSimulator){
			FlowSimulator.Init ();
			if(FlowSimulator.simulateOnPlay && Application.isPlaying)
				FlowSimulator.StartSimulating ();
		}
	}
	
	public void UpdateSimulationPath (){
		FlowmapGenerator.SimulationPath = GetSimulationPath();	
	}
	public void UpdateThreadCount (){
		_threadCount = maxThreadCount;
	}
	
	void Update (){
		transform.rotation = Quaternion.identity;
		cachedPosition = transform.position;
		if(autoAddChildFields){
			foreach(FlowSimulationField field in GetComponentsInChildren<FlowSimulationField>()){
				AddSimulationField (field);	
			}
		}
	}
	
	public void CleanNullFields (){
		fields.RemoveAll (i=>i == null);	
	}
	
	public void AddSimulationField (FlowSimulationField field){
		if(!fields.Contains (field)){
			fields.Add (field);	
		}
	}
	
	public void ClearAllFields (){
		fields.Clear ();	
	}
	
	void OnDrawGizmos () {
		Gizmos.DrawWireCube (transform.position, new Vector3(Dimensions.x, 0, Dimensions.y));
	}
}