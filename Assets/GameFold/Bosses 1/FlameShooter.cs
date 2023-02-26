using System.Collections;
using System.Collections.Generic;
using FXV;
using UnityEngine;

public class FlameShooter : MonoBehaviour
{
	private float epsilonSpaum = 0.5f;
	public float tm = 0;

	public GameObject proj;
	public GameObject efct;

	private bool activated = false;

	public float moveTime = 5.0f;

	public float addSpeed = 2.0f;
	public float sinT = 3.0f;
	public float ampl = 3;
	public float koef = 1;
	public float tStay = 2.0f;

	private float savedSpd;

	public AnimationCurve anCur;

	public int shootNum = 2;


	public bool spawnLeft = false;
	
	// Use this for initialization
	public IEnumerator Moveo()
	{
		float t = moveTime;
		while (t > 0)
		{
			t -= Time.deltaTime;
			yield return null;
		}
		
		//shoot
		savedSpd = GetComponent<MoveControl>().addSpeed;
		GetComponent<MoveControl>().addSpeed = Camera.main.GetComponent<MoveControl>().addSpeed;

		
		StartCoroutine(DoShoot(-1, 0));

	}

	public IEnumerator DoShoot(float dir, int cnt)
	{
		var go = Instantiate(proj);
		go.transform.position = transform.position;

		if (efct != null)
		{
			var c = Instantiate(efct, transform);
			c.transform.position = transform.position - new Vector3(0.1f, 0, 0);
		}
		
		go.GetComponent<fall>().sx =
			dir * go.GetComponent<fall>().sx + Camera.main.GetComponent<MoveControl>().addSpeed;
		yield return null;

		GetComponent<MoveControl>().addSpeed = Camera.main.GetComponent<MoveControl>().addSpeed - dir * addSpeed;
		float savedY = transform.position.y;
		float t = sinT;
		while (t > 0)
		{
			t -= Time.deltaTime;
			var kf = anCur.Evaluate(1 - t / sinT);
			GetComponent<MoveControl>().addSpeed = Camera.main.GetComponent<MoveControl>().addSpeed - dir * addSpeed * kf;
			transform.position = new Vector3(transform.position.x,  savedY + ampl * Mathf.Sin((sinT - t) * koef), transform.position.z);
			yield return null;
		}

		GetComponent<MoveControl>().addSpeed = Camera.main.GetComponent<MoveControl>().addSpeed;
		
		yield return new WaitForSeconds(tStay);

		if (cnt < shootNum - 1)
		{
			dir = -1;
			StartCoroutine(DoShoot(dir, cnt + 1));
		}
		else
		{
			GetComponent<MoveControl>().addSpeed = savedSpd;
		}


	}
	
	// Update is called once per frame
	void Update () {

		if (TimeController.instance.tm > tm && !activated && (TimeController.instance.tm < tm + epsilonSpaum))
		{
			activated = true;
			transform.position = new Vector3(CamBound.instance.hix.position.x, transform.position.y, transform.position.z);
			
			if(spawnLeft) transform.position = new Vector3(CamBound.instance.lox.position.x, transform.position.y, transform.position.z);
			
			StartCoroutine(Moveo());
		}
		
		if (!activated) return;


	}
}
