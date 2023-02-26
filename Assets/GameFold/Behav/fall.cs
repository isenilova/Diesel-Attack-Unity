using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fall : MonoBehaviour {

    public float t;
    public float sx;
    public float sy;

    float t0;

    public bool useDeath = false;

    private void Update()
    {
        t0 += Time.deltaTime;

        if (t0 < t) return;

        transform.position += new Vector3(sx * Time.deltaTime, sy * Time.deltaTime, 0);

        if (useDeath)
        {
            if (transform.position.x < -100 || transform.position.x > 100 || transform.position.y < -100 || transform.position.y > 100)
            {
                Destroy(gameObject);
            }
        }
    }
}
