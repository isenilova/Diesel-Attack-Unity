using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGener : MonoBehaviour {

    public GameObject mon;
    public float t = 3.0f;
    float t0 = 0;

    private void Update()
    {
        t0 += Time.deltaTime;

        if (t0 > t)
        {
            var go = (GameObject)Instantiate(mon);
            go.transform.position = transform.position;
            t0 = 0;
        }
    }

}
