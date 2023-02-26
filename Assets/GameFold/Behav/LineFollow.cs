using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineFollow : MonoBehaviour {

	// Use this for initialization
	public Vector3 vec;
	public float max = 2;
	public Vector3 pos;
	public float dir = 1;
	public float mul = 1;
	public float div = 1;

	private float t = 0;
	
	
	// Update is called once per frame
	void Update ()
	{

		t += Time.deltaTime;

		Vector3 p = pos + vec * t * mul;
		Vector3 n = new Vector3(-vec.y, vec.x, 0);
		n.Normalize();

		var np = p + dir * n * Mathf.Sin(t * div);
		transform.position = np;
		
		//get next coord to see transform
		Vector3 p1 = pos + vec * (t + Time.deltaTime) * mul;
		var np1 =  p1 + dir * n * Mathf.Sin((t + Time.deltaTime) * div);

		transform.up = np1 - np;
	}
}
