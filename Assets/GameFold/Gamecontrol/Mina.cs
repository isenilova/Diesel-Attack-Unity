using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mina : MonoBehaviour
{

	public GameObject projectile;

	public int cnt;
	// Use this for initialization
	void Start ()
	{

		float ang = 360 / cnt;
		float curA = 0;
		for (int i = 0; i < cnt; i++)
		{
			var go = (GameObject) Instantiate(projectile);
			var sp = go.GetComponent<fall>().sx;
			go.transform.position = transform.position;
			go.GetComponent<fall>().sx = sp * Mathf.Sin(curA * Mathf.PI/ 180);
			go.GetComponent<fall>().sy = sp * Mathf.Cos(curA * Mathf.PI/ 180);

			curA += ang;

		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
