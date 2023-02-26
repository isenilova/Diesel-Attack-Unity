using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifter : MonoBehaviour {

    float t = 4;
    // Use this for initialization
    float t0 = 0;
    bool isLift = false;
    float spd = 1.0f;

    public IEnumerator StartLift()
    {

        for (int i = 0; i < 6000; i ++)
        {
            yield return null;

            transform.position += new Vector3(0, spd*Time.deltaTime, 0);
        }

    }
	// Update is called once per frame
	void Update ()
    {

        t0 += Time.deltaTime;
        if (t0 > t && !isLift)
        {
            isLift = true;
            StartCoroutine(StartLift());
        }

	}
}
