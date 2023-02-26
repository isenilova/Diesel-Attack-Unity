using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidPlatform : MonoBehaviour
{

	float heightHalf = 3;
	public float diro = 1;
	private bool opened = false;
	public GameObject view;
	public float appearTime = 0;
	
	public string state = "move";

	public float selfSpd = 10;

	public float openEvery = 5;

	private float openAngle = 100;

	public float openWait = 1;
	public float closeTime = 0.3f;
	public float openTime = 2;
	
	public float timeThrow = 3;
	private float timeReturn = 1;
	
	private float timeStayOpen = 2;


	public float loYdlt = 1;
	private float curY = 0;

	public Transform cable;
	public Transform cable2;
	
	public Transform kovsh;

	private Transform leftK;

	private Transform rightK;
	// Use this for initialization
	private float t = 0;

	public bool destroyLeft = false;
	public bool destroyRight = false;
	public bool destroyStay = false;
	public bool destroyAndBonus = false;
	public GameObject bonus;

	public GameObject[] doors;
	public GameObject[] explosions;
	public Transform leftSw;
	public Transform rightSw;
	public GameObject blockingCol;
	public Transform center;

	public Material matSwap;

	private float cabScale = 3.42f;
	private float cabTexScale = 1f;

	public void CustomDeath()
	{
		StopAllCoroutines();

		for (int i = 0; i < doors.Length; i++)
		{
			doors[i].SetActive(true);
		}
		
		for (int i = 0; i < explosions.Length; i++)
		{
			explosions[i].SetActive(true);
		}

		if (matSwap != null && !destroyLeft && !destroyRight)
		{
			kovsh.GetComponent<MeshRenderer>().material = matSwap;
		}
		

		if (destroyLeft)
		{
			cable.gameObject.SetActive(false);
			StartCoroutine(StartSwing(leftSw, 0));

		}
		
		if (destroyRight)
		{
			cable2.gameObject.SetActive(false);
			StartCoroutine(StartSwing(rightSw, 1));

		}

		if (destroyStay || destroyAndBonus)
		{
			blockingCol.SetActive(false);
		}

		if (destroyAndBonus)
		{
			var go = (GameObject) Instantiate(bonus, center.position, Quaternion.identity);
		}
	}

	public IEnumerator StartSwing(Transform point, int left)
	{
		point.SetParent(point.parent.parent);
		kovsh.SetParent(point);

		float grLo = 50;
		float dir = 1;
		
		
		if (left == 0)
		{
			dir = -1;
		}
		
		
		
		float angl = 0;
		float spd = 1;
		while (angl < grLo)
		{
			yield return null;
			point.transform.Rotate(0, diro * dir * spd, 0, Space.Self);
			angl += spd;
		}
		
		/*
		while (Mathf.Abs(grLo - grHi) > 5)
		{
			
		}
		*/
		
		yield return null;
	}

	public IEnumerator Open()
	{
		float angl = 0;
		float dsp = openAngle / (openTime / Time.deltaTime);
		float t = 0;
		yield return new WaitForSeconds(openWait);
		
		/*
		while (t < openTime)
		{
			leftK.Rotate(0,0,dsp,Space.Self);
			rightK.Rotate(0,0,-dsp,Space.Self);
			
			//checking
			var f1 = leftK.localEulerAngles.z;
			if (f1 < 0)
			{
				leftK.localEulerAngles = new Vector3(leftK.localEulerAngles.x, leftK.localEulerAngles.y, 0);
				rightK.localEulerAngles = new Vector3(rightK.localEulerAngles.x, rightK.localEulerAngles.y, 0);
				break;
			}
			
			t += Time.deltaTime;
			angl += dsp;
			yield return null;

		}
		*/

		//state = "bros";
		//StartCoroutine(Throwing());
	}

	public IEnumerator Throwing(float tm, float dir = 1)
	{

		float ep = 0;
		if (dir > 0)
		{
			ep = curY - loYdlt;
		}
		else
		{
			ep = curY + loYdlt;
		}

		float t = 0;
		while (t < tm)
		{
			float throwSpeed = Mathf.Abs(kovsh.position.y - ep) / (tm - t);
			kovsh.transform.position += new Vector3(0,-dir * throwSpeed * Time.deltaTime,0);
			t += Time.deltaTime;
			cable.localScale = new Vector3(cable.localScale.x, Mathf.Abs(kovsh.localPosition.y) * cabScale, cable.localScale.z);
			cable2.localScale = new Vector3(cable2.localScale.x, Mathf.Abs(kovsh.localPosition.y) * cabScale, cable2.localScale.z);
			
			cable.GetComponent<Renderer>().material.mainTextureScale = new Vector2(1 , cabTexScale * Mathf.Abs(kovsh.localPosition.y) /  cable.localScale.x);
			cable2.GetComponent<Renderer>().material.mainTextureScale = new Vector2(1 , cabTexScale * Mathf.Abs(kovsh.localPosition.y) /  cable2.localScale.x);

			if (dir > 0 && kovsh.position.y < ep) break;
			if (dir < 0 && kovsh.position.y > ep) break;
			
			
			yield return null;
		}


			yield return new WaitForSeconds(timeStayOpen);

			state = "closing";

			//StartCoroutine(Closing());

	}
	
	public IEnumerator Closing()
	{
		float angl = 0;
		float dsp = openAngle / (closeTime / Time.deltaTime);
		float t = 0;

		yield return null;
		/*
		while (t < closeTime)
		{
			leftK.Rotate(0,0,-dsp,Space.Self);
			rightK.Rotate(0,0,dsp,Space.Self);	
			
			//checking
			var f1 = leftK.localEulerAngles.z;
			if (f1 < 0)
			{
				leftK.localEulerAngles = new Vector3(leftK.localEulerAngles.x, leftK.localEulerAngles.y, 0);
				rightK.localEulerAngles = new Vector3(rightK.localEulerAngles.x, rightK.localEulerAngles.y, 0);
				break;
			}
			
			t += Time.deltaTime;
			angl += dsp;
			yield return null;

		}

		leftK.localEulerAngles = new Vector3(leftK.localEulerAngles.x, leftK.localEulerAngles.y, 0);
		rightK.localEulerAngles = new Vector3(rightK.localEulerAngles.x, rightK.localEulerAngles.y, 0);
		*/
		state = "returning";
		//StartCoroutine(Throwing(timeReturn, -1));
	}

	/*
	void Start()
	{
		GetComponent<MoveControl>().addSpeed = selfSpd;
		curY = kovsh.position.y;
	}
	*/

	private void OnDrawGizmos()
	{
		Gizmos.DrawCube(kovsh.position + new Vector3(0, - diro *loYdlt - diro * heightHalf, 0 ), Vector3.one);
	}

	// Update is called once per frame
	void Update () 
	{
		if (state == "move")
		{
			t += Time.deltaTime;

			if (!opened && t > appearTime)
			{
				opened = true;
				view.SetActive(true);
				GetComponent<MoveControl>().addSpeed = selfSpd;
				curY = kovsh.position.y;				
			}

			if (t > appearTime + openEvery)
			{

				t = -100000;
				state = "throwing";
				//StartCoroutine(Open());
				StartCoroutine(Throwing(timeThrow, diro));
				
			}
		}
	}
}
