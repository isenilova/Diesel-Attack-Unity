using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grav : MonoBehaviour {

    public float t;

    float t0;
    bool isAdded = false;

    private void Update()
    {
        t0 += Time.deltaTime;

        if (t0 < t) return;

        if (isAdded) return;

        gameObject.AddComponent<Rigidbody2D>();
        isAdded = true;
    }
}
