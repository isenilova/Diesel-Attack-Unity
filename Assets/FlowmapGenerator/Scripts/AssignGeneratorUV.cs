using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class AssignGeneratorUV : MonoBehaviour {

	[SerializeField]
	FlowmapGenerator generator;
	[SerializeField]
	Vector3 position;
	[SerializeField]
	Vector2 dimensions;
	[SerializeField]
	Renderer[] renderers;
	[SerializeField]
	bool assignToSharedMaterial = true;
	[SerializeField]
	string uvVectorName = "_FlowmapUV";
	
	void Update (){
		for(int i=0; i<renderers.Length; i++){
			if(generator){
				position = generator.Position;
				dimensions = generator.Dimensions;
			}
			
			Vector4 transformVector = Vector4.zero;
			if(dimensions.x < dimensions.y){
				transformVector = new Vector4(dimensions.x * (dimensions.y/dimensions.x), dimensions.y, position.x, position.z);
			}else{
				transformVector = new Vector4(dimensions.x, dimensions.y * (dimensions.x/dimensions.y), position.x, position.z);
			}
			
			if(assignToSharedMaterial){
				renderers[i].sharedMaterial.SetVector (uvVectorName, transformVector);
			}else{
				renderers[i].material.SetVector (uvVectorName, transformVector);	
			}
		}
	}
}
