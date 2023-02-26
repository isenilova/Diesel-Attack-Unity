using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour {

	[SerializeField]
	FlowForceField[] vortexPair0;
	[SerializeField]
	Transform skimmerClockwise0;
	[SerializeField]
	Transform skimmerCounterClockwise0;
	[SerializeField]
	float strengthVortexPair0;
	[SerializeField]
	FlowForceField[] vortexPair1;
	[SerializeField]
	Transform skimmerClockwise1;
	[SerializeField]
	Transform skimmerCounterClockwise1;
	[SerializeField]
	float strengthVortexPair1;
	[SerializeField]
	FlowForceField[] vortexPair2;
	[SerializeField]
	Transform skimmerClockwise2;
	[SerializeField]
	Transform skimmerCounterClockwise2;
	[SerializeField]
	float strengthVortexPair2;
	[SerializeField]
	FluidAddField[] sideAddFluid;
	[SerializeField]
	float strengthSideAddFluid;
	
	void Update (){
		
		for(int i=0; i<vortexPair0.Length; i++){
			vortexPair0[i].strength = strengthVortexPair0;
		}
				
		for(int i=0; i<vortexPair1.Length; i++){
			vortexPair1[i].strength = strengthVortexPair1;	
		}
		
		for(int i=0; i<vortexPair2.Length; i++){
			vortexPair2[i].strength = strengthVortexPair2;	
		}
		
		for(int i=0; i<sideAddFluid.Length; i++){
			sideAddFluid[i].strength = strengthSideAddFluid;	
		}
		
		skimmerClockwise0.Rotate (Vector3.up, Mathf.Min (0.04f, strengthVortexPair0 * 20 * Time.deltaTime));
		skimmerCounterClockwise0.Rotate (Vector3.up, -Mathf.Min (0.04f, strengthVortexPair0 * 20 * Time.deltaTime));
		
		skimmerClockwise1.Rotate (Vector3.up, Mathf.Min (0.04f, strengthVortexPair1 * 20 * Time.deltaTime));
		skimmerCounterClockwise1.Rotate (Vector3.up, -Mathf.Min (0.04f, strengthVortexPair1 * 20 * Time.deltaTime));
		
		skimmerClockwise2.Rotate (Vector3.up, Mathf.Min (0.04f, strengthVortexPair2 * 20 * Time.deltaTime));
		skimmerCounterClockwise2.Rotate (Vector3.up, -Mathf.Min (0.04f, strengthVortexPair2 * 20 * Time.deltaTime));
	}
}
