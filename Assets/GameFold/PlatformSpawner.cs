using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour {

    public GameObject onePl;

    public float tMax = 3.0f;
    public float tDestr = 9;

    public float spdx = 0;
    public float spdy = -3;
    public float rot = 0;

    float t = 0;

    private void Start()
    {
        transform.Rotate(0, 0, rot);
    }

    private void Update()
    {
        t += Time.deltaTime;

        if (t > tMax)
        {
            t = 0;
            var go = (GameObject)Instantiate(onePl);
            go.transform.position = transform.position;

            go.transform.Rotate(0, 0, rot);
            go.GetComponent<fall>().sx = spdx;
            go.GetComponent<fall>().sy = spdy;


            Destroy(go, tDestr);
        }
    }

}
