//----------------------------------------------
// Flowmap Generator
// Copyright © 2013 Superposition Games
// http://www.superpositiongames.com
// support@superpositiongames.com
//----------------------------------------------
using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(FlowForceField))]
[CanEditMultipleObjects]
public class FlowForceFieldEditor : FlowSimulationFieldEditor {
	
	SerializedProperty force;
	
	protected override void OnEnable ()
	{
		base.OnEnable ();
		force = serializedObject.FindProperty ("force");
	}
	
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();		
		EditorGUILayout.PropertyField (force, new GUIContent("Force"));
		serializedObject.ApplyModifiedProperties ();
		
		if(GUI.changed){
			foreach(Object selectedTarget in targets){
				if(selectedTarget is FlowForceField)
					(selectedTarget as FlowForceField).UpdateVectorTexture ();
			}
			EditorUtility.SetDirty (this);
		}
	}
}
