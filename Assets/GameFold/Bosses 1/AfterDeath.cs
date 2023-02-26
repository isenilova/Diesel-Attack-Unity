using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterDeath : MonoBehaviour
{
	public float deltaY = 1.0f;
	public Transform whale;

	public bool jp = false;
	public bool disCol = false;

	public Collider2D[] cols;
	
	public void Do()
	{
		if (jp)
		{
			GetComponent<SpiderSmallSpawn>().Update();
			return;
		}

		if (disCol)
		{
			for (int i = 0; i < cols.Length; i++)
			{
				if (cols[i] != null)
				{
					Destroy(cols[i]);
				}
			}
		}
		
		StartCoroutine(FallDown());
	}

	public IEnumerator FallDown()
	{
		FindObjectOfType<CameraShake>().ShakeCamera();
		whale.GetComponent<MoveControl>().speedY = -5;
		whale.GetComponent<MoveControl>().addSpeed -= 3;
		
		while (true)
		{
			yield return null;

			if (whale.transform.position.y - deltaY < CamBound.instance.loy.position.y)
			{
				//now moving horisontaly
				whale.GetComponent<MoveControl>().speedY = 0;
				whale.GetComponent<MoveControl>().addSpeed -= 3;
				break;

			}
		}
	}
}
