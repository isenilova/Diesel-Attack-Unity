using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMove : MonoBehaviour
{

	public float curLen = 0;
	public float len = 5;

	public float spd = 20;

	public Vector3 vec;

	public float liveTime = 5.0f;

	private float t = 0;

	private bool isdone = false;
	// Use this for initialization

	// Update is called once per frame
	void Update ()
	{

		t += Time.deltaTime;

		if (t > liveTime)
		{
			Destroy(gameObject);
		}
		
		var inP = transform.position + spd * vec.normalized * Time.deltaTime;


		Vector3 ep = inP + vec.normalized * len;
		
		transform.position = inP;

		if ((ep.y > CamBound.instance.hiy.position.y && !isdone) || (ep.y < CamBound.instance.loy.position.y && !isdone))
		{
			isdone = true;

			var go = (GameObject) Instantiate(gameObject);
			go.transform.position = ep;
			go.transform.right = new Vector3(vec.x, -vec.y, 0);
			go.GetComponent<LaserMove>().vec = new Vector3(vec.x, -vec.y, 0);
			go.GetComponent<LaserMove>().len = len;
			go.GetComponent<LaserMove>().spd = spd;
			go.GetComponent<LaserMove>().liveTime = liveTime - t;
		}
	}
}
