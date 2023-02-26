//----------------------------------------------
// Flowmap Generator
// Copyright © 2013 Superposition Games
// http://www.superpositiongames.com
// support@superpositiongames.com
//----------------------------------------------
using UnityEngine;
using UnityEditor;
using System.Collections;
using Flowmap;

[CustomEditor(typeof(ShallowWaterSimulator))]
[CanEditMultipleObjects]
public class ShallowWaterSimulatorEditor : Editor {
	
	ShallowWaterSimulator.OutputTexture outputTexture;
	
	SerializedProperty simulateOnPlay;
	SerializedProperty maxSimulationSteps;
	SerializedProperty continuousSimulation;
	SerializedProperty writeToFileOnMaxSimulationSteps;
	SerializedProperty updateTextureDelayCPU;
	SerializedProperty outputFolderPath;
	SerializedProperty outputPrefix;
	SerializedProperty writeHeightAndFluid;
	SerializedProperty simulateFoam;
	SerializedProperty foamVelocityScale;
	SerializedProperty writeFoamSeparately;
	SerializedProperty writeFluidDepthInAlpha;
	
	SerializedProperty assignFlowmapToMaterials;
	SerializedProperty assignFlowmap;
	SerializedProperty assignedFlowmapName;
	SerializedProperty assignHeightAndFluid;
	SerializedProperty assignedHeightAndFluidName;
	SerializedProperty assignUVScaleTransform;
	SerializedProperty assignUVCoordsName;
	
	SerializedProperty timestep;
	SerializedProperty gravity;
	SerializedProperty velocityScale;
	SerializedProperty resolutionX;
	SerializedProperty resolutionY;
	SerializedProperty borderCollision;
	SerializedProperty fluidDepth;
	SerializedProperty evaporationRate;
	SerializedProperty initialFluidAmount;
	SerializedProperty fluidAddMultiplier;
	SerializedProperty fluidRemoveMultiplier;
	SerializedProperty fluidForceMultiplier;	
	
	SerializedProperty accumulationRate;
	SerializedProperty blurFilterStrength;
		
	void OnEnable (){
		simulateOnPlay = serializedObject.FindProperty ("simulateOnPlay");
		maxSimulationSteps = serializedObject.FindProperty ("maxSimulationSteps");
		continuousSimulation = serializedObject.FindProperty ("continuousSimulation");
		writeToFileOnMaxSimulationSteps = serializedObject.FindProperty ("writeToFileOnMaxSimulationSteps");	
		updateTextureDelayCPU = serializedObject.FindProperty ("updateTextureDelayCPU");
		outputFolderPath = serializedObject.FindProperty ("outputFolderPath");
		outputPrefix = serializedObject.FindProperty ("outputPrefix");
		writeHeightAndFluid = serializedObject.FindProperty ("writeHeightAndFluid");
		simulateFoam = serializedObject.FindProperty ("simulateFoam");
		foamVelocityScale = serializedObject.FindProperty ("foamVelocityScale");
		writeFoamSeparately = serializedObject.FindProperty ("writeFoamSeparately");
		writeFluidDepthInAlpha = serializedObject.FindProperty ("writeFluidDepthInAlpha");
		
		assignFlowmapToMaterials = serializedObject.FindProperty ("assignFlowmapToMaterials");
		assignFlowmap = serializedObject.FindProperty ("assignFlowmap");
		assignedFlowmapName = serializedObject.FindProperty ("assignedFlowmapName");
		assignHeightAndFluid = serializedObject.FindProperty ("assignHeightAndFluid");
		assignedHeightAndFluidName = serializedObject.FindProperty ("assignedHeightAndFluidName");
		assignUVScaleTransform = serializedObject.FindProperty ("assignUVScaleTransform");
		assignUVCoordsName = serializedObject.FindProperty ("assignUVCoordsName");
		
		timestep = serializedObject.FindProperty ("timestep");
		gravity = serializedObject.FindProperty ("gravity");
		velocityScale = serializedObject.FindProperty ("velocityScale");
		resolutionX = serializedObject.FindProperty ("resolutionX");
		resolutionY = serializedObject.FindProperty ("resolutionY");
		borderCollision = serializedObject.FindProperty ("borderCollision");
		fluidDepth = serializedObject.FindProperty ("fluidDepth");
		evaporationRate = serializedObject.FindProperty ("evaporationRate");
		initialFluidAmount = serializedObject.FindProperty ("initialFluidAmount");
		fluidAddMultiplier = serializedObject.FindProperty ("fluidAddMultiplier");
		fluidRemoveMultiplier = serializedObject.FindProperty ("fluidRemoveMultiplier");
		fluidForceMultiplier = serializedObject.FindProperty ("fluidForceMultiplier");		
		accumulationRate = serializedObject.FindProperty ("outputAccumulationRate");
		blurFilterStrength = serializedObject.FindProperty ("outputFilterStrength");
	}
	
