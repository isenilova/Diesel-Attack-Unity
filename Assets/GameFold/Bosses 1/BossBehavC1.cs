using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavC1 : MonoBehaviour {

    public Transform lo;
    public Transform high;

    public Vector3 spd =  new Vector3(0, 10, 0);
    public float movingTime = 5;
    public float stayTime = 6.0f;

    bool isMoving = true;
    float t = 0;
    float dir = 1;

    public AllShoot[] shooters;
	// Use this for initialization
	void Start ()
    {
        int r = 1;
        //reset shooter ?
        for (int i = 2 * r; i < 2 * r + 2; i++)
        {
            shooters[i].enabled = true;
        }
    }

    public void OffShooters()
    {
        for (int i = 0; i < shooters.Length; i++)
        {
            shooters[i].enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        t += Time.deltaTime;

        if (isMoving && t > movingTime)
        {
            isMoving = false;
            int r = 0;
            //reset shooter ?
            OffShooters();
            for (int i = 2 * r; i < 2 * r + 2; i++)
            {
                shooters[i].enabled = true;
            }
            t = 0;

        }

        if (isMoving)
        {
            var newVec = transform.position + dir * spd * Time.deltaTime;
            if (newVec.y > high.position.y)
            {
                dir *= -1;
            }

            if (newVec.y < lo.position.y)
            {
                dir *= -1;
            }
            newVec = transform.position + dir * spd * Time.deltaTime;

            transform.position = newVec;

        }

        if (!isMoving && t > stayTime)
        {
            OffShooters();
            t = 0;
            isMoving = true;

            int r = 1;
            //reset shooter ?
            for (int i = 2 * r; i < 2 * r + 2; i++)
            {
                shooters[i].enabled = true;
            }
        }
	}
}
