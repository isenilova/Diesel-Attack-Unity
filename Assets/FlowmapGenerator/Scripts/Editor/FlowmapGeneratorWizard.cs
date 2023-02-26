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

public class FlowmapGeneratorWizard : ScriptableWizard {
	
	[MenuItem("GameObject/Create Other/Flowmap Generator")]
	static void Create (){
		ScriptableWizard.DisplayWizard<FlowmapGeneratorWizard>("Flowmap Generator", "Create", "Cancel");
	}
	
	public enum HeightmapStyles {None, Texture, Render}
	public HeightmapStyles heightmapStyle = HeightmapStyles.Render;
	
	void OnWizardCreate (){
		GameObject go = new GameObject("FlowmapGenerator", typeof(FlowmapGenerator), typeof(ShallowWaterSimulator));
		switch(heightmapStyle){
		case HeightmapStyles.Render:
			go.AddComponent<FlowRenderHeightmap>();
			break;
		case HeightmapStyles.Texture:
			go.AddComponent<FlowTextureHeightmap>();
			break;
		}
		Selection.activeObject = go;
	}
	
	void OnWizardUpdate (){
		helpString = "Create a flowmap generate with default settings.";
	}
	
	void OnWizardOtherButton (){
		Close ();	
	}
}
