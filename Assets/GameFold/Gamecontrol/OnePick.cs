using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnePick : MonoBehaviour
{


	public string id;
	public string prefab;
	public float duration = 5.0f;
	public float speedModify = 5.0f;
	public float attakSpeedModify = 0.5f;
	public float projSpeedModify = 2;

	public float health = 100;
	// Use this for initialization
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag != "Player")
		{
			return;
		}

		var ft = other.GetComponentInParent<OneShip>().id;
		GameController.instance.AcceptPick(ft, id, prefab, duration, speedModify, attakSpeedModify, health, projSpeedModify);
		Destroy(gameObject);
	}
}
