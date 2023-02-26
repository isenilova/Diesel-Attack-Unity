//----------------------------------------------
// Flowmap Generator
// Copyright © 2013 Superposition Games
// http://www.superpositiongames.com
// support@superpositiongames.com
//----------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Flowmap;

namespace Flowmap{
	/** A helper class for sending info to CPU multithreaded methods. */
	class ArrayThreadedInfo {
		public int start;
		public int length;
		public ManualResetEvent resetEvent;
		public ArrayThreadedInfo (int start, int length, ManualResetEvent resetEvent){
			this.start = start;
			this.length = length;
			this.resetEvent = resetEvent;
		}
	}
	/** A helper class for sending info to CPU multithreaded methods that need a list of fields and a generator. */
	class ThreadedFieldBakeInfo : ArrayThreadedInfo {
		public FlowSimulationField[] fields;
		public FlowmapGenerator generator;
		public ThreadedFieldBakeInfo (int start, int length, ManualResetEvent resetEvent, FlowSimulationField[] fields, FlowmapGenerator generator) : base (start, length, resetEvent){
			this.fields = fields;
			this.generator = generator;
		}
	}
	
	/** A struct for storing all data for the CPU simulation path. */
	[System.Serializable]
	struct SimulationData{
		public float height;
		public float fluid;
		public float addFluid;
		public float removeFluid;
		public Vector3 force;
		public Vector4 outflow;
		public Vector2 velocity;
		public Vector3 velocityAccumulated;
	}
	
	public delegate void VoidEvent ();
}

/** A simulator that uses the shallow water method of simulating fluids. The cheapest simulation type and works fine for most situations. */
[AddComponentMenu("Flowmaps/Simulators/Shallow Water")]
[ExecuteInEditMode]
public class ShallowWaterSimulator : FlowSimulator {

	public enum OutputTexture { Flowmap, HeightAndFluid, Foam}
			
	#region simulation_settings
	public int updateTextureDelayCPU = 10;
	/** Sets the timestep for each simulation tick. Timesteps higher than ~0.4 tend to explode. */
	public float timestep = 0.4f;
	/** Remove a bit of fluid from the simulation per tick. */
	public float evaporationRate = 0.001f;
	public float gravity = 1;
	public float velocityScale = 1;
	/** A global multiplier for all fluid add fields. */
	public float fluidAddMultiplier = 0.01f;
	/** A global multiplier for all fluid remove fields. */
	public float fluidRemoveMultiplier = 0.01f;
	public float fluidForceMultiplier = 0.01f;
	public float initialFluidAmount;
	public FluidDepth fluidDepth;
	#endregion
	
	#region output_settings
	/** A value close to 1 follows the current simulation values closely while a smaller value takes an average of the simulation.
	  * For baking static flowmaps this value should be fairly low to avoid short lived changes in velocity which may look out of place.*/
	public float outputAccumulationRate = 0.02f;
	int outputFilterSize = 1;
	public float outputFilterStrength = 1f;
	/** Calculates where foam would arise based on fluid speed and the heightmap. Foam accumulates in slower moving areas. */
	public bool simulateFoam;
	public float foamVelocityScale = 1;
	/** Records the first time the fluid reaches a point in the container. Can be used for animating the flowmap's contribution. */
	public bool simulateFirstFluidHit;
	/** The first fluid hit is divided by this value (in seconds). Set it to be the last possible time recorded. */
	public float firstFluidHitTimeMax = 30;
	public Material[] assignFlowmapToMaterials;
	public bool assignFlowmap;
	public string assignedFlowmapName = "_FlowmapTex";
	public bool assignHeightAndFluid;
	public string assignedHeightAndFluidName = "_HeightFluidTex";
	public bool assignUVScaleTransform;
	public string assignUVCoordsName = "_FlowmapUV";
	public bool writeHeightAndFluid;
	public bool writeFoamSeparately;
	public bool writeFluidDepthInAlpha;
	#endregion
	
