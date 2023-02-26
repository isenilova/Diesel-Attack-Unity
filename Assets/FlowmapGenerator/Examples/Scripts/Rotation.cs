using UnityEngine;
using System.Collections;

public class Rotation : MonoBehaviour {
	
	[SerializeField]
	float m_angle = 5;
	
	[SerializeField]
	bool m_useLocalYAxis = true;
	
	[SerializeField]
	Vector3 m_axis = Vector3.up;
		
	void Update () 
	{
		if(m_useLocalYAxis)
			transform.rotation *= Quaternion.AngleAxis (m_angle * Time.deltaTime, transform.up);
		else	
			transform.rotation *= Quaternion.AngleAxis (m_angle * Time.deltaTime, m_axis);
	}
	
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		if(m_useLocalYAxis)
			Gizmos.DrawRay (transform.position, transform.up * 10);	
		else
			Gizmos.DrawRay (transform.position, m_axis * 10);	
	}
}
