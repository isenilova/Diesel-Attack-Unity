using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotControl : MonoBehaviour
{


	public Transform[] rotators;

	public float[] anglers = new float[2];
	public float[] dirs = new float[2]{-1, 1};

	private bool isFixed = false;

	public float spdRot = 2.0f;
	public float bound = 180;
	
	public string horAxis = "Horizontal";

	public Transform sock;
	// Use this for initialization
	void Start ()
	{
		spdRot *= 75;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
		var dx = Input.GetAxis(horAxis);
		//dy = Input.GetAxis("Vertical");

		if (dx != 0 && !isFixed)
		{
			for (int i = 0; i < rotators.Length; i++)
			{
				if (rotators[i].childCount > 0)
				{
					float r = anglers[i] + dx * spdRot * Time.deltaTime;
					Debug.Log(i.ToString() + " " + dx  + " " + r);
					if (r < 0 || r > bound) continue;
					
					anglers[i] += dx * spdRot * Time.deltaTime;
					rotators[i].Rotate(0,0, dirs[i] * dx * spdRot * Time.deltaTime, Space.Self);
				}
			}
		}

		if (Input.GetKeyDown("e") || Input.GetKeyDown("joystick button 3"))
		{
			isFixed = !isFixed;
		}
		
	}
}
