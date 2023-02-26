using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavC2 : MonoBehaviour
{

	public ShootControl[] shooters;
	public RotatePong[] pongs;
	
	// Update is called once per frame
	public void ActivateShooters()
	{
		for (int i = 0; i < shooters.Length; i++)
		{
			shooters[i].enabled = true;
		}
		
		for (int i = 0; i < pongs.Length; i++)
		{
			pongs[i].enabled = true;
		}
		
	}

}
