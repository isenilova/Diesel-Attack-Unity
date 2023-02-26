using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBlock : MonoBehaviour
{
	private float iniScale = 65;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		RaycastHit2D rh;
		if (rh = Physics2D.Raycast(transform.position, -transform.right, 2 * iniScale, 1 << LayerMask.NameToLayer("block")))
		{
			float dst = (rh.point - new Vector2(transform.position.x, transform.position.y)).magnitude;
			//Debug.Log(dst);
			transform.localScale = new Vector3(dst + 2, transform.localScale.y, transform.localScale.z);
		}
		else
		{
			transform.localScale = new Vector3(iniScale, transform.localScale.y, transform.localScale.z);
		}
	}
}
