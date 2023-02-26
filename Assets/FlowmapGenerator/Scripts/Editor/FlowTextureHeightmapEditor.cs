//----------------------------------------------
// Flowmap Generator
// Copyright © 2013 Superposition Games
// http://www.superpositiongames.com
// support@superpositiongames.com
//----------------------------------------------
using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(FlowTextureHeightmap))]
public class FlowTextureHeightmapEditor : Editor {
	
	SerializedProperty heightmap;
	SerializedProperty isRaw;
	SerializedProperty previewHeightmap;
	
	void OnEnable (){
		heightmap = serializedObject.FindProperty ("heightmap");
		isRaw = serializedObject.FindProperty ("isRaw");
		previewHeightmap = serializedObject.FindProperty ("previewHeightmap");
	}
	
	public override void OnInspectorGUI ()
	{
		serializedObject.Update ();
		if(GUILayout.Button (new GUIContent("Import RAW", "Import a 16 bit RAW."), GUILayout.ExpandWidth (false))){
			string path = EditorUtility.OpenFilePanel ("Import RAW", Application.dataPath, "raw");
			if(!string.IsNullOrEmpty (path)){
				RawImportOptionsWindow.activeFlowTextureHeightmap = target as FlowTextureHeightmap;
				RawImportOptionsWindow.path = path;
				RawImportOptionsWindow.Init ();
			}
		}
		EditorGUILayout.PropertyField (heightmap, new GUIContent("Heightmap", "Heightmap to use in the simulation."));
		Texture2D heightmapTexture = heightmap.objectReferenceValue as Texture2D;
		if(heightmapTexture){
			TextureImporter heightmapImporter = AssetImporter.GetAtPath (AssetDatabase.GetAssetPath (heightmapTexture)) as TextureImporter;
			if(heightmapImporter){
				if(!heightmapImporter.isReadable || !heightmapImporter.linearTexture){
					GUILayout.BeginHorizontal ();
					GUILayout.Label ("Incorrect texture settings.", GUILayout.ExpandWidth (false));
					if(GUILayout.Button ("Fix now", GUILayout.ExpandWidth (false))){
						heightmapImporter.isReadable = true;
						heightmapImporter.linearTexture = true;
						heightmapImporter.textureFormat = TextureImporterFormat.AutomaticTruecolor;
						AssetDatabase.ImportAsset (AssetDatabase.GetAssetPath (heightmapTexture), ImportAssetOptions.ForceUpdate);
					}
					GUILayout.EndHorizontal ();
				}
			}
		}
		EditorGUILayout.PropertyField (isRaw, new GUIContent("Is Raw", "Should be enabled when using an imported RAW texture."));
		EditorGUILayout.PropertyField (previewHeightmap, new GUIContent("Preview Heightmap", "Draws the heightmap in the scene view."));
		serializedObject.ApplyModifiedProperties ();
		(target as FlowTextureHeightmap).UpdatePreviewHeightmap ();
	}
}