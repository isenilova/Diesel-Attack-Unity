using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllInput : MonoBehaviour
{

	public GameObject map;
	public GameObject inventory;


	// Update is called once per frame
	void Update () 
	{

		var scene = SceneManager.GetActiveScene();
		
		if (scene.name == "new_start")
		{
			return;	
		}
		

		
		
		
	}
}
