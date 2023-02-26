//----------------------------------------------
// Flowmap Generator
// Copyright © 2013 Superposition Games
// http://www.superpositiongames.com
// support@superpositiongames.com
//----------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Flowmap;

namespace Flowmap{
	/** GPU path is faster, but requires Unity Pro.*/
	public enum SimulationPath {GPU, CPU}
	/** Collide will cause fluid to bounce off the simulation borders. Pass through will let the fluid run out of the simulation.*/
	public enum SimulationBorderCollision { Collide, PassThrough }
}
[RequireComponent(typeof(FlowmapGenerator))]
public abstract class FlowSimulator : MonoBehaviour {
	
	#region common_settings
	public int resolutionX = 256;
	public int resolutionY = 256;
	/** @see Flowmap::SimulationBorderCollision */
	public SimulationBorderCollision borderCollision;
	/** Start simulating when in play mode. */
	public bool simulateOnPlay;
	public int maxSimulationSteps = 500;
	private int simulationStepsCount;
	public int SimulationStepsCount{
		get{
			return simulationStepsCount;
		}
	}
	public bool continuousSimulation = false;
	public string outputFolderPath;
	public string outputPrefix;
	public bool writeToFileOnMaxSimulationSteps = true;
	#endregion
	
	#region status_members
	private bool simulating;
	public bool Simulating{
		get{
			return simulating;
		}
	}
	private bool initialized;
	protected bool Initialized{
		get{
			return initialized;
		}
	}
	#endregion
	
	protected FlowmapGenerator generator;
	public FlowmapGenerator Generator{
		get{
			if(generator == null){
				generator = GetComponent<FlowmapGenerator>();	
			}
			return generator;
		}
	}
	
	protected virtual void Update () {
		if(!Initialized)
			Init ();
		if(Application.isPlaying && Simulating){
			Tick ();	
		}
	}
	
	public virtual void Init () {
		simulationStepsCount = 0;
		initialized = true;
	}
	public virtual void StartSimulating (){
		if(!Initialized || SimulationStepsCount == 0)
			Init ();
		simulating = true;
	}
	public virtual void StopSimulating (){
		simulating = false;
	}
	/** Reset the simulation to the initial values. */
	public virtual void Reset () {
		simulationStepsCount = 0;
		if(!Initialized)
			Init ();
	}
	/** Simulate one step of the simulation. */
	public virtual void Tick (){
		if(!Simulating)
			return;
		simulationStepsCount++;
		if(simulationStepsCount == maxSimulationSteps && (maxSimulationSteps != 0 && continuousSimulation == false))
			MaxStepsReached ();				
	}
	
	protected virtual void MaxStepsReached (){
		StopSimulating ();
	}
}