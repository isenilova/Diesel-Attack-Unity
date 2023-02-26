using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twicker : MonoBehaviour {

    float spd = 100.0f;
    float dir = 1.0f;
	// Update is called once per frame
	void Update ()
    {
        Vector3 vv = transform.localPosition + new Vector3(0, dir * spd * Time.deltaTime, 0);
        transform.localPosition = transform.localPosition + new Vector3(0, dir * spd * Time.deltaTime, 0);

        if (vv.y > 67 || vv.y < -15)
        {
            dir *= -1;
        }
        else
        {
            transform.localPosition = vv;
        }
	}
}
