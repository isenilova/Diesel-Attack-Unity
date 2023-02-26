using System.Collections;
using System.Collections.Generic;
using Colorful;
using UnityEngine;

public class GrayFader : MonoBehaviour
{

	public OneHealth[] oh;
	private bool isDone = false;

	public Grayscale gs;
	// Use this for initialization
	void Start () {
		
	}

	public IEnumerator StartFade()
	{
		float f = 0f;
		while (f < 1)
		{
			gs.Amount = f;
			yield return null;

			f += 0.01f;
		}
	}

	public bool CheckHealthDead()
	{
		bool q = true;
		for (int i = 0; i < oh.Length; i++)
		{
			if (oh[i] != null && oh[i].gameObject.activeInHierarchy && oh[i].curHealth > 0) q = false;
		}

		return q;
	}
	// Update is called once per frame
	void Update () 
	{
		if (!isDone && CheckHealthDead())
		{
			isDone = true;
			StartCoroutine(StartFade());
		}
	}
}
