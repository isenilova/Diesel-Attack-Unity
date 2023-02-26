using MoreMountains.CorgiEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunCorrect : MonoBehaviour {

    public float rate = 2;
    public float spd = 6;
	// Use this for initialization
	void Start ()
    {

        GetComponentInChildren<PathedProjectileSpawner>().FireRate = rate;
        GetComponentInChildren<PathedProjectileSpawner>().Speed = spd;


    }


}
