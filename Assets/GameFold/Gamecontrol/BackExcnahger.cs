using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Exchange
{
	public GameObject one;
	public GameObject two;
	public float t;

	public bool single = false;
	public bool once = false;
	public bool loop = true;
	public bool usePingPong = true;

	public int curO = 0;
	public bool doneOnce = false;
	public float curT = 0;
}

public class BackExcnahger : MonoBehaviour
{
	private float t = 0;

	public Exchange[] chnages;
	// Update is called once per frame
	void Update () 
	{

		for (int i = 0; i < chnages.Length; i++)
		{
			chnages[i].curT += Time.deltaTime;

			if (chnages[i].curT > chnages[i].t)
			{
				if (!chnages[i].loop && chnages[i].doneOnce)
				{
					continue;
				}

				if (chnages[i].single)
				{
					chnages[i].one.SetActive(true);
					chnages[i].one.GetComponent<BackScroll>().SetToNextScreen(chnages[i].once);
				}
				else
				{



					if (!chnages[i].usePingPong)
					{
						chnages[i].one.GetComponent<BackScroll>().MakeExcnahge(chnages[i].two);
					}
					else
					{
						if (chnages[i].curO == 0)
						{
							chnages[i].one.GetComponent<BackScroll>().MakeExcnahge(chnages[i].two);
						}
						else
						{
							chnages[i].two.GetComponent<BackScroll>().MakeExcnahge(chnages[i].one);
						}

						chnages[i].curO = (chnages[i].curO + 1) % 2;
					}
				}

				chnages[i].doneOnce = true;
				chnages[i].curT = 0;
			}
		}
	}
}
