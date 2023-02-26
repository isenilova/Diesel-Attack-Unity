using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
//using Steamworks;
using UnityEngine;

public class ThrashSpawner : MonoBehaviour
{


	public GameObject[] garbagers;
	public float delay = 3;
	public int numSpawn = 10;

	public float startTime = 30;
	
	private float t = 0;
	private int curSp = 0;
	private int curAll = 0;

	private bool isStarted = false;
	
	// Update is called once per frame
	void Update () 
	{
		if (TimeController.instance.tm > startTime && !isStarted)
		{
			isStarted = true;
			t = delay;
		}

		if (!isStarted) return;

		t -= Time.deltaTime;
		
		if (t > 0) return;
		
		if (curAll >= numSpawn) return;

		var go = (GameObject) Instantiate(garbagers[curSp]);
		go.transform.position = transform.position;

		t = delay;
		//curSp = (curSp + 1) % 2;
		curAll++;


	}
}
