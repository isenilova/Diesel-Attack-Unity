using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwoDLaserPack;

public class EvilLaser : MonoBehaviour {

    public float tMax1 = 3;
    float tMax2 = 3;
    float t0;
    int state = 0;
    public float rot = 0;
    public float rotS = 0;

    private void Start()
    {
        transform.Rotate(0, 0, rot);
    }


    private void Update()
    {
        t0 += Time.deltaTime;

        if (t0 > tMax1 && state == 0)
        {
            //transform.GetChild(0).gameObject.SetActive(false);
            GetComponentInChildren<LineBasedLaser>().SetLaserState(false);
            state = 1;
            t0 = 0;
        }

        if (t0 > tMax2 && state == 1)
        {
            //transform.GetChild(0).gameObject.SetActive(true);
            GetComponentInChildren<LineBasedLaser>().SetLaserState(true);
            state = 0;
            t0 = 0;
        }

        transform.Rotate(0, 0, rotS * Time.deltaTime);

    }
}
