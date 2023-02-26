using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedTracker : MonoBehaviour {

    public static SpeedTracker instance;

    public float spd = 0;
    public float speedV;
    public Vector3 prevPos = Vector3.zero;

    private void Awake()
    {
        instance = this;
    }


    void Update () {

        var go = GameObject.FindGameObjectWithTag("Player");

        if (go != null)
        {
            if (prevPos == Vector3.zero)
            {
                prevPos = go.transform.position;
                return;
            }

            //Debug.Log(go.GetComponent<Rigidbody2D>().velocity);
            spd = go.transform.position.y - prevPos.y;
            prevPos = go.transform.position;

            //if (spd != 0)
            //     Debug.Log(spd);

            if (spd >= 0)
            {
                if (speedV != 0)
                {
                    //Debug.Log("---------------------" + speedV.ToString());
                }

                speedV = 0;
            }
            else
            {
                speedV += spd;
            }

            //
            //Debug.Log(spd);
        }

	}
}
