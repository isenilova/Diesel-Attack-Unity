using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detonato : MonoBehaviour
{

	public float tm = 2.0f;

	public GameObject mina;

	private float t;

	public GameObject explEffect;


	public bool multyMina = false;
	public GameObject[] minas;

	public bool useZZ = false;

	private void Update()
	{
		t += Time.deltaTime;
		if (t > tm)
		{

			if (!multyMina)
			{
				var go = Instantiate(mina);
				go.transform.position = transform.position;
			}

			if (multyMina)
			{

				for (int i = 0; i < minas.Length; i++)
				{
					GameObject min = null;

					if (useZZ)
					{
						min = Instantiate(minas[i]);
						min.transform.position = transform.position;
					}
					else
					{
						min = Instantiate(minas[i], transform.position, Camera.main.transform.rotation);						
					}


					if(min.GetComponent<Detonato>() != null) min.GetComponent<Detonato>().enabled = false;

				}
			}

			if (explEffect != null) Instantiate(explEffect, transform.position, transform.rotation);
			
			Destroy(gameObject);
		}
	}
}
