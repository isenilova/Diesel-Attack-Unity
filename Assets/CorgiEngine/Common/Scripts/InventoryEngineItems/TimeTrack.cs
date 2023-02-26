using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeTrack : MonoBehaviour {

    public Text txt;
	
	// Update is called once per frame
	void Update ()
    {
        txt.text = "Time: " + System.Math.Round(TimeController.instance.tm, 2).ToString();
	}
}