	#region GPU_members
	RenderTexture heightFluidRT;
	RenderTexture heightPreviewRT;
	RenderTexture fluidPreviewRT;
	RenderTexture fluidAddRT;
	RenderTexture fluidRemoveRT;
	RenderTexture fluidForceRT;
	RenderTexture heightmapFieldsRT;
	RenderTexture outflowRT;
	RenderTexture bufferRT1;
	RenderTexture velocityRT;
	RenderTexture velocityAccumulatedRT;
	Material simulationMaterial;
	Material SimulationMaterial{
		get{
			if(!simulationMaterial)
			{
				simulationMaterial = new Material(Shader.Find ("Hidden/ShallowWaterFlowmapSimulator"));
				simulationMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return simulationMaterial;
		}
	}
	
	/** A camera for rendering all fields to their respective input textures. */
	Camera fieldRenderCamera;
	bool initializedGpu;
	#endregion
	
	#region CPU_members	
	[HideInInspector]
	[SerializeField]
	SimulationData[][] simulationDataCpu;
	Texture2D heightFluidCpu;
	Texture2D velocityAccumulatedCpu;
	bool initializedCpu;
	#endregion
	
	#region debugOutput
	public Texture HeightFluidTexture{
		get{
			if(FlowmapGenerator.SimulationPath == SimulationPath.CPU)
				return heightFluidCpu;
			else
				return heightFluidRT;
		}
	}
//	public Texture HeightPreviewTexture{
//		get{
//			if(FlowmapGenerator.SimulationPath == SimulationPath.CPU)
//				return heightFluidCpu;
//			else
//				return heightPreviewRT;
//		}
//	}
//	public Texture FluidPreviewTexture {
//		get{
//			if(FlowmapGenerator.SimulationPath == SimulationPath.CPU)
//				return heightFluidCpu;
//			else
//				return fluidPreviewRT;
//		}
//	}
	public Texture VelocityAccumulatedTexture{
		get{
			if(FlowmapGenerator.SimulationPath == SimulationPath.CPU && velocityAccumulatedCpu)
				return velocityAccumulatedCpu;
			else if(velocityAccumulatedRT)
				return velocityAccumulatedRT;
			else
				return null;
		}
	}
//	public Texture FoamPreviewTexture {
//		get{
//			if(FlowmapGenerator.SimulationPath == SimulationPath.CPU)
//				return heightFluidCpu;
//			else
//				return heightFluidPreviewRT;
//		}
//	}
	#endregion
	
	public event VoidEvent OnRenderTextureReset;
	public event VoidEvent OnMaxStepsReached;
	
	public override void Init ()
	{
		base.Init ();
		Cleanup ();
		switch(FlowmapGenerator.SimulationPath)
		{
		case SimulationPath.GPU:
			UnityEngine.Profiling.Profiler.BeginSample ("init gpu");
			heightFluidRT = new RenderTexture(resolutionX, resolutionY, 0, FlowmapGenerator.GetTwoChannelRTFormat, RenderTextureReadWrite.Linear);
			heightFluidRT.hideFlags = HideFlags.HideAndDontSave;
			heightFluidRT.name = "HeightFluid";
			heightFluidRT.Create ();
			
			fluidAddRT = new RenderTexture(resolutionX, resolutionY, 0, FlowmapGenerator.GetSingleChannelRTFormat, RenderTextureReadWrite.Linear);
			fluidAddRT.hideFlags = HideFlags.HideAndDontSave;
			fluidAddRT.name = "FluidAdd";
			fluidAddRT.Create ();
			
			fluidRemoveRT = new RenderTexture(resolutionX, resolutionY, 0, FlowmapGenerator.GetSingleChannelRTFormat, RenderTextureReadWrite.Linear);
			fluidRemoveRT.hideFlags = HideFlags.HideAndDontSave;
			fluidRemoveRT.name = "FluidRemove";
			fluidRemoveRT.Create ();
			
			fluidForceRT = new RenderTexture(resolutionX, resolutionY, 0, FlowmapGenerator.GetFourChannelRTFormat, RenderTextureReadWrite.Linear);
			fluidForceRT.hideFlags = HideFlags.HideAndDontSave;
			fluidForceRT.name = "FluidForce";
			fluidForceRT.Create ();
			
			outflowRT = new RenderTexture(resolutionX, resolutionY, 0, FlowmapGenerator.GetFourChannelRTFormat, RenderTextureReadWrite.Linear);
			outflowRT.hideFlags = HideFlags.HideAndDontSave;
			outflowRT.name = "Outflow";
			outflowRT.Create ();
			
			velocityRT = new RenderTexture(resolutionX, resolutionY, 0, FlowmapGenerator.GetTwoChannelRTFormat, RenderTextureReadWrite.Linear);
			velocityRT.hideFlags = HideFlags.HideAndDontSave;
			velocityRT.name = "Velocity";
			velocityRT.Create ();
			
			velocityAccumulatedRT = new RenderTexture(resolutionX, resolutionY, 0, FlowmapGenerator.GetFourChannelRTFormat, RenderTextureReadWrite.Linear);
			velocityAccumulatedRT.hideFlags = HideFlags.HideAndDontSave;
			velocityAccumulatedRT.name = "VelocityAccumulated";
			velocityAccumulatedRT.Create ();
			
			bufferRT1 = new RenderTexture(resolutionX, resolutionY, 0, FlowmapGenerator.GetFourChannelRTFormat, RenderTextureReadWrite.Linear);
			bufferRT1.hideFlags = HideFlags.HideAndDontSave;
			bufferRT1.name = "Buffer1";
			bufferRT1.Create ();
			
			fieldRenderCamera = (new GameObject("Field Renderer", typeof(Camera))).GetComponent<Camera>();
			fieldRenderCamera.gameObject.hideFlags = HideFlags.HideAndDontSave;
			fieldRenderCamera.orthographic = true;
			fieldRenderCamera.orthographicSize = Mathf.Max (Generator.Dimensions.x, Generator.Dimensions.y) * 0.5f;
			fieldRenderCamera.renderingPath = RenderingPath.Forward;
			fieldRenderCamera.cullingMask = 1<<FlowmapGenerator.GpuRenderLayer.value;
			fieldRenderCamera.clearFlags = CameraClearFlags.Color;
			fieldRenderCamera.backgroundColor = Color.black;
			fieldRenderCamera.enabled = false;
			ResetGPUData();
			initializedGpu = true;
			UnityEngine.Profiling.Profiler.EndSample ();
			break;
		case SimulationPath.CPU:
			UnityEngine.Profiling.Profiler.BeginSample ("init cpu");
			ResetCpuData ();
			BakeFieldsCpu ();
			for(int x = 0; x<resolutionX; x++){
				for(int y = 0; y<resolutionY; y++){
//					if deep water, no fluid where height is greater than 0, if not deep water just add the same amount of fluid everywhere
					simulationDataCpu[x][y].fluid = (fluidDepth == FluidDepth.DeepWater ? (1-Mathf.Ceil (simulationDataCpu[x][y].height)) * initialFluidAmount : initialFluidAmount);
				}
			}
			initializedCpu = true;
			UnityEngine.Profiling.Profiler.EndSample ();
			break;
		}
		
		if(Generator.Heightmap is FlowRenderHeightmap){
			(Generator.Heightmap as FlowRenderHeightmap).UpdateHeightmap ();
		}
	}
	
	void DestroyProperly (Object obj){
		if(Application.isEditor || !Application.isPlaying)
			DestroyImmediate (obj);
		else if(Application.isPlaying && !Application.isEditor)
			Destroy (obj);
	}
	
	void Cleanup (){
//			cleanup gpu
		RenderTexture.active = null;
		if(heightFluidRT)
			DestroyProperly (heightFluidRT);
		if(fluidAddRT)
			DestroyProperly (fluidAddRT);
		if(fluidRemoveRT)
			DestroyProperly (fluidRemoveRT);
		if(fluidForceRT)
			DestroyProperly (fluidForceRT);
		if(outflowRT)
			DestroyProperly (outflowRT);
		if(velocityRT)
			DestroyProperly (velocityRT);
		if(velocityAccumulatedRT)
			DestroyProperly (velocityAccumulatedRT);
		if(bufferRT1)
			DestroyProperly (bufferRT1);
		if(fieldRenderCamera)
			DestroyProperly (fieldRenderCamera.gameObject);
		if(simulationMaterial)
			DestroyProperly (simulationMaterial);			
		initializedGpu = false;
//			cleaup cpu
		simulationDataCpu = null;
		if(heightFluidCpu)
			DestroyProperly (heightFluidCpu);
		if(velocityAccumulatedCpu)
			DestroyProperly (heightFluidCpu);
		initializedCpu = false;
	}
	
	void OnDestroy (){
		Cleanup ();
	}
	
	public override void Reset ()
	{
		Init ();
		base.Reset ();			
		AssignToMaterials();
	}
	
	void ResetGPUData (){
//		fill with default values
		SimulationMaterial.SetColor ("_Color", new Color(0.5f, 0.5f, 0,1));
		Graphics.Blit (null, velocityRT, SimulationMaterial, 4);
		Graphics.Blit (null, velocityAccumulatedRT, SimulationMaterial, 4);		
		Graphics.Blit (null, fluidForceRT, SimulationMaterial, 4);
		SimulationMaterial.SetColor ("_Color", Color.black);
		Graphics.Blit (null, fluidAddRT, SimulationMaterial, 4);
		Graphics.Blit (null, fluidRemoveRT, SimulationMaterial, 4);
		Graphics.Blit (null, bufferRT1, SimulationMaterial, 4);	
		SimulationMaterial.SetColor ("_Color", new Color(0,0,0,0));
		Graphics.Blit (null, outflowRT, SimulationMaterial, 4);	
//		copy heightmap
		if(Generator.Heightmap){
			Graphics.Blit (Generator.Heightmap.HeightmapTexture, heightFluidRT, SimulationMaterial, 6);
		}else{
			SimulationMaterial.SetColor ("_Color", new Color(0,0,0,0));
			Graphics.Blit (null, heightFluidRT, SimulationMaterial, 4);	
		}
		if(initialFluidAmount > 0){
			SimulationMaterial.SetFloat ("_DeepWater", (fluidDepth == FluidDepth.DeepWater ? 1 : 0));
			SimulationMaterial.SetFloat ("_FluidAmount", initialFluidAmount);
			Graphics.Blit (heightFluidRT, bufferRT1, SimulationMaterial, 14);
			Graphics.Blit (bufferRT1, heightFluidRT);
		}
	}
	
	void ResetCpuData (){
		UnityEngine.Profiling.Profiler.BeginSample ("reset cpu data");
		if(simulationDataCpu == null)
			simulationDataCpu = new SimulationData[resolutionX][];
		for(int x = 0; x<resolutionX; x++){
			if(simulationDataCpu[x] == null)
				simulationDataCpu[x] = new SimulationData[resolutionY];
			for(int y = 0; y<resolutionY; y++){
				simulationDataCpu[x][y] = new SimulationData();
				simulationDataCpu[x][y].velocity = new Vector3(0.5f, 0.5f, 0);
				simulationDataCpu[x][y].velocityAccumulated = new Vector3(0.5f, 0.5f, 0);
			}
		}
		UnityEngine.Profiling.Profiler.EndSample ();
	}
	
	public override void StartSimulating ()
	{
		base.StartSimulating ();
		switch(FlowmapGenerator.SimulationPath){
		case SimulationPath.CPU:
			if(simulationDataCpu == null)
				initializedCpu = false;
			if(!initializedCpu)
				Init ();
			break;
		case SimulationPath.GPU:
			if(!initializedGpu)
				Init ();
			break;
		}
	}
	
	public override void Tick ()
	{
		base.Tick ();
		if(!Simulating)
			return;
				
		switch(FlowmapGenerator.SimulationPath)
		{
		case SimulationPath.GPU:
			
//			make sure render textures haven't been lost
			if(!heightFluidRT.IsCreated () || !outflowRT.IsCreated () || !velocityRT.IsCreated () || !velocityAccumulatedRT.IsCreated ()){
				if(OnRenderTextureReset != null)
					OnRenderTextureReset();
				Init ();
			}
			
			UnityEngine.Profiling.Profiler.BeginSample ("tick gpu");
			float fieldMaxY = Generator.transform.position.y;
			float fieldMinY = Generator.transform.position.y;
			for(int i=0; i<Generator.Fields.Length; i++){
				fieldMaxY = Mathf.Max (fieldMaxY, Generator.Fields[i].transform.position.y);
				fieldMinY = Mathf.Min (fieldMinY, Generator.Fields[i].transform.position.y);
			}
			fieldRenderCamera.transform.localPosition = Generator.transform.position;
			fieldRenderCamera.transform.position = new Vector3(fieldRenderCamera.transform.position.x, fieldMaxY+1, fieldRenderCamera.transform.position.z);
			fieldRenderCamera.farClipPlane = (fieldMaxY - fieldMinY) + 2;
			fieldRenderCamera.transform.rotation = Quaternion.LookRotation (Vector3.down, Vector3.forward);				
			SimulationMaterial.SetVector ("_Resolution", new Vector4(resolutionX, resolutionY, 0, 0));
			
			SimulationMaterial.SetFloat ("_Timestep", timestep);
			SimulationMaterial.SetFloat ("_Gravity", gravity);
			SimulationMaterial.SetFloat ("_VelocityScale", velocityScale);
						
//			bake add fluid texture
			foreach(FlowSimulationField field in Generator.Fields){
				if(field.Pass == FieldPass.AddFluid){
					field.TickStart ();
				}
			}
			fieldRenderCamera.backgroundColor = Color.black;
			fieldRenderCamera.targetTexture = fluidAddRT;
			fieldRenderCamera.RenderWithShader (Shader.Find("Hidden/FlowmapOffscreenRender"), "Offscreen");
			foreach(FlowSimulationField field in Generator.Fields){
				if(field.Pass == FieldPass.AddFluid){
					field.TickEnd ();
				}
			}
//			bake remove fluid texture
			foreach(FlowSimulationField field in Generator.Fields){
				if(field.Pass == FieldPass.RemoveFluid){
					field.TickStart ();
				}
			}
			fieldRenderCamera.backgroundColor = Color.black;
			fieldRenderCamera.targetTexture = fluidRemoveRT;
			fieldRenderCamera.RenderWithShader (Shader.Find("Hidden/FlowmapOffscreenRender"), "Offscreen");
			foreach(FlowSimulationField field in Generator.Fields){
				if(field.Pass == FieldPass.RemoveFluid){
					field.TickEnd ();
				}
			}
//			bake force field texture
			foreach(FlowSimulationField field in Generator.Fields){
				if(field.Pass == FieldPass.Force){
					field.TickStart ();
				}
			}
			fieldRenderCamera.backgroundColor = new Color(Mathf.LinearToGammaSpace (0.5f), Mathf.LinearToGammaSpace (0.5f), 0,1);				
			fieldRenderCamera.targetTexture = fluidForceRT;
			fieldRenderCamera.RenderWithShader (Shader.Find("Hidden/FlowmapOffscreenRender"), "Offscreen");
			foreach(FlowSimulationField field in Generator.Fields){
				if(field.Pass == FieldPass.Force){
					field.TickEnd ();
				}
			}
			UnityEngine.Profiling.Profiler.EndSample ();
			
//			render heightmap from scene
			if(Generator.Heightmap && Generator.Heightmap is FlowRenderHeightmap && (Generator.Heightmap as FlowRenderHeightmap).dynamicUpdating){
				(Generator.Heightmap as FlowRenderHeightmap).UpdateHeightmap ();
			}
//			update height from heightmap texture
			if(Generator.Heightmap){
				if(Generator.Heightmap is FlowTextureHeightmap && (Generator.Heightmap as FlowTextureHeightmap).isRaw){
					SimulationMaterial.SetFloat ("_IsFloatRGBA", 1);
				}else{
					SimulationMaterial.SetFloat ("_IsFloatRGBA", 0);
				}
				SimulationMaterial.SetTexture ("_NewHeightTex", Generator.Heightmap.HeightmapTexture);
				Graphics.Blit (heightFluidRT, bufferRT1, SimulationMaterial, 9);
				Graphics.Blit (bufferRT1, heightFluidRT);
			}
			
//			bake heightmap fields and add to heightmap
			foreach(FlowSimulationField field in Generator.Fields){
				if(field.Pass == FieldPass.Heightmap){
					field.TickStart ();
				}
			}
			fieldRenderCamera.backgroundColor = Color.black;
			RenderTexture heightFieldsRT = RenderTexture.GetTemporary (resolutionX, resolutionY, 0, FlowmapGenerator.GetSingleChannelRTFormat, RenderTextureReadWrite.Linear);
			fieldRenderCamera.targetTexture = heightFieldsRT;
			fieldRenderCamera.RenderWithShader (Shader.Find("Hidden/FlowmapOffscreenRender"), "Offscreen");
			foreach(FlowSimulationField field in Generator.Fields){
				if(field.Pass == FieldPass.Heightmap){
					field.TickEnd ();
				}
			}
			SimulationMaterial.SetTexture ("_HeightmapFieldsTex", heightFieldsRT);
			Graphics.Blit (heightFluidRT, bufferRT1, SimulationMaterial, 11);
			Graphics.Blit (bufferRT1, heightFluidRT);
			RenderTexture.ReleaseTemporary (heightFieldsRT);
			
//			inflow/outflow and evaporation
			SimulationMaterial.SetTexture ("_FluidAddTex", fluidAddRT);
			SimulationMaterial.SetTexture ("_FluidRemoveTex", fluidRemoveRT);
			SimulationMaterial.SetFloat ("_Evaporation", evaporationRate);
			SimulationMaterial.SetFloat ("_FluidAddMultiplier", fluidAddMultiplier);
			SimulationMaterial.SetFloat ("_FluidRemoveMultiplier", fluidRemoveMultiplier);
			Graphics.Blit (heightFluidRT, bufferRT1, SimulationMaterial, 0);
			Graphics.Blit (bufferRT1, heightFluidRT);
			
//			update outflow
			SimulationMaterial.SetTexture ("_FluidForceTex", fluidForceRT);
			SimulationMaterial.SetFloat ("_FluidForceMultiplier", fluidForceMultiplier);
			SimulationMaterial.SetTexture ("_OutflowTex", outflowRT);
			SimulationMaterial.SetTexture ("_VelocityTex", velocityRT);
			SimulationMaterial.SetFloat ("_BorderCollision", (borderCollision == SimulationBorderCollision.Collide) ? 1 : 0);
			Graphics.Blit (heightFluidRT, bufferRT1, SimulationMaterial, 1);
			Graphics.Blit (bufferRT1, outflowRT);
			
//			update fluid amount
			SimulationMaterial.SetTexture ("_OutflowTex", outflowRT);				
			Graphics.Blit (heightFluidRT, bufferRT1, SimulationMaterial, 2);
			Graphics.Blit (bufferRT1, heightFluidRT);
			
//			update velocity
			SimulationMaterial.SetTexture ("_OutflowTex", outflowRT);
			Graphics.Blit (heightFluidRT, bufferRT1, SimulationMaterial, 3);
			Graphics.Blit (bufferRT1, velocityRT);
			
//			accumulate velocity
			SimulationMaterial.SetFloat ("_Delta", outputAccumulationRate);
			SimulationMaterial.SetTexture ("_VelocityTex", velocityRT);
			SimulationMaterial.SetTexture ("_VelocityAccumTex", velocityAccumulatedRT);
			Graphics.Blit (heightFluidRT, bufferRT1, SimulationMaterial, 5);
			Graphics.Blit (bufferRT1, velocityAccumulatedRT);
			
//			foam accumulation
			if(simulateFoam){
				SimulationMaterial.SetFloat ("_Delta", outputAccumulationRate);
				SimulationMaterial.SetTexture ("_FluidAddTex", fluidAddRT);
				SimulationMaterial.SetTexture ("_VelocityAccumTex", velocityAccumulatedRT);
				SimulationMaterial.SetFloat ("_FoamVelocityScale", foamVelocityScale);
				Graphics.Blit (heightFluidRT, bufferRT1, SimulationMaterial, 10);
				Graphics.Blit (bufferRT1, velocityAccumulatedRT);
			}
//			write fluid depth
			if(writeFluidDepthInAlpha){
				SimulationMaterial.SetTexture ("_HeightFluidTex", heightFluidRT);
				Graphics.Blit (velocityAccumulatedRT, bufferRT1, SimulationMaterial, 15);	
				Graphics.Blit (bufferRT1, velocityAccumulatedRT);
			}
			
			if(outputFilterStrength > 0){
//				blur
				SimulationMaterial.SetFloat ("_BlurSpread", outputFilterSize);
				SimulationMaterial.SetFloat ("_Strength", outputFilterStrength);
				Graphics.Blit (velocityAccumulatedRT, bufferRT1, SimulationMaterial, 7);
				Graphics.Blit (bufferRT1, velocityAccumulatedRT, SimulationMaterial, 8);
			}
			
			break;
		case SimulationPath.CPU:
			UnityEngine.Profiling.Profiler.BeginSample ("tick cpu");
			
			if(FlowmapGenerator.ThreadCount > 1){
				int chunkLength = Mathf.CeilToInt (resolutionX/(float)FlowmapGenerator.ThreadCount);
				ManualResetEvent[] resetEvents = new ManualResetEvent[FlowmapGenerator.ThreadCount];
				ArrayThreadedInfo[] threadedInfo = new ArrayThreadedInfo[FlowmapGenerator.ThreadCount];
				for(int i=0; i<FlowmapGenerator.ThreadCount; i++){
					resetEvents[i] = new ManualResetEvent(false);
					threadedInfo[i] = new ArrayThreadedInfo(0,0,null);
				}
				
//				add/remove fluid
				for(int i=0; i<FlowmapGenerator.ThreadCount; i++){
					resetEvents[i].Reset ();
					threadedInfo[i].start = i * chunkLength;
					threadedInfo[i].length = (i==FlowmapGenerator.ThreadCount-1) ? ((resolutionX-1) - i*chunkLength) : chunkLength;
					threadedInfo[i].resetEvent = resetEvents[i];
					ThreadPool.QueueUserWorkItem (AddRemoveFluidThreaded, threadedInfo[i]);
				}
				WaitHandle.WaitAll (resetEvents);
				
//				outflow
				for(int i=0; i<FlowmapGenerator.ThreadCount; i++){
					int start = i * chunkLength;
					int length = (i==FlowmapGenerator.ThreadCount-1) ? ((resolutionX-1) - i*chunkLength) : chunkLength;
					resetEvents[i] = new ManualResetEvent(false);
					ThreadPool.QueueUserWorkItem (OutflowThreaded, new ArrayThreadedInfo(start, length, resetEvents[i]));
				}
				WaitHandle.WaitAll (resetEvents);
				
//				update velocity
				for(int i=0; i<FlowmapGenerator.ThreadCount; i++){
					int start = i * chunkLength;
					int length = (i==FlowmapGenerator.ThreadCount-1) ? ((resolutionX-1) - i*chunkLength) : chunkLength;
					resetEvents[i] = new ManualResetEvent(false);
					ThreadPool.QueueUserWorkItem (UpdateVelocityThreaded, new ArrayThreadedInfo(start, length, resetEvents[i]));
				}
				WaitHandle.WaitAll (resetEvents);
				
//				foam
				if(simulateFoam){
					for(int i=0; i<FlowmapGenerator.ThreadCount; i++){
						int start = i * chunkLength;
						int length = (i==FlowmapGenerator.ThreadCount-1) ? ((resolutionX-1) - i*chunkLength) : chunkLength;
						resetEvents[i] = new ManualResetEvent(false);
						ThreadPool.QueueUserWorkItem (FoamThreaded, new ArrayThreadedInfo(start, length, resetEvents[i]));
					}
					WaitHandle.WaitAll (resetEvents);
				}
				
				if(outputFilterStrength > 0){
	//				blur horizontal
					for(int i=0; i<FlowmapGenerator.ThreadCount; i++){
						int start = i * chunkLength;
						int length = (i==FlowmapGenerator.ThreadCount-1) ? ((resolutionX-1) - i*chunkLength) : chunkLength;
						resetEvents[i] = new ManualResetEvent(false);
						ThreadPool.QueueUserWorkItem (BlurVelocityAccumulatedHorizontalThreaded, new ArrayThreadedInfo(start, length, resetEvents[i]));
					}
					WaitHandle.WaitAll (resetEvents);
	//				blur vertical
					for(int i=0; i<FlowmapGenerator.ThreadCount; i++){
						int start = i * chunkLength;
						int length = (i==FlowmapGenerator.ThreadCount-1) ? ((resolutionX-1) - i*chunkLength) : chunkLength;
						resetEvents[i] = new ManualResetEvent(false);
						ThreadPool.QueueUserWorkItem (BlurVelocityAccumulatedVerticalThreaded, new ArrayThreadedInfo(start, length, resetEvents[i]));
					}
					WaitHandle.WaitAll (resetEvents);
				}
			}else{
				UnityEngine.Profiling.Profiler.BeginSample ("add/remove fluid");
//				add/remove fluid
				for(int x = 0; x<resolutionX; x++){
					for(int y = 0; y<resolutionY; y++){
						AddRemoveFluidCpu (x,y);
					}
				}
				UnityEngine.Profiling.Profiler.EndSample ();
				UnityEngine.Profiling.Profiler.BeginSample ("outflow");
//				outflow
				for(int x = 0; x<resolutionX; x++){
					for(int y = 0; y<resolutionY; y++){
						OutflowCpu (x, y);
					}
				}
				UnityEngine.Profiling.Profiler.EndSample ();
				UnityEngine.Profiling.Profiler.BeginSample ("velocity");
//				update velocity
				for(int x = 0; x<resolutionX; x++){
					for(int y = 0; y<resolutionY; y++){
						UpdateVelocityCpu (x, y);
					}
				}
				UnityEngine.Profiling.Profiler.EndSample ();
				UnityEngine.Profiling.Profiler.BeginSample ("foam");
//				foam
				if(simulateFoam){
					for(int x = 0; x<resolutionX; x++){
						for(int y = 0; y<resolutionY; y++){	
							FoamCpu (x, y);
						}
					}
				}
				UnityEngine.Profiling.Profiler.EndSample ();
				UnityEngine.Profiling.Profiler.BeginSample ("blur");
				if(outputFilterStrength > 0){
	//				blur accumulated velocity horizontal
					for(int x = 0; x<resolutionX; x++){
						for(int y = 0; y<resolutionY; y++){
							BlurVelocityAccumulatedHorizontalCpu (x, y);
						}
					}
	//				blur accumulated velocity vertical
					for(int x = 0; x<resolutionX; x++){
						for(int y = 0; y<resolutionY; y++){
							BlurVelocityAccumulatedVerticalCpu (x, y);
						}
					}
				}
				UnityEngine.Profiling.Profiler.EndSample ();				
			}
			UnityEngine.Profiling.Profiler.BeginSample ("write texture");
			if(SimulationStepsCount % updateTextureDelayCPU == 0){
				WriteCpuDataToTexture ();
			}
			UnityEngine.Profiling.Profiler.EndSample ();
			UnityEngine.Profiling.Profiler.EndSample();
			break;
		}
	}		
	
	#region CPU_simulationMethods
	void BakeFieldsCpu ()
	{
//		copy heightmap
		if(Generator.Heightmap){
			Texture2D heightTexture = null;
			bool isRaw = false;
			bool createdFromRT = false;
			
			if(Generator.Heightmap is FlowTextureHeightmap && ((Generator.Heightmap as FlowTextureHeightmap).HeightmapTexture as Texture2D) != null){
				heightTexture = (Generator.Heightmap as FlowTextureHeightmap).HeightmapTexture as Texture2D;
				isRaw = (Generator.Heightmap as FlowTextureHeightmap).isRaw;
			}else if(Generator.Heightmap is FlowRenderHeightmap && FlowRenderHeightmap.Supported){
//				need to blit to an LDR render texture, can't read from it otherwise
				(generator.Heightmap as FlowRenderHeightmap).UpdateHeightmap ();
				RenderTexture heightmapRT = (generator.Heightmap as FlowRenderHeightmap).HeightmapTexture as RenderTexture;
				RenderTexture heighmapRTLow = RenderTexture.GetTemporary (heightmapRT.width, heightmapRT.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
				Graphics.Blit (heightmapRT, heighmapRTLow);
				heightTexture = new Texture2D(heightmapRT.width, heightmapRT.height);
				heightTexture.hideFlags = HideFlags.HideAndDontSave;
				RenderTexture.active = heighmapRTLow;
				heightTexture.ReadPixels (new Rect(0,0,heightTexture.width, heightTexture.height), 0,0);
				heightTexture.Apply ();
				createdFromRT = true;
				RenderTexture.ReleaseTemporary (heighmapRTLow);
			}
			if(heightTexture != null){
				Color[] heights = heightTexture.GetPixels ();
				for(int x = 0; x<resolutionX; x++){	
					for(int y = 0; y<resolutionY; y++){
						if(isRaw){
							simulationDataCpu[x][y].height = TextureUtilities.DecodeFloatRGBA (TextureUtilities.SampleColorBilinear (heights, heightTexture.width, heightTexture.height, x/(float)resolutionX, y/(float)resolutionY));
						}else{
							simulationDataCpu[x][y].height = TextureUtilities.SampleColorBilinear (heights, heightTexture.width, heightTexture.height, x/(float)resolutionX, y/(float)resolutionY).r;
						}
					}
				}
				if(createdFromRT){
					if(Application.isPlaying)
						Destroy(heightTexture);
					else
						DestroyImmediate (heightTexture);
				}
			}
		}
		UnityEngine.Profiling.Profiler.BeginSample ("bake textures cpu");
		if(FlowmapGenerator.ThreadCount > 1){
			int chunkLength = Mathf.CeilToInt (resolutionX/(float)FlowmapGenerator.ThreadCount);
			ManualResetEvent[] resetEvents = new ManualResetEvent[FlowmapGenerator.ThreadCount];
			
//			bake fluid add
			UnityEngine.Profiling.Profiler.BeginSample ("add fluid cpu");
			FlowSimulationField[] addFields = Generator.Fields.Where (f=>f is FluidAddField && f.enabled).ToArray ();
			foreach(FlowSimulationField field in addFields){
				field.TickStart ();	
			}
			for(int i=0; i<FlowmapGenerator.ThreadCount; i++){
				int start = i * chunkLength;
				int length = (i==FlowmapGenerator.ThreadCount-1) ? ((resolutionX-1) - i*chunkLength) : chunkLength;
				resetEvents[i] = new ManualResetEvent(false);
				ThreadPool.QueueUserWorkItem (BakeAddFluidThreaded, new ThreadedFieldBakeInfo(start, length, resetEvents[i], addFields, Generator));
			}
			WaitHandle.WaitAll (resetEvents);
			foreach(FlowSimulationField field in addFields){
				field.TickEnd ();	
			}
			UnityEngine.Profiling.Profiler.EndSample ();
//			bake fluid remove
			FlowSimulationField[] removeFields = Generator.Fields.Where (f=>f is FluidRemoveField && f.enabled).ToArray ();
			foreach(FlowSimulationField field in removeFields){
				field.TickStart ();	
			}
			for(int i=0; i<FlowmapGenerator.ThreadCount; i++){
				int start = i * chunkLength;
				int length = (i==FlowmapGenerator.ThreadCount-1) ? ((resolutionX-1) - i*chunkLength) : chunkLength;
				resetEvents[i] = new ManualResetEvent(false);
				ThreadPool.QueueUserWorkItem (BakeRemoveFluidThreaded, new ThreadedFieldBakeInfo(start, length, resetEvents[i], removeFields, Generator));
			}
			WaitHandle.WaitAll (resetEvents);
			foreach(FlowSimulationField field in removeFields){
				field.TickEnd ();	
			}
//			bake forces
			FlowSimulationField[] forceFields = Generator.Fields.Where (f=>f is FlowForceField && f.enabled).ToArray ();
			foreach(FlowSimulationField field in forceFields){
				field.TickStart ();	
			}
			for(int i=0; i<FlowmapGenerator.ThreadCount; i++){
				int start = i * chunkLength;
				int length = (i==FlowmapGenerator.ThreadCount-1) ? ((resolutionX-1) - i*chunkLength) : chunkLength;
				resetEvents[i] = new ManualResetEvent(false);
				ThreadPool.QueueUserWorkItem (BakeForcesThreaded, new ThreadedFieldBakeInfo(start, length, resetEvents[i], forceFields, Generator));
			}
			WaitHandle.WaitAll (resetEvents);
			foreach(FlowSimulationField field in forceFields){
				field.TickEnd ();	
			}
//				bake heightmap
			FlowSimulationField[] heightmapFields = Generator.Fields.Where (f=>f is HeightmapField && f.enabled).ToArray ();
			foreach(FlowSimulationField field in heightmapFields){
				field.TickStart ();	
			}
			for(int i=0; i<FlowmapGenerator.ThreadCount; i++){
				int start = i * chunkLength;
				int length = (i==FlowmapGenerator.ThreadCount-1) ? ((resolutionX-1) - i*chunkLength) : chunkLength;
				resetEvents[i] = new ManualResetEvent(false);
				ThreadPool.QueueUserWorkItem (BakeHeightmapThreaded, new ThreadedFieldBakeInfo(start, length, resetEvents[i], heightmapFields, Generator));
			}
			WaitHandle.WaitAll (resetEvents);
			foreach(FlowSimulationField field in heightmapFields){
				field.TickEnd ();	
			}
		}else{
//			bake add fluid
			FlowSimulationField[] addFields = Generator.Fields.Where (f=>f is FluidAddField && f.enabled).ToArray ();
			foreach(FlowSimulationField field in addFields){
				field.TickStart ();	
			}
			for(int x = 0; x<resolutionX; x++){
				for(int y = 0; y<resolutionY; y++){
					foreach(FlowSimulationField field in addFields){
						simulationDataCpu[x][y].addFluid += field.GetStrengthCpu (Generator, new Vector2(x/(float)resolutionX, y/(float)resolutionY));
					}
				}
			}
			foreach(FlowSimulationField field in addFields){
				field.TickEnd ();	
			}
//			bake fluid remove
			FlowSimulationField[] removeFields = Generator.Fields.Where (f=>f is FluidRemoveField && f.enabled).ToArray ();
			foreach(FlowSimulationField field in removeFields){
				field.TickStart ();	
			}
			for(int x = 0; x<resolutionX; x++){
				for(int y = 0; y<resolutionY; y++){
					foreach(FlowSimulationField field in removeFields){
						simulationDataCpu[x][y].removeFluid += field.GetStrengthCpu (Generator, new Vector2(x/(float)resolutionX, y/(float)resolutionY));
					}
				}
			}
			foreach(FlowSimulationField field in removeFields){
				field.TickEnd ();	
			}
//			bake forces
			FlowSimulationField[] forceFields = Generator.Fields.Where (f=>f is FlowForceField && f.enabled).ToArray ();
			foreach(FlowSimulationField field in forceFields){
				field.TickStart ();	
			}
			for(int x = 0; x<resolutionX; x++){
				for(int y = 0; y<resolutionY; y++){
					foreach(FlowSimulationField field in forceFields){
						simulationDataCpu[x][y].force += (field as FlowForceField).GetForceCpu (Generator, new Vector2(x/(float)resolutionX, y/(float)resolutionY));
						simulationDataCpu[x][y].force.z = Mathf.Max (simulationDataCpu[x][y].force.z, 0);
					}
				}
			}
			foreach(FlowSimulationField field in forceFields){
				field.TickEnd ();	
			}
//			bake heightmap fields
			FlowSimulationField[] heightmapFields = Generator.Fields.Where (f=>f is HeightmapField && f.enabled).ToArray ();
			foreach(FlowSimulationField field in heightmapFields){
				field.TickStart ();	
			}
			for(int x = 0; x<resolutionX; x++){
				for(int y = 0; y<resolutionY; y++){
					foreach(FlowSimulationField field in heightmapFields){
						float strength = field.GetStrengthCpu (Generator, new Vector2(x/(float)resolutionX, y/(float)resolutionY));
						simulationDataCpu[x][y].height = Mathf.Lerp (simulationDataCpu[x][y].height, strength, strength * (1-simulationDataCpu[x][y].height));
					}
				}
			}
			foreach(FlowSimulationField field in heightmapFields){
				field.TickEnd ();	
			}
		}
		WriteCpuDataToTexture ();
		UnityEngine.Profiling.Profiler.EndSample ();
	}
	
	void AddRemoveFluidCpu (int x, int y)
	{
		simulationDataCpu[x][y].fluid += simulationDataCpu[x][y].addFluid * timestep * fluidAddMultiplier;
		simulationDataCpu[x][y].fluid = Mathf.Max(0, simulationDataCpu[x][y].fluid - simulationDataCpu[x][y].removeFluid * fluidRemoveMultiplier);
		simulationDataCpu[x][y].fluid = simulationDataCpu[x][y].fluid * (1 - evaporationRate * timestep);
	}
	
	void OutflowCpu (int x, int y)
	{
		int neighbourN = Mathf.Min(y+1, resolutionY-1);
		int neighbourE = Mathf.Min(x+1, resolutionX-1);
		int neighbourS = Mathf.Max(y-1, 0);
		int neighbourW = Mathf.Max(x-1, 0);
		Vector2 forceDirection = new Vector2(simulationDataCpu[x][y].force.x, simulationDataCpu[x][y].force.y);					
		float outflowN = Mathf.Max (0, simulationDataCpu[x][y].outflow.x + timestep * gravity * (simulationDataCpu[x][y].height + simulationDataCpu[x][y].fluid
			- simulationDataCpu[x][neighbourN].height - simulationDataCpu[x][neighbourN].fluid) + Mathf.Clamp01 (Vector2.Dot (forceDirection, new Vector2(0,1))) * timestep * fluidForceMultiplier);
		float outflowE = Mathf.Max (0, simulationDataCpu[x][y].outflow.y + timestep * gravity * (simulationDataCpu[x][y].height + simulationDataCpu[x][y].fluid
			- simulationDataCpu[neighbourE][y].height - simulationDataCpu[neighbourE][y].fluid) + Mathf.Clamp01 (Vector2.Dot (forceDirection, new Vector2(1,0))) * timestep * fluidForceMultiplier);
		float outflowS = Mathf.Max (0, simulationDataCpu[x][y].outflow.z + timestep * gravity * (simulationDataCpu[x][y].height + simulationDataCpu[x][y].fluid
			- simulationDataCpu[x][neighbourS].height - simulationDataCpu[x][neighbourS].fluid) + Mathf.Clamp01 (Vector2.Dot (forceDirection, new Vector2(0,-1))) * timestep * fluidForceMultiplier);
		float outflowW = Mathf.Max (0, simulationDataCpu[x][y].outflow.w + timestep * gravity * (simulationDataCpu[x][y].height + simulationDataCpu[x][y].fluid
			- simulationDataCpu[neighbourW][y].height - simulationDataCpu[neighbourW][y].fluid) + Mathf.Clamp01 (Vector2.Dot (forceDirection, new Vector2(-1,0))) * timestep * fluidForceMultiplier);
		if(borderCollision == SimulationBorderCollision.PassThrough){
			if(x==0)
				outflowE = 0;
			if(x==resolutionX-1)
				outflowW = 0;
			if(y==0)
				outflowN = 0;
			if(y==resolutionY-1)
				outflowS = 0;
		}
		float outflowK = (outflowN + outflowE + outflowS + outflowW > 0) ?  Mathf.Min(1, simulationDataCpu[x][y].fluid / (timestep * (outflowN + outflowE + outflowS + outflowW))) : 0;
		outflowK *= 1-simulationDataCpu[x][y].force.z;
		simulationDataCpu[x][y].outflow = new Vector4(outflowN * outflowK, outflowE * outflowK, outflowS * outflowK, outflowW * outflowK);
	}

	void UpdateVelocityCpu (int x, int y)
	{
		int neighbourN = Mathf.Min(y+1, resolutionY-1);
		int neighbourE = Mathf.Min(x+1, resolutionX-1);
		int neighbourS = Mathf.Max(y-1, 0);
		int neighbourW = Mathf.Max(x-1, 0);
		
		float inflowNorth = simulationDataCpu[x][neighbourN].outflow.z;
		float inflowEast = simulationDataCpu[neighbourE][y].outflow.w;
		float inflowSouth = simulationDataCpu[x][neighbourS].outflow.x;
		float inflowWest = simulationDataCpu[neighbourW][y].outflow.y;
		
		float flowDelta = timestep * ((inflowNorth + inflowEast + inflowSouth + inflowWest) - (simulationDataCpu[x][y].outflow.x + simulationDataCpu[x][y].outflow.y + simulationDataCpu[x][y].outflow.z + simulationDataCpu[x][y].outflow.w));
		simulationDataCpu[x][y].fluid = simulationDataCpu[x][y].fluid + flowDelta;
		float flowDeltaX = 0.5f * (inflowWest-inflowEast + (simulationDataCpu[x][y].outflow.y - simulationDataCpu[x][y].outflow.w));
		float flowDeltaY = 0.5f * (inflowSouth-inflowNorth + (simulationDataCpu[x][y].outflow.x - simulationDataCpu[x][y].outflow.z));
		float fluidDelta = 0.5f * (simulationDataCpu[x][y].fluid + (simulationDataCpu[x][y].fluid + flowDelta));
		Vector2 velocity = Vector2.zero;
		if(fluidDelta != 0){
			velocity.x = (flowDeltaX / fluidDelta);	
			velocity.y = (flowDeltaY / fluidDelta);
		}
		velocity.x = Mathf.Clamp(velocity.x * velocityScale, -1, 1) * 0.5f + 0.5f;
		velocity.y = Mathf.Clamp(velocity.y * velocityScale, -1, 1) * 0.5f + 0.5f;
		simulationDataCpu[x][y].velocity = velocity;
		float foam = simulationDataCpu[x][y].velocityAccumulated.z;
		simulationDataCpu[x][y].velocityAccumulated = Vector3.Lerp (simulationDataCpu[x][y].velocityAccumulated, simulationDataCpu[x][y].velocity, outputAccumulationRate);
		simulationDataCpu[x][y].velocityAccumulated.z = foam;
	}
	
	void BlurVelocityAccumulatedHorizontalCpu (int x, int y)
	{
		Vector3 velocityLeft = simulationDataCpu[(int)Mathf.Max (0, x-1)][y].velocityAccumulated;
		Vector3 velocityRight = simulationDataCpu[(int)Mathf.Min (resolutionX-1, x+1)][y].velocityAccumulated;
		simulationDataCpu[x][y].velocityAccumulated = Vector3.Lerp (simulationDataCpu[x][y].velocityAccumulated , velocityLeft * 0.25f + simulationDataCpu[x][y].velocityAccumulated * 0.5f
			+ velocityRight * 0.25f, outputFilterStrength);
	}
	
	void BlurVelocityAccumulatedVerticalCpu (int x, int y)
	{
		Vector3 velocityDown = simulationDataCpu[x][(int)Mathf.Max (0, y-1)].velocityAccumulated;
		Vector3 velocityUp = simulationDataCpu[x][(int)Mathf.Min (resolutionY-1, y+1)].velocityAccumulated;
		simulationDataCpu[x][y].velocityAccumulated = Vector3.Lerp (simulationDataCpu[x][y].velocityAccumulated , velocityDown * 0.25f + simulationDataCpu[x][y].velocityAccumulated * 0.5f
			+ velocityUp * 0.25f, outputFilterStrength);
	}

	void FoamCpu (int x, int y)
	{
		int neighbourN = Mathf.Min(y+1, resolutionY-1);
		int neighbourE = Mathf.Min(x+1, resolutionX-1);
		int neighbourS = Mathf.Max(y-1, 0);
		int neighbourW = Mathf.Max(x-1, 0);
		float velocityMagnitude = new Vector2(simulationDataCpu[x][y].velocityAccumulated.x * 2 - 1, simulationDataCpu[x][y].velocityAccumulated.y * 2 - 1).magnitude;
		float velocityMagnitudeN = new Vector2(simulationDataCpu[x][neighbourN].velocityAccumulated.x * 2 - 1, simulationDataCpu[x][neighbourN].velocityAccumulated.y * 2 - 1).magnitude;
		float velocityMagnitudeE = new Vector2(simulationDataCpu[neighbourE][y].velocityAccumulated.x * 2 - 1, simulationDataCpu[neighbourE][y].velocityAccumulated.y * 2 - 1).magnitude;
		float velocityMagnitudeS = new Vector2(simulationDataCpu[x][neighbourS].velocityAccumulated.x * 2 - 1, simulationDataCpu[x][neighbourS].velocityAccumulated.y * 2 - 1).magnitude;
		float velocityMagnitudeW = new Vector2(simulationDataCpu[neighbourW][y].velocityAccumulated.x * 2 - 1, simulationDataCpu[neighbourW][y].velocityAccumulated.y * 2 - 1).magnitude;
		float velocityDelta = 100 * ((velocityMagnitudeN - velocityMagnitude) + (velocityMagnitudeE - velocityMagnitude) + (velocityMagnitudeS - velocityMagnitude) + (velocityMagnitudeW - velocityMagnitude));
		float foam = Mathf.Pow (1-Mathf.Clamp01 (new Vector2 ((simulationDataCpu[x][y].velocity.x * 2 - 1), (simulationDataCpu[x][y].velocity.y * 2 - 1)).magnitude * foamVelocityScale), 2);
		foam *= 1 - simulationDataCpu[x][y].addFluid;
		foam = (Mathf.Clamp01((foam * 1.2f - 0.5f) * 4) + 0.5f) * Mathf.Clamp01(velocityDelta);
		simulationDataCpu[x][y].velocityAccumulated.z = Mathf.Lerp (simulationDataCpu[x][y].velocityAccumulated.z, foam, outputAccumulationRate);
	}
	
	#endregion
	
	#region CPU_threadedMethods
	void BakeAddFluidThreaded (System.Object threadContext){
		ThreadedFieldBakeInfo arrayThreadedInfo = threadContext as ThreadedFieldBakeInfo;
		try{
			for(int x = arrayThreadedInfo.start; x<arrayThreadedInfo.start + arrayThreadedInfo.length; x++){
				for(int y = 0; y<resolutionY; y++){
					foreach(FlowSimulationField field in arrayThreadedInfo.fields){
						simulationDataCpu[x][y].addFluid += field.GetStrengthCpu (arrayThreadedInfo.generator, new Vector2(x/(float)resolutionX, y/(float)resolutionY));
					}
				}
			}
		}catch (System.Exception e){
			Debug.Log (e.ToString ());
		}
		arrayThreadedInfo.resetEvent.Set ();
	}
	
	void BakeRemoveFluidThreaded (System.Object threadContext){
		ThreadedFieldBakeInfo arrayThreadedInfo = threadContext as ThreadedFieldBakeInfo;
		try{
			for(int x = arrayThreadedInfo.start; x<arrayThreadedInfo.start + arrayThreadedInfo.length; x++){
				for(int y = 0; y<resolutionY; y++){
					foreach(FlowSimulationField field in arrayThreadedInfo.fields){
						simulationDataCpu[x][y].removeFluid += field.GetStrengthCpu (arrayThreadedInfo.generator, new Vector2(x/(float)resolutionX, y/(float)resolutionY));
					}
				}
			}
		}catch (System.Exception e){
			Debug.Log (e.ToString ());
		}
		arrayThreadedInfo.resetEvent.Set ();
	}
	
	void BakeForcesThreaded (System.Object threadContext){
		ThreadedFieldBakeInfo arrayThreadedInfo = threadContext as ThreadedFieldBakeInfo;
		try{
			for(int x = arrayThreadedInfo.start; x<arrayThreadedInfo.start + arrayThreadedInfo.length; x++){
				for(int y = 0; y<resolutionY; y++){
					foreach(FlowSimulationField field in arrayThreadedInfo.fields){
						simulationDataCpu[x][y].force += (field as FlowForceField).GetForceCpu (arrayThreadedInfo.generator, new Vector2(x/(float)resolutionX, y/(float)resolutionY));
						simulationDataCpu[x][y].force.z = Mathf.Max (simulationDataCpu[x][y].force.z, 0);
					}
				}
			}
		}catch (System.Exception e){
			Debug.Log (e.ToString ());
		}
		arrayThreadedInfo.resetEvent.Set ();
	}
	
	void BakeHeightmapThreaded (System.Object threadContext){
		ThreadedFieldBakeInfo arrayThreadedInfo = threadContext as ThreadedFieldBakeInfo;
		try{
			for(int x = arrayThreadedInfo.start; x<arrayThreadedInfo.start + arrayThreadedInfo.length; x++){
				for(int y = 0; y<resolutionY; y++){
					foreach(FlowSimulationField field in arrayThreadedInfo.fields){
						float strength = field.GetStrengthCpu (arrayThreadedInfo.generator, new Vector2(x/(float)resolutionX, y/(float)resolutionY));
						simulationDataCpu[x][y].height = Mathf.Lerp (simulationDataCpu[x][y].height, strength, strength * (1-simulationDataCpu[x][y].height));
					}
				}
			}
		}catch (System.Exception e){
			Debug.Log (e.ToString ());
		}
		arrayThreadedInfo.resetEvent.Set ();
	}
	
	void AddRemoveFluidThreaded (System.Object threadContext){
		ArrayThreadedInfo arrayThreadedInfo = threadContext as ArrayThreadedInfo;
		try{
			for(int x = arrayThreadedInfo.start; x<arrayThreadedInfo.start + arrayThreadedInfo.length; x++){
				for(int y = 0; y<resolutionY; y++){
					AddRemoveFluidCpu (x, y);
				}
			}
		}catch (System.Exception e){
			Debug.Log (e.ToString ());
		}
		arrayThreadedInfo.resetEvent.Set ();
	}
	
	void OutflowThreaded (System.Object threadContext){
		ArrayThreadedInfo arrayThreadedInfo = threadContext as ArrayThreadedInfo;
		try{
			for(int x = arrayThreadedInfo.start; x<arrayThreadedInfo.start + arrayThreadedInfo.length; x++){
				for(int y = 0; y<resolutionY; y++){
					OutflowCpu (x, y);
				}
			}
		}catch (System.Exception e){
			Debug.Log (e.ToString ());
		}
		arrayThreadedInfo.resetEvent.Set ();
	}
	
	void UpdateVelocityThreaded (System.Object threadContext){
		ArrayThreadedInfo arrayThreadedInfo = threadContext as ArrayThreadedInfo;
		try{
			for(int x = arrayThreadedInfo.start; x<arrayThreadedInfo.start + arrayThreadedInfo.length; x++){
				for(int y = 0; y<resolutionY; y++){
					UpdateVelocityCpu (x, y);
				}
			}
		}catch (System.Exception e){
			Debug.Log (e.ToString ());
		}
		arrayThreadedInfo.resetEvent.Set ();
	}
	void BlurVelocityAccumulatedHorizontalThreaded (System.Object threadContext){
		ArrayThreadedInfo arrayThreadedInfo = threadContext as ArrayThreadedInfo;
		try{
			for(int x = arrayThreadedInfo.start; x<arrayThreadedInfo.start + arrayThreadedInfo.length; x++){
				for(int y = 0; y<resolutionY; y++){
					BlurVelocityAccumulatedHorizontalCpu (x, y);
				}
			}
		}catch (System.Exception e){
			Debug.Log (e.ToString ());
		}
		arrayThreadedInfo.resetEvent.Set ();
	}
	void BlurVelocityAccumulatedVerticalThreaded (System.Object threadContext){
		ArrayThreadedInfo arrayThreadedInfo = threadContext as ArrayThreadedInfo;
		try{
			for(int x = arrayThreadedInfo.start; x<arrayThreadedInfo.start + arrayThreadedInfo.length; x++){
				for(int y = 0; y<resolutionY; y++){
					BlurVelocityAccumulatedVerticalCpu (x, y);
				}
			}
		}catch (System.Exception e){
			Debug.Log (e.ToString ());
		}
		arrayThreadedInfo.resetEvent.Set ();
	}
	void FoamThreaded (System.Object threadContext){
		ArrayThreadedInfo arrayThreadedInfo = threadContext as ArrayThreadedInfo;
		try{
			for(int x = arrayThreadedInfo.start; x<arrayThreadedInfo.start + arrayThreadedInfo.length; x++){
				for(int y = 0; y<resolutionY; y++){
					FoamCpu (x, y);
				}
			}
		}catch (System.Exception e){
			Debug.Log (e.ToString ());
		}
		arrayThreadedInfo.resetEvent.Set ();
	}
	#endregion
	
	protected override void Update (){
		base.Update ();
		AssignToMaterials ();	
	}
	
	
	void WriteCpuDataToTexture (){	
		if(heightFluidCpu == null || heightFluidCpu.width != resolutionX || heightFluidCpu.height != resolutionY){
			if(heightFluidCpu)
				DestroyProperly (heightFluidCpu);
			heightFluidCpu = new Texture2D(resolutionX, resolutionY, TextureFormat.ARGB32, true, true);
			heightFluidCpu.hideFlags = HideFlags.HideAndDontSave;
			heightFluidCpu.name = "HeightFluidCpu";
		}
		Color[] colors = new Color[resolutionX*resolutionY];
		for(int y = 0; y<resolutionY; y++){
			for(int x = 0; x<resolutionX; x++){
				colors[x + y*resolutionX] = new Color(simulationDataCpu[x][y].height, simulationDataCpu[x][y].fluid, 0,1);
			}
		}
		heightFluidCpu.SetPixels (colors);
		heightFluidCpu.Apply ();
		
		if(velocityAccumulatedCpu == null || velocityAccumulatedCpu.width != resolutionX || velocityAccumulatedCpu.height != resolutionY){
			if(velocityAccumulatedCpu)
				DestroyProperly (velocityAccumulatedCpu);
			velocityAccumulatedCpu = new Texture2D(resolutionX, resolutionY, TextureFormat.ARGB32, true, true);
			velocityAccumulatedCpu.hideFlags = HideFlags.HideAndDontSave;
			velocityAccumulatedCpu.name = "VelocityAccumulatedCpu";
		}
		for(int y = 0; y<resolutionY; y++){
			for(int x = 0; x<resolutionX; x++){
				colors[x + y*resolutionX] = new Color(simulationDataCpu[x][y].velocityAccumulated.x, simulationDataCpu[x][y].velocityAccumulated.y, simulationDataCpu[x][y].velocityAccumulated.z,1);
			}
		}
		velocityAccumulatedCpu.SetPixels (colors);
		velocityAccumulatedCpu.Apply ();
	}

	void AssignToMaterials (){
		if(assignFlowmapToMaterials != null){
			foreach(Material mat in assignFlowmapToMaterials){
				if(mat == null)
					continue;
				if(assignFlowmap)
					mat.SetTexture (assignedFlowmapName, (FlowmapGenerator.SimulationPath == SimulationPath.GPU ? (Texture)velocityAccumulatedRT : (Texture)velocityAccumulatedCpu));
				if(assignHeightAndFluid)
					mat.SetTexture (assignedHeightAndFluidName, (FlowmapGenerator.SimulationPath == SimulationPath.GPU ? (Texture)heightFluidRT : (Texture)heightFluidCpu));
				if(assignUVScaleTransform){
//					crop to fit the largest dimension
					if(Generator.Dimensions.x < Generator.Dimensions.y){
						float aspectRatio =  Generator.Dimensions.y/Generator.Dimensions.x;
						mat.SetVector (assignUVCoordsName, new Vector4(Generator.Dimensions.x * aspectRatio, Generator.Dimensions.y, Generator.Position.x, Generator.Position.z));
					}else{
						float aspectRatio =  Generator.Dimensions.x/Generator.Dimensions.y;
						mat.SetVector (assignUVCoordsName, new Vector4(Generator.Dimensions.x, Generator.Dimensions.y * aspectRatio, Generator.Position.x, Generator.Position.z));
					}
				}
			}
		}
	}
	
	public void WriteTextureToDisk (OutputTexture textureToWrite, string path){
		switch(FlowmapGenerator.SimulationPath){
		case SimulationPath.GPU:
			switch(textureToWrite){
			case OutputTexture.HeightAndFluid:
				TextureUtilities.WriteRenderTextureToFile (heightFluidRT, path, true, TextureUtilities.SupportedFormats[generator.outputFileFormat], "Hidden/WriteHeightFluid");
				break;
			case OutputTexture.Flowmap:
				if(writeFoamSeparately){
					RenderTexture flowmap = GetFlowmapWithoutFoamRT ();
					TextureUtilities.WriteRenderTextureToFile (flowmap, path, true, TextureUtilities.SupportedFormats[generator.outputFileFormat]);	
					if(Application.isPlaying)
						Destroy (flowmap);
				}else{
					TextureUtilities.WriteRenderTextureToFile (velocityAccumulatedRT, path, true, TextureUtilities.SupportedFormats[generator.outputFileFormat]);
				}
				break;
			case OutputTexture.Foam:
				RenderTexture foam = GetFoamRT ();
				TextureUtilities.WriteRenderTextureToFile (foam, path, true, TextureUtilities.SupportedFormats[generator.outputFileFormat]);	
				if(Application.isPlaying)
					Destroy (foam);
				else
					DestroyImmediate (foam);
				break;
			}
			break;
		case SimulationPath.CPU:
			switch(textureToWrite){
			case OutputTexture.HeightAndFluid:
				TextureUtilities.WriteTexture2DToFile (heightFluidCpu, path, TextureUtilities.SupportedFormats[generator.outputFileFormat]);
				break;
			case OutputTexture.Flowmap:
				if(writeFoamSeparately){
					Texture2D flowmapTex = GetFlowmapWithoutFoamTextureCPU ();
					TextureUtilities.WriteTexture2DToFile (flowmapTex, path, TextureUtilities.SupportedFormats[generator.outputFileFormat]);
					if(Application.isPlaying)
						Destroy (flowmapTex);
					else
						DestroyImmediate (flowmapTex);
				}else{
					TextureUtilities.WriteTexture2DToFile (velocityAccumulatedCpu, path, TextureUtilities.SupportedFormats[generator.outputFileFormat]);
				}
				#if UNITY_EDITOR
				UnityEditor.AssetDatabase.Refresh (UnityEditor.ImportAssetOptions.ForceUpdate);
				#endif
				break;
			case OutputTexture.Foam:
				Texture2D foamTex = GetFoamTextureCPU ();
				TextureUtilities.WriteTexture2DToFile (foamTex, path, TextureUtilities.SupportedFormats[generator.outputFileFormat]);	
				if(Application.isPlaying)
					Destroy (foamTex);
				else
					DestroyImmediate (foamTex);
				break;
			}
			break;
		}
		#if UNITY_EDITOR
		UnityEditor.AssetDatabase.Refresh (UnityEditor.ImportAssetOptions.ForceUpdate);
		#endif
	}
	
	Texture2D GetFoamTextureCPU (){
		Texture2D foamTex = new Texture2D(resolutionX, resolutionY, TextureFormat.ARGB32, true);
		foamTex.hideFlags = HideFlags.HideAndDontSave;
		Color[] colors = new Color[resolutionX * resolutionY];
		for(int y = 0; y<resolutionY; y++){
			for(int x = 0; x<resolutionX; x++){
				colors[x + y * resolutionX] = new Color(simulationDataCpu[x][y].velocityAccumulated.z, simulationDataCpu[x][y].velocityAccumulated.z, simulationDataCpu[x][y].velocityAccumulated.z, 1);
			}
		}
		foamTex.SetPixels (colors);
		foamTex.Apply ();
		return foamTex;
	}
	
	Texture2D GetFlowmapWithoutFoamTextureCPU (){
		Texture2D flowmapTex = new Texture2D(resolutionX, resolutionY, TextureFormat.ARGB32, true);
		flowmapTex.hideFlags = HideFlags.HideAndDontSave;
		Color[] colors = new Color[resolutionX * resolutionY];
		for(int y = 0; y<resolutionY; y++){
			for(int x = 0; x<resolutionX; x++){
				colors[x + y * resolutionX] = new Color(simulationDataCpu[x][y].velocityAccumulated.x, simulationDataCpu[x][y].velocityAccumulated.y, 0, 1);
			}
		}
		flowmapTex.SetPixels (colors);
		flowmapTex.Apply ();
		return flowmapTex;
	}
	
	RenderTexture GetFoamRT (){
		RenderTexture foamRT = new RenderTexture(resolutionX, resolutionY, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
		Graphics.Blit (velocityAccumulatedRT, foamRT, SimulationMaterial, 12);
		return foamRT;
	}
	
	RenderTexture GetFlowmapWithoutFoamRT (){
		RenderTexture flowmapRT = new RenderTexture(resolutionX, resolutionY, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
		Graphics.Blit (velocityAccumulatedRT, flowmapRT, SimulationMaterial, 13);
		return flowmapRT;
	}
	
	protected override void MaxStepsReached ()
	{
		base.MaxStepsReached ();
		if(writeToFileOnMaxSimulationSteps && !string.IsNullOrEmpty (outputFolderPath) && System.IO.Directory.Exists (outputFolderPath)){
			WriteAllTextures ();
		}
		if(OnMaxStepsReached != null)
			OnMaxStepsReached ();
	}
	
	public void WriteAllTextures (){
		switch(FlowmapGenerator.SimulationPath){
		case SimulationPath.GPU:					
			if(writeHeightAndFluid)
				TextureUtilities.WriteRenderTextureToFile (heightFluidRT, outputFolderPath + "/" + outputPrefix + "HeightAndFluid", true, TextureUtilities.SupportedFormats[generator.outputFileFormat], "Hidden/WriteHeightFluid");
			if(writeFoamSeparately){
				RenderTexture foam = GetFoamRT ();
				TextureUtilities.WriteRenderTextureToFile (foam, outputFolderPath + "/" + outputPrefix + "Foam", TextureUtilities.SupportedFormats[generator.outputFileFormat]);	
				if(Application.isPlaying)
					Destroy (foam);
				else
					DestroyImmediate (foam);
				
				RenderTexture flowmap = GetFlowmapWithoutFoamRT ();
				TextureUtilities.WriteRenderTextureToFile (flowmap, outputFolderPath + "/" + outputPrefix + "Flowmap", TextureUtilities.SupportedFormats[generator.outputFileFormat]);	
				if(Application.isPlaying)
					Destroy (flowmap);
				else
					DestroyImmediate (flowmap);
			}else{
				TextureUtilities.WriteRenderTextureToFile (velocityAccumulatedRT, outputFolderPath + "/" + outputPrefix + "Flowmap", TextureUtilities.SupportedFormats[generator.outputFileFormat]);	
			}
			break;
			
		case SimulationPath.CPU:
			if(simulationDataCpu == null){
				Init ();	
			}
			WriteCpuDataToTexture ();					
			if(writeHeightAndFluid)
				TextureUtilities.WriteTexture2DToFile (heightFluidCpu, outputFolderPath + "/" + outputPrefix + "HeightAndFluid", TextureUtilities.SupportedFormats[generator.outputFileFormat]);
			if(writeFoamSeparately){
				Texture2D foamTex = GetFoamTextureCPU ();
				TextureUtilities.WriteTexture2DToFile (foamTex, outputFolderPath + "/" + outputPrefix + "Foam", TextureUtilities.SupportedFormats[generator.outputFileFormat]);	
				if(Application.isPlaying)
					Destroy (foamTex);
				else
					DestroyImmediate (foamTex);
				
				Texture2D flowmapTex = GetFlowmapWithoutFoamTextureCPU ();
				TextureUtilities.WriteTexture2DToFile (flowmapTex, outputFolderPath + "/" + outputPrefix + "Flowmap", TextureUtilities.SupportedFormats[generator.outputFileFormat]);
				if(Application.isPlaying)
					Destroy (flowmapTex);
				else
					DestroyImmediate (flowmapTex);
			}else{
				TextureUtilities.WriteTexture2DToFile (velocityAccumulatedCpu, outputFolderPath + "/" + outputPrefix + "Flowmap", TextureUtilities.SupportedFormats[generator.outputFileFormat]);	
			}
			break;
		}
//		set correct import settings
		#if UNITY_EDITOR
		if(outputFolderPath.Contains (Application.dataPath)){
			UnityEditor.AssetDatabase.Refresh (UnityEditor.ImportAssetOptions.ForceUpdate);
			string localPath = "Assets" + (outputFolderPath.Split (new string[]{"Assets"}, System.StringSplitOptions.None))[1];
			string importPath = localPath + "/" + outputPrefix + "Flowmap" +"."+ TextureUtilities.SupportedFormats[generator.outputFileFormat].extension;
			UnityEditor.TextureImporter flowmapImporter = UnityEditor.AssetImporter.GetAtPath (importPath) as UnityEditor.TextureImporter;
			flowmapImporter.linearTexture = true;
			flowmapImporter.textureFormat = UnityEditor.TextureImporterFormat.AutomaticTruecolor;
			UnityEditor.AssetDatabase.ImportAsset (importPath, UnityEditor.ImportAssetOptions.ForceUpdate);
		}
		#endif
	}
}