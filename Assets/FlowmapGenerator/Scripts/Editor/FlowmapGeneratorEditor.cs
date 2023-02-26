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

[CustomEditor(typeof(FlowmapGenerator))]
public class FlowmapGeneratorEditor : Editor {
	
	SerializedProperty gpuAcceleration;
	SerializedProperty simulationFields;
	SerializedProperty autoAddChildFields;
	SerializedProperty dimensions;
	SerializedProperty maxThreadCount;
	SerializedProperty outputFileFormat;
	
	void OnEnable (){
		gpuAcceleration = serializedObject.FindProperty ("gpuAcceleration");
		simulationFields = serializedObject.FindProperty ("fields");
		autoAddChildFields = serializedObject.FindProperty ("autoAddChildFields");
		dimensions = serializedObject.FindProperty ("dimensions");
		maxThreadCount = serializedObject.FindProperty ("maxThreadCount");
		outputFileFormat = serializedObject.FindProperty ("outputFileFormat");
	}
	
	public override void OnInspectorGUI (){
		
		serializedObject.Update();
		FlowmapGenerator generator = target as FlowmapGenerator;
		generator.CleanNullFields ();
		
		FlowSimulationField.DrawFalloffTextures = EditorGUILayout.Toggle (new GUIContent("Draw falloff textures", "Draws falloff textures for simulation fields when selected."), FlowSimulationField.DrawFalloffTextures);
		if(generator.FlowSimulator){
			GUI.enabled = !generator.FlowSimulator.Simulating && SystemInfo.SupportsRenderTextureFormat (RenderTextureFormat.ARGBHalf) && SystemInfo.supportsRenderTextures && UnityEditorInternal.InternalEditorUtility.HasPro ();
			EditorGUILayout.PropertyField (gpuAcceleration, new GUIContent("GPU acceleration", "Enable GPU acceleration if available. (Unity Pro only, Requires support for at least ARGBHalf render textures.)"));		
			gpuAcceleration.boolValue = gpuAcceleration.boolValue && FlowmapGenerator.SupportsGPUPath;
			GUI.enabled = true;
			
			if(FlowmapGenerator.SimulationPath == SimulationPath.CPU){
				GUI.enabled = !generator.FlowSimulator.Simulating;
				EditorGUILayout.PropertyField (maxThreadCount, new GUIContent("Max Thread Count", "Sets the maximum simulation threads used."));
				maxThreadCount.intValue = (int)Mathf.Clamp(maxThreadCount.intValue, 1, SystemInfo.processorCount);
				GUI.enabled = true;
			}
		}
		
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Create field:", GUILayout.ExpandWidth (false));
		GUILayout.EndHorizontal ();	
		GUILayout.BeginHorizontal ();
		if(GUILayout.Button (new GUIContent("Add fluid", "Adds fluid to the simulation."), GUILayout.ExpandWidth (false))){
			Undo.RegisterSceneUndo ("Add field");
			GameObject newGo = new GameObject("Add Fluid Field");
			FluidAddField newField = newGo.AddComponent<FluidAddField>();
			generator.AddSimulationField (newField);
			newGo.transform.parent = generator.transform;
			newGo.transform.localScale = Vector3.one;
			newGo.transform.localPosition = Vector3.zero;
			Selection.activeObject = newGo;
		}
		if(GUILayout.Button (new GUIContent("Remove fluid", "Removes fluid from the simulation."), GUILayout.ExpandWidth (false))){
			Undo.RegisterSceneUndo ("Add field");
			GameObject newGo = new GameObject("Remove Fluid Field");
			FluidRemoveField newField = newGo.AddComponent<FluidRemoveField>();
			generator.AddSimulationField (newField);
			newGo.transform.parent = generator.transform;
			newGo.transform.localScale = Vector3.one;
			newGo.transform.localPosition = Vector3.zero;
			Selection.activeObject = newGo;
		}	
		if(GUILayout.Button (new GUIContent("Force", "Applies a force to the fluid in the simulation (Directional, vortex, etc)."), GUILayout.ExpandWidth (false))){
			Undo.RegisterSceneUndo ("Add field");
			GameObject newGo = new GameObject("Force Field");
			FlowForceField newField = newGo.AddComponent<FlowForceField>();
			generator.AddSimulationField (newField);
			newGo.transform.parent = generator.transform;
			newGo.transform.localScale = Vector3.one;
			newGo.transform.localPosition = Vector3.zero;
			Selection.activeObject = newGo;
		}
		if(GUILayout.Button (new GUIContent("Heightmap", "Writes to the heightmap."), GUILayout.ExpandWidth (false))){
			Undo.RegisterSceneUndo ("Add field");
			GameObject newGo = new GameObject("Height Field");
			HeightmapField newField = newGo.AddComponent<HeightmapField>();
			generator.AddSimulationField (newField);
			newGo.transform.parent = generator.transform;
			newGo.transform.localScale = Vector3.one;
			newGo.transform.localPosition = Vector3.zero;
			Selection.activeObject = newGo;
		}
		GUILayout.EndHorizontal ();		
		
		EditorGUILayout.PropertyField (simulationFields, new GUIContent("Simulation Fields"), true);		
		EditorGUILayout.PropertyField (autoAddChildFields, new GUIContent("Auto Add Child Fields", "Automatically add all child fields."));
		GUILayout.BeginHorizontal ();
		if(GUILayout.Button (new GUIContent("Add child fields", "Add all child fields to this generator."), GUILayout.ExpandWidth (false))){
			Undo.RegisterSceneUndo ("Add child fields");
			foreach(FlowSimulationField field in generator.GetComponentsInChildren<FlowSimulationField>()){
				generator.AddSimulationField (field);	
			}
		}
		if(GUILayout.Button (new GUIContent("Clear fields", "Remove all fields from this generator."), GUILayout.ExpandWidth (false))){
			Undo.RegisterSceneUndo ("Clear fields");
			autoAddChildFields.boolValue = false;
			serializedObject.ApplyModifiedProperties ();
			generator.ClearAllFields ();
		}
		GUILayout.EndHorizontal ();		
		if(generator.FlowSimulator == null){
			Undo.RegisterSceneUndo ("Add simulator");
			GUILayout.Label ("No FlowSimulator found.");	
			if(GUILayout.Button ("Add Shallow Water Simulator")){
				generator.gameObject.AddComponent<ShallowWaterSimulator>();	
			}
		}
		
		EditorGUILayout.PropertyField (dimensions, new GUIContent("Dimensions", "Sets the bounds of the flowmap generator and render to heightmap if there is one attached."), true);
		dimensions.vector2Value = Vector2.Max (Vector2.zero, dimensions.vector2Value);
		
		if(generator.Heightmap && (generator.Heightmap is FlowRenderHeightmap && !FlowRenderHeightmap.Supported)){
			GUILayout.Label ("Warning: This generator has an unsupported Render from Scene heightmap. " + FlowRenderHeightmap.UnsupportedReason);	
		}
		
		outputFileFormat.intValue = EditorGUILayout.Popup ("Output format", generator.outputFileFormat, TextureUtilities.GetSupportedFormatsWithExtension());
		
		serializedObject.ApplyModifiedProperties ();
		if(GUI.changed){
			EditorUtility.SetDirty (generator);				
		}
		generator.UpdateSimulationPath ();
		generator.UpdateThreadCount ();
		
	}
	