	void OnDisable (){
		EditorApplication.update -= DummyUpdate;	
	}
			
	public override void OnInspectorGUI ()
	{
		EditorGUIUtility.LookLikeControls (180);
		serializedObject.Update();
		ShallowWaterSimulator simulator = target as ShallowWaterSimulator;
		if(simulator.Generator == null){
			GUILayout.Label ("No FlowmapGenerator, please add one.");	
			if(GUILayout.Button ("Add FlowmapGenerator")){
				simulator.gameObject.AddComponent<FlowmapGenerator>();	
			}
			return;
		}
		if(PlayerSettings.colorSpace != ColorSpace.Linear){
			EditorGUILayout.HelpBox ("Linear color space required when using GPU acceleration. (Can be set in Player Settings)", MessageType.Error);	
//			unfortunately this doesn't work :(
//			if(GUILayout.Button ("Switch color space now")){
//				PlayerSettings.colorSpace = ColorSpace.Linear;
//				simulator.Reset ();
//			}
//			GUILayout.Space (8);
		}
		
		if(!System.IO.Directory.Exists (outputFolderPath.stringValue)){
			outputFolderPath.stringValue = "";
		}
		
		GUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Reset", GUILayout.ExpandWidth (false))){
			simulator.Reset ();
			EditorApplication.update -= simulator.Tick;
			simulator.StopSimulating ();
		}
		
		GUI.enabled = !simulator.Simulating;
		if(!string.IsNullOrEmpty (outputFolderPath.stringValue) && maxSimulationSteps.intValue > 0){
			if (GUILayout.Button (new GUIContent("Bake", "Simulate the max number of steps then write output textures to file."), GUILayout.ExpandWidth (false))){
				simulator.Reset ();
				simulator.writeToFileOnMaxSimulationSteps = true;
				simulator.StartSimulating ();
				if(!Application.isPlaying){
					EditorApplication.update += simulator.Tick;
					EditorApplication.update += DummyUpdate;
				}
			}
		}else{
			GUI.enabled = false;
			GUILayout.Button (new GUIContent("Bake", (string.IsNullOrEmpty (outputFolderPath.stringValue) ? "Set an output path to bake to file. " : "")
				+ (maxSimulationSteps.intValue <= 0 ? "Set Max Simulation Steps to a value larger than 0 to bake to file." : "") ), GUILayout.ExpandWidth (false));
		}		
		GUI.enabled = !simulator.Simulating;
		if (GUILayout.Button (new GUIContent("Simulate", "Start simulating from current time and optionally write output textures to file when max steps is reached."), GUILayout.ExpandWidth (false))){
			simulator.StartSimulating ();
			if(!Application.isPlaying){
				EditorApplication.update += simulator.Tick;
				EditorApplication.update += DummyUpdate;
			}
		}
		GUI.enabled = simulator.Simulating;
		if (GUILayout.Button ("Pause", GUILayout.ExpandWidth (false))){
			EditorApplication.update -= simulator.Tick;
			EditorApplication.update -= DummyUpdate;
			simulator.StopSimulating ();
		}
		GUI.enabled = true;
		if (GUILayout.Button ("Write All", GUILayout.ExpandWidth (false))){
			simulator.WriteAllTextures ();
		}
		GUILayout.EndHorizontal ();
		GUILayout.Label ("Simulation Steps: " + simulator.SimulationStepsCount + (simulator.maxSimulationSteps>0 ? "/"+simulator.maxSimulationSteps : ""), GUILayout.ExpandWidth (false));
		
		GUILayout.BeginHorizontal ();
		outputTexture = (ShallowWaterSimulator.OutputTexture)EditorGUILayout.EnumPopup ("Write single texture", outputTexture);
		if(GUILayout.Button ("Write to file", GUILayout.ExpandWidth (false))){
			string path = EditorUtility.SaveFilePanel ("Save Texture", (string.IsNullOrEmpty (outputFolderPath.stringValue) ? Application.dataPath : outputFolderPath.stringValue),
				outputPrefix.stringValue + outputTexture.ToString (), Flowmap.TextureUtilities.SupportedFormats[simulator.Generator.outputFileFormat].extension);
			if(!string.IsNullOrEmpty (path)){
				simulator.WriteTextureToDisk (outputTexture, path);
			}
		}
		GUILayout.EndHorizontal ();
					
