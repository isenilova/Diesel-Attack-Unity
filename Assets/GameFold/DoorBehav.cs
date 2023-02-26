using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehav : MonoBehaviour {

    bool isOpen = false;
    bool isClose = false;

    float dOpen = 3f;
    float dClose = -1f;

    float dMax = 4;
    float savedY;

    private void Start()
    {
        savedY = transform.position.y;
    }

    public void Open()
    {
        if (isOpen) return;
        isOpen = true;
    }

    private void Update()
    {
        if (isOpen)
        {
            if (transform.position.y - savedY > dMax)
            {
                isOpen = false;
                isClose = true;
                return;
            }
            else
            {
                transform.position += new Vector3(0, dOpen * Time.deltaTime, 0);
            }
        }

        if (isClose)
        {
            if (transform.position.y < savedY)
            {
                isOpen = false;
                isClose = false;
                return;
            }
            else
            {
                transform.position += new Vector3(0, dClose * Time.deltaTime, 0);
            }
        }
    }

}
