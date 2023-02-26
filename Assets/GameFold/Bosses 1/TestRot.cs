using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRot : MonoBehaviour
{

	public GameObject cube;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{

		if (Input.GetKeyDown("b"))
		{
			Debug.Log(cube.transform.GetChild(0).position);
			cube.transform.Translate(0,0,30);
			Debug.Log(cube.transform.GetChild(0).position);
		}
		
		if (Input.GetKeyDown("a"))
		{
			Debug.Log(cube.transform.GetChild(0).position);
			cube.transform.Rotate(0,0,30);
			Debug.Log(cube.transform.GetChild(0).position);
		}
		
	}
}