		EditorGUILayout.PropertyField (simulateOnPlay, new GUIContent("Simulate on play", "Start simulating when the application is playing."));
		EditorGUILayout.PropertyField (maxSimulationSteps, new GUIContent("Max Simulation Steps", "After simulating for this amount of steps the simulation will pause and optionally write the output textures to file."));
		EditorGUILayout.PropertyField (continuousSimulation, new GUIContent("Continuous Simulation", "Simulation will not stop when max steps is reached."));
		EditorGUILayout.PropertyField (writeToFileOnMaxSimulationSteps, new GUIContent("Write to file on max steps", "When the max simulation steps is reached, write output textures to file."));
		if(FlowmapGenerator.SimulationPath == SimulationPath.CPU)
			EditorGUILayout.PropertyField (updateTextureDelayCPU, new GUIContent("Update Texture Delay", "Delay in simulation steps between updating textures when using the CPU simulation path."));
		updateTextureDelayCPU.intValue = (int)Mathf.Max (1, updateTextureDelayCPU.intValue);
		
		GUILayout.BeginHorizontal ();
		EditorGUILayout.PropertyField (outputFolderPath, new GUIContent("Output folder", "Output textures will be saved to this folder."), GUILayout.MinWidth (230));
		if(GUILayout.Button ("Browse", GUILayout.ExpandWidth (false))){
			Undo.RegisterSceneUndo ("Set output path");
			string path = EditorUtility.SaveFolderPanel ("Output path", (string.IsNullOrEmpty (simulator.outputFolderPath)) ? Application.dataPath : simulator.outputFolderPath, "");
			if(!string.IsNullOrEmpty (path) && System.IO.Directory.Exists (path)){
				outputFolderPath.stringValue = path;
				serializedObject.ApplyModifiedProperties ();
				serializedObject.Update ();
				EditorUtility.SetDirty (target);
			}
		}
		GUILayout.EndHorizontal ();
		EditorGUILayout.PropertyField (outputPrefix, new GUIContent("Output Prefix", "Adds a prefix to output texture filenames."));
		EditorGUILayout.PropertyField (writeHeightAndFluid, new GUIContent("Output Height/Fluid", "Write a texture that contains the heightmap in the red channel and fluid depth in the green channel."));
		EditorGUILayout.PropertyField (simulateFoam, new GUIContent("Simulate Foam", "Calculate where foam should appear. More foam accumulates in slow moving areas."));
		EditorGUILayout.PropertyField (foamVelocityScale, new GUIContent("Foam Velocity Scale", "Scales the velocity from the flowmap before calculating foam. Higher values reduce the amount of foam."));
		foamVelocityScale.floatValue = Mathf.Max (0, foamVelocityScale.floatValue);
		GUI.enabled = simulateFoam.boolValue;
		EditorGUILayout.PropertyField (writeFoamSeparately, new GUIContent("Output Foam Separately", "Write foam to a separate texture, otherwise the foam will be saved to the blue channel of the flowmap."));
		GUI.enabled = true;
		EditorGUILayout.PropertyField (writeFluidDepthInAlpha, new GUIContent("Write Fluid Depth To Alpha", "Write fluid depth to the alpha channel of the flowmap texture."));
		
		
		bool expandAssignToMaterials = EditorPrefs.GetBool ("ShallowWaterSimulatorExpandAssignMat", false);
		expandAssignToMaterials = EditorGUILayout.Foldout (expandAssignToMaterials, new GUIContent("Assign To Materials", "Assign the flowmap being generated to these materials. Lets you see the flowmap while being generated. Useful for continuously updating flowmaps."));
		if(expandAssignToMaterials){
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField (assignFlowmapToMaterials, new GUIContent("Materials", "Materials to assign to."), true);
			EditorGUILayout.PropertyField (assignFlowmap, new GUIContent("Assign Flowmap", "Assign the flowmap currently being generated to materials."));
			EditorGUILayout.PropertyField (assignedFlowmapName, new GUIContent("Flowmap Name", "Assign the flowmap to materials using this texture name."));
			EditorGUILayout.PropertyField (assignHeightAndFluid, new GUIContent("Assign Height/Fluid", "Assign the heightmap and fluid depth currently being generated to materials."));
			EditorGUILayout.PropertyField (assignedHeightAndFluidName, new GUIContent("Height/Fluid Name", "Assign the heightmap and fluid texture to materials using this texture name."));
			EditorGUILayout.PropertyField (assignUVScaleTransform, new GUIContent("Assign UV", "Assigns a float4 to materials containing the generator's dimensions in xy and the generator's position in zw."));
			EditorGUILayout.PropertyField (assignUVCoordsName, new GUIContent("Assign UV Name", "Sets the name of the float4 that contains the generator's dimensions and position."));
			EditorGUI.indentLevel--;
		}
		EditorPrefs.SetBool ("ShallowWaterSimulatorExpandAssignMat", expandAssignToMaterials);
		
