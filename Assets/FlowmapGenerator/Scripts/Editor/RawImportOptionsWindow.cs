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

public class RawImportOptionsWindow : EditorWindow {

	enum ByteOrder {PC, Mac}
	
	public static FlowTextureHeightmap activeFlowTextureHeightmap;
	public static string path;
	
	public static void Init (){
		RawImportOptionsWindow window = (RawImportOptionsWindow)GetWindow(typeof(RawImportOptionsWindow), true, "Raw Import");
		window.title = "RawImport";
		window.ShowUtility ();
	}
	
	int resolutionX = 256;
	int resolutionY = 256;
	ByteOrder byteOrder;
		
	void OnGUI (){
		if(string.IsNullOrEmpty (path)){
			GUILayout.Label ("Raw path not found, try importing again.");
			if(GUILayout.Button ("Close")){
				Close ();
			}
			return;
		}
		
		bool wrap = GUI.skin.label.wordWrap;
		GUI.skin.label.wordWrap = true;
		GUILayout.Label ("The width, height, and byte order should match the raw texture's settings. After clicking import you will be asked to save a converted file to somewhere in your Unity project.",
			GUILayout.Height (64));
		GUI.skin.label.wordWrap = wrap;
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("RAW path", GUILayout.Width (144));
		GUILayout.Label (path);
		GUILayout.EndHorizontal ();
		resolutionX = EditorGUILayout.IntField ("Width", resolutionX);
		resolutionY = EditorGUILayout.IntField ("Height", resolutionY);
		byteOrder = (ByteOrder)EditorGUILayout.EnumPopup ("Byte Order", byteOrder);
		
		GUILayout.BeginArea (new Rect(0,position.height - 24, position.width, 24));
		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();
		if(GUILayout.Button ("Cancel", GUILayout.ExpandWidth (false))){
			Close ();	
		}
		if(GUILayout.Button ("Import", GUILayout.ExpandWidth (false))){
			Texture2D heightmap = TextureUtilities.ReadRawImageToTexture (path, resolutionX, resolutionY, byteOrder == ByteOrder.PC);
			string saveFilePath = EditorUtility.SaveFilePanelInProject ("Save RAW as " + TextureUtilities.SupportedFormats[1].extension, "Heightmap", TextureUtilities.SupportedFormats[1].extension, "Save Heightmap");
			if(!string.IsNullOrEmpty (saveFilePath)){
				TextureUtilities.WriteTexture2DToFile (heightmap, saveFilePath, TextureUtilities.SupportedFormats[1]);
				AssetDatabase.Refresh (ImportAssetOptions.ForceUpdate);
				TextureImporter heightmapImporter = AssetImporter.GetAtPath (saveFilePath) as TextureImporter;
				heightmapImporter.isReadable = true;
				heightmapImporter.linearTexture = true;
				heightmapImporter.textureFormat = TextureImporterFormat.AutomaticTruecolor;
				AssetDatabase.ImportAsset (saveFilePath, ImportAssetOptions.ForceUpdate);
				Texture2D newHeightmap = AssetDatabase.LoadAssetAtPath (saveFilePath, typeof(Texture2D)) as Texture2D;
				activeFlowTextureHeightmap.HeightmapTexture = newHeightmap;
				activeFlowTextureHeightmap.isRaw = true;
				activeFlowTextureHeightmap.GenerateRawPreview ();
			}
			DestroyImmediate (heightmap);
			Close ();	
		}
		GUILayout.EndHorizontal ();
		GUILayout.EndArea ();
	}
}