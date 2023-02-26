using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyh : MonoBehaviour {

    public float spd = 1;
    float dir = 1;

    RaycastHit2D rh;



	void Update ()
    {

        rh = Physics2D.Raycast(transform.position, Vector2.right * dir, 1f, 1 << LayerMask.NameToLayer("Platforms"));

        if (rh.collider != null)
        {
            dir *= -1;
            return;
        }

        transform.position += new Vector3(spd * Time.deltaTime * dir, 0, 0);

    }
}