		EditorGUILayout.PropertyField (timestep, new GUIContent("Timestep", "Controls how large the delta time is for a simulation step. Clamped to 40% of gravity, the simulation will most likely be incorrect otherwise."));
		timestep.floatValue = Mathf.Clamp (timestep.floatValue, 0.0001f, gravity.floatValue * 0.4f);
		EditorGUILayout.PropertyField (gravity, new GUIContent("Gravity", "Controls how fast the fluid flows. Lower gravity settings increase the effect force fields have."));
		gravity.floatValue = Mathf.Clamp01 (gravity.floatValue);
		EditorGUILayout.PropertyField (velocityScale, new GUIContent("Velocity Scale", "The velocity from the simulation can be larger than 1, this scales the velocity before writing to the flowmap."));
		velocityScale.floatValue = Mathf.Max (0, velocityScale.floatValue);
		EditorGUILayout.PropertyField (resolutionX, new GUIContent("Resolution X"));
		resolutionX.intValue = (int)Mathf.Clamp (resolutionX.intValue, 16, 4096);
		EditorGUILayout.PropertyField (resolutionY, new GUIContent("Resolution Y"));
		resolutionY.intValue = (int)Mathf.Clamp (resolutionY.intValue, 16, 4096);
		EditorGUILayout.PropertyField (borderCollision, new GUIContent("Border Collision", "Should the fluid collide with the simulation borders or pass through."));
		EditorGUILayout.PropertyField (fluidDepth, new GUIContent("Fluid Depth", "Set the fluid depth style, several other settings are affected."));
		EditorGUILayout.PropertyField (evaporationRate, new GUIContent("Evaporation Rate", "Removes a bit of fluid every simulation step."));
		evaporationRate.floatValue = Mathf.Clamp01 (evaporationRate.floatValue);
		EditorGUILayout.PropertyField (initialFluidAmount, new GUIContent("Initial Fluid Amount", "Starts the simulation with fluid already existing."));
		initialFluidAmount.floatValue = Mathf.Clamp01 (initialFluidAmount.floatValue);
		EditorGUILayout.PropertyField (fluidAddMultiplier, new GUIContent("Fluid Add Multiplier", "A global multiplier for all fluid add fields."));
		fluidAddMultiplier.floatValue = Mathf.Clamp01 (fluidAddMultiplier.floatValue);
		EditorGUILayout.PropertyField (fluidRemoveMultiplier, new GUIContent("Fluid Remove Multiplier", "A global multiplier for all fluid remove fields."));
		fluidRemoveMultiplier.floatValue = Mathf.Clamp01 (fluidRemoveMultiplier.floatValue);
		EditorGUILayout.PropertyField (fluidForceMultiplier, new GUIContent("Force Multiplier", "A global multiplier for all force fields."));
		fluidForceMultiplier.floatValue = Mathf.Clamp01 (fluidForceMultiplier.floatValue);
		EditorGUILayout.PropertyField (accumulationRate, new GUIContent("Accumulation Rate", "Controls how quickly the flowmap accumulates velocity. A lower rate will make sure that short lived velocity changes are ignored."));
		accumulationRate.floatValue = Mathf.Clamp (accumulationRate.floatValue, 0.001f, 1);
		EditorGUILayout.PropertyField (blurFilterStrength, new GUIContent("Blur Filter Strength", "Sets the strength of the blur filter."));
		blurFilterStrength.floatValue = Mathf.Clamp01 (blurFilterStrength.floatValue);
		serializedObject.ApplyModifiedProperties ();
	}
	
	/* The inspector won't update regularly when simulating unless something in the scene changes. */
	void DummyUpdate (){
		if(target)
			(target as ShallowWaterSimulator).transform.position = (target as ShallowWaterSimulator).transform.position;
	}
}
