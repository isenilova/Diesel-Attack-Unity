using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour {

    public static TimeController instance;

    public float tm = 0;

	public float fixTime;
	
	

    private void Awake()
    {
        instance = this;
	    tm = DoRestart.curTime;
    }

	
	// Update is called once per frame
	void Update () {
        //772973074780
		if (tm < fixTime) tm = fixTime;

        tm += Time.deltaTime;

		if (Input.GetKeyDown("1"))
		{
			//if (Time.timeScale == 1) Time.timeScale = 10;
			//else Time.timeScale = 1;
		}
		
	}

	
}
