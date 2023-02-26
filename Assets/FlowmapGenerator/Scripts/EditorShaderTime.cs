//----------------------------------------------
// Editor Shader Time
// Sets a global shader variable "_EditorTime"
// Can be added with _Time in shaders to let time dependant shaders with in the scene view when not playing.
// Copyright © 2013 Superposition Games
// http://www.superpositiongames.com
// support@superpositiongames.com
//----------------------------------------------
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

/** Set a global shader variable "_EditorTime" from the time since editor startup. This lets shaders that depend on time work in the editor when the application isn't playing.*/
#if UNITY_EDITOR
[ExecuteInEditMode]
[InitializeOnLoad]
#endif
public class EditorShaderTime : MonoBehaviour {
#if UNITY_EDITOR
	void OnEnable (){
		UnityEditor.EditorApplication.update += UpdateEditorTime;	
	}
	
	void OnDisable (){
		UnityEditor.EditorApplication.update -= UpdateEditorTime;
	}
	
	void UpdateEditorTime (){
		if(!Application.isPlaying){
			Shader.SetGlobalFloat ("_EditorTime", (float)UnityEditor.EditorApplication.timeSinceStartup);
//		need to set dirty, otherwise the update doesn't get called every frame
			EditorUtility.SetDirty (this);
		}
	}
	
	public static float CurrentTime{
		get{
			return (float)UnityEditor.EditorApplication.timeSinceStartup;
		}
	}
#endif
}
