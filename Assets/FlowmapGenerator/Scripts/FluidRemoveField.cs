//----------------------------------------------
// Flowmap Generator
// Copyright © 2013 Superposition Games
// http://www.superpositiongames.com
// support@superpositiongames.com
//----------------------------------------------
using UnityEngine;
using System.Collections;
using Flowmap;

/** Removes fluid to the simulation. */
[AddComponentMenu("Flowmaps/Fields/Remove fluid")]
public class FluidRemoveField : FlowSimulationField {
	
	public override FieldPass Pass {
		get {
			return FieldPass.RemoveFluid;
		}
	}
	
	protected override Shader RenderShader {
		get {
			return Shader.Find ("Hidden/RemoveFluidPreview");
		}
	}
}
