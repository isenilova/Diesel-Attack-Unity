using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freezer : MonoBehaviour
{

	float freezeTime = 2.0f;
	private bool isFreezed = false;
	

	public void Freeze()
	{
		if (isFreezed)
		{
			return;
		}

		StartCoroutine(DoFreeze());
	}

	public IEnumerator DoFreeze()
	{
		isFreezed = true;
		GetComponent<MoveControl>().useInp = false;
		
		//we also start blinking
		
		//yield return new WaitForSeconds(freezeTime);
		float t = 0;
		float t1 = 0f;
		float dTime = 0.2f;
		while (t < freezeTime)
		{
			t1 -= Time.deltaTime;
			if (t1 < 0)
			{
				GetComponentInChildren<OneHealth>().DoDamage(0.01f);
				t1 = dTime;
			}

			t += Time.deltaTime;
			yield return null;
		}

		
		
		GetComponent<MoveControl>().useInp = true;
		isFreezed = false;
	}
	
}
