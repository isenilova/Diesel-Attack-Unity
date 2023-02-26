using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbagerDown : MonoBehaviour
{
	private string state = "move";

	public float selfSpd = 10;

	public float openEvery = 5;

	public float openAngle = 100;

	public float openWait = 1;
	public float closeTime = 0.3f;
	public float openTime = 2;
	
	public float timeThrow = 3;
	public float timeReturn = 1;
	
	public float timeStayOpen = 2;


	public float loYdlt = 1;
	private float curY = 0;

	public Transform cable;
	
	public Transform kovsh;

	public Transform leftK;

	public Transform rightK;
	// Use this for initialization
	private float t = 0;

	public AudioClip contOpen;
	public AudioClip contClose;
	
	public IEnumerator Open()
	{
		
		if (contOpen != null)
		{
			AudioSource.PlayClipAtPoint(contOpen, Camera.main.transform.position);
		}
		
		float angl = 0;
		float dsp = openAngle / (openTime / Time.deltaTime);
		float t = 0;
		yield return new WaitForSeconds(openWait);
		
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

		//state = "bros";
		//StartCoroutine(Throwing());
	}

	public IEnumerator Throwing(float tm, float dir = 1)
	{

		float ep = 0;
		if (dir > 0)
		{
			ep = CamBound.instance.loy.position.y + loYdlt;
		}
		else
		{
			ep = curY;
		}

		float t = 0;
		while (t < tm)
		{
			float throwSpeed = Mathf.Abs(kovsh.position.y - ep) / (tm - t);
			kovsh.transform.position += new Vector3(0,-dir * throwSpeed * Time.deltaTime,0);
			t += Time.deltaTime;
			cable.localScale = new Vector3(cable.localScale.x, Mathf.Abs(kovsh.localPosition.y) * 3.42f, cable.localScale.z);
			yield return null;
		}

		if (dir > 0)
		{
			yield return new WaitForSeconds(timeStayOpen);

			state = "closing";

			StartCoroutine(Closing());
		}
		else
		{
			state = "move";
		}
	}
	
	public IEnumerator Closing()
	{
		
		if (contClose != null)
		{
			AudioSource.PlayClipAtPoint(contClose, Camera.main.transform.position);
		}
		
		float angl = 0;
		float dsp = openAngle / (closeTime / Time.deltaTime);
		float t = 0;
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
		state = "returning";
		StartCoroutine(Throwing(timeReturn, -1));
	}

	void Start()
	{
		GetComponent<MoveControl>().addSpeed = selfSpd;
		curY = kovsh.position.y;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (state == "move")
		{
			t += Time.deltaTime;

			if (t > openEvery)
			{
				t = 0;
				state = "throwing";
				StartCoroutine(Open());
				StartCoroutine(Throwing(timeThrow));
				
			}
		}
	}
}
