using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtbivShower : MonoBehaviour
{

	public GameObject otbiv;
	
	// Update is called once per frame
	void Update () {

		if ((Input.GetKeyDown("z") ||  Input.GetKeyDown("joystick button 2")) && !otbiv.activeSelf)
		{
			//otbiv.SetActive(true);
		}
		
	}
}
