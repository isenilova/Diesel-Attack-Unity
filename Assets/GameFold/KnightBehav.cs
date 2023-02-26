using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBehav : MonoBehaviour {

    public Transform hand;

    float savedRot;
    GameObject player;
    float minDstX = 2.5f;
    float minDstY = 1.5f;

    string state = "idle";

    float rotSpd = 1;
    float maxRot = 100;

    int flip = 0;

    private void Start()
    {
        savedRot = hand.localEulerAngles.z;
        Debug.Log(savedRot);

        player = GameObject.FindGameObjectWithTag("Player");

    }


    // Update is called once per frame
    void Update ()
    {
		if (state == "idle")
        {
            var dlt = player.transform.position - transform.position;
            //Debug.Log(Mathf.Abs(dlt.y).ToString() + "    " + Mathf.Abs(dlt.x).ToString());
            if (Mathf.Abs(dlt.y) < minDstY && Mathf.Abs(dlt.x) < minDstX)
            {
                if (dlt.x < 0 && flip == 0)
                {
                    flip = 1;
                    transform.Rotate(0, 180, 0);
                }
                else if (flip == 1 && dlt.x > 0)
                {
                    flip = 0;
                    transform.Rotate(0, 180, 0);
                }

                state = "razmah";
                Debug.Log(state);
            }
            return;
        }

        if (state == "razmah")
        {
            hand.Rotate(0, 0, rotSpd);
            var rt = hand.localEulerAngles.z;

            if (rt > maxRot)
            {
                state = "udar";
                Debug.Log(state);
            }

            return;
        }

        if (state == "udar")
        {
            hand.Rotate(0, 0,-rotSpd);
            var rt = hand.localEulerAngles.z;
            //Debug.Log(rt);
            if (rt < 340 && rt > 338)
            {
                state = "return";
                Debug.Log(state);
            }

            return;
        }

        if (state == "return")
        {
            hand.Rotate(0, 0, rotSpd);
            var rt = hand.localEulerAngles.z;

            if (rt > savedRot && rt < 337)
            {
                state = "idle";
                Debug.Log(state);
            }

            return;
        }


    }
}
