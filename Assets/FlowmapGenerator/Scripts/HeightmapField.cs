//----------------------------------------------
// Flowmap Generator
// Copyright © 2013 Superposition Games
// http://www.superpositiongames.com
// support@superpositiongames.com
//----------------------------------------------
using UnityEngine;
using System.Collections;
using Flowmap;

/** Writes to the flowmap simulator's heightmap. */
[AddComponentMenu("Flowmaps/Fields/Heightmap")]
public class HeightmapField : FlowSimulationField {
	
	public override FieldPass Pass {
		get {
			return FieldPass.Heightmap;
		}
	}
	
	protected override Shader RenderShader {
		get {
			return Shader.Find ("Hidden/HeightmapFieldPreview");
		}
	}
}