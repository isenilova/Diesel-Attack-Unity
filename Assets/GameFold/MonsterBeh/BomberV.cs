using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberV : MonoBehaviour {

    float range = 5;
    float dir = 1;

    public Vector3 spd = new Vector3(1, 0, 0);

    public float spdx = -3;
    public float spdy = 0;
    // Use this for initialization
    public bool useBomb = false;
    public GameObject bomb;
    Vector3 savedPos;

    public float angRot = 90;
    public float initRot = 0;

    public float dropTime = 2.0f;
    float dt = 0;

    private void Start()
    {
        savedPos = transform.position;
        transform.Rotate(0, initRot, 0);
    }

    // Update is called once per frame
    void Update ()
    {
        dt += Time.deltaTime;

        /*
		if (transform.position.y + spd * Time.deltaTime * dir > savedPos.y + range)
        {
            dir *= -1;
            //transform.forward = -transform.forward;
            return;
        }

        if (transform.position.y + spd * Time.deltaTime * dir < savedPos.y - range)
        {
            dir *= -1;
            //transform.forward = -transform.forward;
            return;
        }
        */


        transform.position += spd * Time.deltaTime * dir;

        if (dt > dropTime && useBomb)
        {
            dt -= dropTime;
            var go = (GameObject)Instantiate(bomb, transform.position, Quaternion.identity);
            go.transform.Rotate(0, 0, angRot);
            //go.GetComponent<fall>().sx = spdx;
            //go.GetComponent<fall>().sy = spdy;

        }
    }
}
