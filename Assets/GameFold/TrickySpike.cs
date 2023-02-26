using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickySpike : MonoBehaviour {


    public GameObject player;

    int state = 0;

    Vector3 savedPos;
    float distY = 6;
    float spd = -4;



    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        savedPos = transform.position;
    }

    private void Update()
    {

        if (player == null)
        {
            Start();
            return;
        }

        var dlt = player.transform.position - transform.position;

        if (Mathf.Abs(dlt.x) < 1.5f && -dlt.y < 10 && state == 0)
        {
            state = 1;

        }

        if (state == 1 && (savedPos.y - transform.position.y < distY))
        {
            transform.position += new Vector3(0, spd * Time.deltaTime, 0);
        }

        if (state == 1 && (savedPos.y - transform.position.y > distY))
        {
            state = 2;
            transform.Rotate(0, 0, 180);
        }

        if (state == 2 && savedPos.y > transform.position.y)
        {
            transform.position += new Vector3(0, -spd * Time.deltaTime, 0);
        }

        if (state == 2 && savedPos.y < transform.position.y)
        {
            transform.Rotate(0, 0, 180);
            state = 0;
        }
    }


}
