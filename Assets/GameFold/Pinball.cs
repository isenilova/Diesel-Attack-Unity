using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinball : MonoBehaviour {

    public float spdx = 1;
    public float spdy = 1;
    float str = 0.50f;
    float tl = 0.1f;
    float t = 0;
    RaycastHit2D rh;
	//
    // Update is called once per frame
	void Update ()
    {
        t += Time.deltaTime;
        rh = Physics2D.Raycast(transform.position, Vector2.right, 1f * str, 1 << LayerMask.NameToLayer("Platforms"));

        if (rh.collider != null && t > tl)
        {
            t = 0;
            spdx *= -1;
            return;
        }

        rh = Physics2D.Raycast(transform.position, Vector2.left, 1f * str, 1 << LayerMask.NameToLayer("Platforms"));

        if (rh.collider != null && t > tl)
        {
            t = 0;
            spdx *= -1;
            return;
        }

        rh = Physics2D.Raycast(transform.position, Vector2.up, 1f * str, 1 << LayerMask.NameToLayer("Platforms"));

        if (rh.collider != null && t > tl)
        {
            t = 0;
            spdy *= -1;
            return;
        }


        rh = Physics2D.Raycast(transform.position, Vector2.down, 1f * str, 1 << LayerMask.NameToLayer("Platforms"));

        if (rh.collider != null && t > tl)
        {
            t = 0;
            spdy *= -1;
            return;
        }


        transform.position += new Vector3(spdx, spdy, 0) * Time.deltaTime;

    }
}
