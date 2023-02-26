//----------------------------------------------
// Flowmap Generator
// Copyright © 2013 Superposition Games
// http://www.superpositiongames.com
// support@superpositiongames.com
//----------------------------------------------
using UnityEngine;
using System.Collections;
using Flowmap;

/** Adds fluid to the simulation. */
[AddComponentMenu("Flowmaps/Fields/Add fluid")]
public class FluidAddField : FlowSimulationField {
	
	public override FieldPass Pass {
		get {
			return FieldPass.AddFluid;
		}
	}
	
	protected override Shader RenderShader {
		get {
			return Shader.Find ("Hidden/AddFluidPreview");
		}
	}
}