	public override bool HasPreviewGUI (){
		return true;
	}
	
	public override void OnPreviewGUI (Rect r, GUIStyle background){
		FlowmapGenerator generator = target as FlowmapGenerator;
		if(generator.FlowSimulator && generator.FlowSimulator is ShallowWaterSimulator){
			Rect velocityRect = r;
			if(generator.Heightmap && generator.Heightmap.HeightmapTexture){
				Rect heightmapRect = r;
				if(r.width > r.height){
//					horizontal layout
					heightmapRect.width = r.width/2f - 4;
					velocityRect.width = heightmapRect.width;
					velocityRect.x = heightmapRect.width + 4;
				}else{
//					vertical layout
					heightmapRect.height = r.height/2f - 4;
					velocityRect.height = heightmapRect.height;
					velocityRect.y = heightmapRect.y + heightmapRect.height + 4;
				}
				heightmapRect.height -= 16;
				heightmapRect.y += 16;
				GUI.DrawTexture (heightmapRect, generator.Heightmap.PreviewHeightmapTexture, ScaleMode.ScaleToFit);
				heightmapRect.height = 16;
				heightmapRect.y -= 16;
				GUI.Label (heightmapRect, "Heightmap");
			}
			if((generator.FlowSimulator as ShallowWaterSimulator).VelocityAccumulatedTexture){
				velocityRect.height -= 16;
				velocityRect.y += 16;
				GUI.DrawTexture (velocityRect, (generator.FlowSimulator as ShallowWaterSimulator).VelocityAccumulatedTexture, ScaleMode.ScaleToFit);
				velocityRect.height = 16;
				velocityRect.y -= 16;
				GUI.Label (velocityRect, "Flowmap");
			}
		}
	}
}
