using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Otbiv : MonoBehaviour
{
	private bool done = false;
	public float coefSelf = 1.0f;
	public float coefPlayer = 1.0f;
	public float maxSpeed = 200f;
	
	public void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log(collision.tag);
		if (done) return;

		if (collision.tag == "otbiv")
		{
			//we should calculate speeds
			//based on impulse
			
			done = true;
			float spd = Mathf.Abs(GetComponentInParent<MoveControl>().addSpeed);
			spd = spd / Mathf.Sqrt(2);

			var adv = collision.GetComponentInParent<MoveControl>().GetSpd();
			var fd = adv.magnitude;

			if (spd + fd * coefSelf < maxSpeed)
			{
				GetComponentInParent<MoveControl>().addSpeed = -(spd + fd * coefSelf);
				GetComponentInParent<MoveControl>().addSpeedy = (spd + fd * coefSelf);
			}
			else
			{
				GetComponentInParent<MoveControl>().addSpeed = -maxSpeed;
				GetComponentInParent<MoveControl>().addSpeedy = maxSpeed;
				
				
			}

			if (-spd * 10 * coefPlayer < maxSpeed)
			{
				collision.GetComponentInParent<MoveControl>().hitSpeed =
					new Vector3(-spd * 10 * coefPlayer, -spd * 10 * coefPlayer, 0);
			}
			else
			{
				collision.GetComponentInParent<MoveControl>().hitSpeed =
					new Vector3(-maxSpeed, -maxSpeed, 0);
				
			}
		}
	}

}
