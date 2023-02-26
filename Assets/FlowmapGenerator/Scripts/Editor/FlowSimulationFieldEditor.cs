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
using System.Linq;

[CanEditMultipleObjects]
[CustomEditor(typeof(FlowSimulationField))]
public class FlowSimulationFieldEditor : Editor {

	SerializedProperty strength;
	SerializedProperty falloffTexture;
		
	protected virtual void OnEnable (){
		serializedObject.Update();
		strength = serializedObject.FindProperty ("strength");
		falloffTexture = serializedObject.FindProperty ("falloffTexture");
	}
	
	public override void OnInspectorGUI ()
	{
		foreach(Object selectedTarget in targets){
			if(selectedTarget is FlowSimulationField){
				FlowSimulationField field = selectedTarget as FlowSimulationField;
				field.UpdateRenderPlane();
			}
		}
		serializedObject.Update ();
		EditorGUILayout.PropertyField (strength, new GUIContent("Strength"));
		strength.floatValue = Mathf.Clamp01 (strength.floatValue);
		EditorGUILayout.PropertyField (falloffTexture, new GUIContent("Falloff"));
		Texture2D falloffTex2D = falloffTexture.objectReferenceValue as Texture2D;
		if(falloffTex2D){
			TextureImporter falloffImporter = AssetImporter.GetAtPath (AssetDatabase.GetAssetPath (falloffTex2D)) as TextureImporter;
			if(!falloffImporter.isReadable || !falloffImporter.linearTexture){
				GUILayout.BeginHorizontal ();
				GUILayout.Label ("Incorrect texture settings.", GUILayout.ExpandWidth (false));
				if(GUILayout.Button ("Fix now", GUILayout.ExpandWidth (false))){
					falloffImporter.isReadable = true;
					falloffImporter.linearTexture = true;
					falloffImporter.textureFormat = TextureImporterFormat.AutomaticTruecolor;
					AssetDatabase.ImportAsset (AssetDatabase.GetAssetPath (falloffTex2D), ImportAssetOptions.ForceUpdate);
				}
				GUILayout.EndHorizontal ();
			}
		}
		serializedObject.ApplyModifiedProperties ();		
	}
}
