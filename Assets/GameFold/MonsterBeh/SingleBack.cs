using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleBack : MonoBehaviour
{

	public float appearTime;

	public float selfSpd = -10;

	private float t = 0;

	private bool isDone = false;
	// Use this for initialization
	
	// Update is called once per frame
	void Update ()
	{
		t += Time.deltaTime;
		
		if (isDone) return;


		if (t > appearTime)
		{
			isDone = true;
			transform.GetChild(0).gameObject.SetActive(true);
			GetComponent<MoveControl>().addSpeed = selfSpd;
		}
	}
}
