using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbagerUp : MonoBehaviour
{
	private string state = "move";

	public float selfSpd = 10;

	public float openEvery = 5;

	public float openAngle = 100;

	public float openTime = 2;

	public float timeStayOpen = 2;

	public GameObject[] effect;
	public GameObject[] smallEffect;

	public float timePrefire = 0.5f;

	public Transform leftK;

	public Transform rightK;
	// Use this for initialization
	private float t = 0;
	
	public AudioClip contOpen;
	public AudioClip contClose;
	
	public Vector3 rotVec = new Vector3(0, 0, 1);

	public IEnumerator Open()
	{
		if (contOpen != null)
		{
			AudioSource.PlayClipAtPoint(contOpen, Camera.main.transform.position);
		}
		
		float angl = 0;
		float dsp = openAngle / (openTime / Time.deltaTime);
		float t = 0;
		while (t < openTime)
		{
			leftK.Rotate(rotVec * dsp,Space.Self);
			rightK.Rotate(rotVec * -dsp,Space.Self);
			
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
			if (angl > openAngle)
			{
				leftK.Rotate(-rotVec * (angl - openAngle),Space.Self);
				rightK.Rotate(-rotVec * (angl - openAngle),Space.Self);
				break;
			}
			yield return null;

		}

		state = "bros";
		StartCoroutine(Throwing());
	}

	public IEnumerator Throwing()
	{
		if (smallEffect != null)
		{
			for (int i = 0; i < smallEffect.Length; i++)
				if (smallEffect[i] != null) smallEffect[i].SetActive(true);
		}
		
		yield return new WaitForSeconds(timePrefire);
		
		if (smallEffect != null)
		{
			for (int i = 0; i < smallEffect.Length; i++)
				if (smallEffect[i] != null) smallEffect[i].SetActive(false);
		}
		
		if (effect != null)
		{
			for (int i = 0; i < effect.Length; i++)
			 if (effect[i] != null) effect[i].SetActive(true);
		}

		yield return new WaitForSeconds(timeStayOpen);
		if (effect != null)
		{
			for (int i = 0; i < effect.Length; i++)
				if (effect[i] != null) effect[i].SetActive(false);
		}

		state = "closing";

		StartCoroutine(Closing());
	}
	
	public IEnumerator Closing()
	{
		if (contClose != null)
		{
			AudioSource.PlayClipAtPoint(contClose, Camera.main.transform.position);
		}
		
		float angl = 0;
		float dsp = openAngle / (openTime / Time.deltaTime);
		float t = 0;
		while (t < openTime)
		{
			leftK.Rotate(rotVec * -dsp,Space.Self);
			rightK.Rotate(rotVec * dsp,Space.Self);	
			
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
			if (angl > openAngle)
			{
				leftK.Rotate(rotVec * (angl - openAngle),Space.Self);
				rightK.Rotate(-rotVec * (angl - openAngle),Space.Self);
				break;
			}
			yield return null;

		}

		leftK.localEulerAngles = new Vector3(leftK.localEulerAngles.x, leftK.localEulerAngles.y, 0);
		rightK.localEulerAngles = new Vector3(rightK.localEulerAngles.x, rightK.localEulerAngles.y, 0);
		state = "move";
	}

	void Start()
	{
		GetComponent<MoveControl>().addSpeed = selfSpd;
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
				state = "opening";
				StartCoroutine(Open());
			}
		}
	}
}
