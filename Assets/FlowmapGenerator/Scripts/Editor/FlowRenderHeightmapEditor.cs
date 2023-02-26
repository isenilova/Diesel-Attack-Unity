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

[CustomEditor(typeof(FlowRenderHeightmap))]
public class FlowRenderHeightmapEditor : Editor {
	
	SerializedProperty resolutionX;
	SerializedProperty resolutionY;
	SerializedProperty fluidDepth;
	SerializedProperty heightMin;
	SerializedProperty heightMax;
	SerializedProperty cullingMask;
	SerializedProperty dynamicUpdating;	
	SerializedProperty previewHeightmap;
	
	void OnEnable (){
		resolutionX = serializedObject.FindProperty ("resolutionX");
		resolutionY = serializedObject.FindProperty ("resolutionY");
		fluidDepth = serializedObject.FindProperty ("fluidDepth");
		heightMin = serializedObject.FindProperty ("heightMin");
		heightMax = serializedObject.FindProperty ("heightMax");
		cullingMask = serializedObject.FindProperty ("cullingMask");
		dynamicUpdating = serializedObject.FindProperty ("dynamicUpdating");
		previewHeightmap = serializedObject.FindProperty ("previewHeightmap");
	}
	
	public override void OnInspectorGUI ()
	{
		serializedObject.Update ();
		FlowRenderHeightmap renderHeightmap = target as FlowRenderHeightmap;
		GUILayout.BeginHorizontal ();
		if(GUILayout.Button ("Write to file", GUILayout.ExpandWidth (false))){
			renderHeightmap.UpdateHeightmap ();
			string defaultPath = EditorPrefs.GetString ("FlowRenderHeightmapPath", Application.dataPath);
			string path = EditorUtility.SaveFilePanel ("Save Heightmap", defaultPath, "Heightmap", Flowmap.TextureUtilities.SupportedFormats[renderHeightmap.GetComponent<FlowmapGenerator>().outputFileFormat].extension);
			if(!string.IsNullOrEmpty (path)){
				TextureUtilities.WriteRenderTextureToFile ((target as FlowRenderHeightmap).HeightmapTexture as RenderTexture, path, Flowmap.TextureUtilities.SupportedFormats[renderHeightmap.GetComponent<FlowmapGenerator>().outputFileFormat]);
				EditorPrefs.SetString ("FlowRenderHeightmapPath", path);
				if(path.Contains (Application.dataPath)){
					UnityEditor.AssetDatabase.Refresh (UnityEditor.ImportAssetOptions.ForceUpdate);
					string localPath = "Assets" + (path.Split (new string[]{"Assets"}, System.StringSplitOptions.None))[1];
					TextureImporter textureImporter = UnityEditor.AssetImporter.GetAtPath (localPath) as UnityEditor.TextureImporter;
					textureImporter.linearTexture = true;
					textureImporter.isReadable = true;
					textureImporter.textureFormat = UnityEditor.TextureImporterFormat.AutomaticTruecolor;
					AssetDatabase.ImportAsset (localPath, UnityEditor.ImportAssetOptions.ForceUpdate);
				}
			}
		}
		if(GUILayout.Button("Update", GUILayout.ExpandWidth (false))){
			renderHeightmap.UpdateHeightmap ();
		}
		GUILayout.EndHorizontal ();
		EditorGUILayout.PropertyField (resolutionX, new GUIContent("Resolution X", "The heightmap texture's width."));
		resolutionX.intValue = (int)Mathf.Clamp (resolutionX.intValue, 16, 4096);
		EditorGUILayout.PropertyField (resolutionY, new GUIContent("Resolution Y", "The heightmap texture's height."));
		resolutionY.intValue = (int)Mathf.Clamp (resolutionY.intValue, 16, 4096);
		EditorGUILayout.PropertyField (fluidDepth, new GUIContent("Fluid Depth", "If set to Deep Water, the resulting heightmap will be a cross section of geometry that intersects with the generator plane. If set to Surface, the resulting heightmap will look more like a traditional heightmap."));
		EditorGUILayout.PropertyField (heightMin, new GUIContent("Height Min", "The minimum height, the result depends on the fluid depth chosen."));
		heightMin.floatValue = Mathf.Max (heightMin.floatValue, 0.001f);
		EditorGUILayout.PropertyField (heightMax, new GUIContent("Height Max", "The maximum height, the result depends on the fluid depth chosen."));
		heightMax.floatValue = Mathf.Max (heightMax.floatValue, 0.001f);
		EditorGUILayout.PropertyField (cullingMask, new GUIContent("Culling Mask", "Only geometry on these layers are rendered to the heightmap."));
		EditorGUILayout.PropertyField (dynamicUpdating, new GUIContent("Dynamic Updating", "If enabled the heightmap is rendered before every simulation tick."));
		EditorGUILayout.PropertyField (previewHeightmap, new GUIContent("Preview Heightmap", "Draws the heightmap in the scene view."));
		serializedObject.ApplyModifiedProperties ();
		
		renderHeightmap.UpdatePreviewHeightmap ();
		
		if(GUI.changed){
			renderHeightmap.UpdateHeightmap();
			EditorUtility.SetDirty (target);	
		}
	}
}
