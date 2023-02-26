using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardBoss : MonoBehaviour {

    public Transform eyePosition;

    public GameObject[] stars;
    public GameObject oneStar;

    public GameObject spike;

    public GameObject rotLaser;

    float tMax = 6.0f;
    float t = 0;
    float tMax1 = 4.0f;
    float t1 = 0;




    float alpha = 0;
    float spd = 1.0f;

    string state = "idle";
    int starCnt = 50;
    float dst = 1.0f;

    RaycastHit2D rh;


    public GameObject player;


    public GameObject[] points;
    float travelSpd = 1.0f;
    int nextTrvl = 0;


    private void Start()
    {
        for (int i = 0; i < starCnt; i++)
        {
            stars[i] = (GameObject)Instantiate(oneStar);
            stars[i].transform.position = new Vector3(10000, 10000, 10000);
        }

        player = GameObject.FindGameObjectWithTag("Player");

        points = GameObject.FindGameObjectsWithTag("point");

    }

    public void Retranslate(Vector2 end)
    {
        Vector2 tp = new Vector2(transform.position.x, transform.position.y);
        int cnt = (int) ((end - tp).magnitude / dst);

        var dlt = (end - tp) / cnt;
        //Debug.Log(cnt);

        for (int j = cnt; j < starCnt; j++)
        {
            stars[j].transform.position = new Vector3(10000, 10000, 10000);
        }

        var dlt1 = new Vector3(dlt.x, dlt.y, 0);

        for (int j = 0; j < cnt; j++ )
        {
            stars[j].transform.position = transform.position + dlt1 * j;
        }

    }

    public void Hide()
    {
        for (int j = 0; j < starCnt; j++)
        {
            stars[j].transform.position = new Vector3(10000, 10000, 10000);
        }
    }

    public void PrepareSet()
    {
        //we make 5 spikes and send them
        var dlt = player.transform.position - transform.position;
        dlt.Normalize();
        Vector2 dlt1 = new Vector2(-dlt.y, dlt.x);
        dlt1.Normalize();

        for (int i = -2; i < 3; i++)
        {
            var go = (GameObject)Instantiate(spike);
            go.transform.position = transform.position + new Vector3(dlt1.x * i, dlt1.y * i, 0);
            go.transform.up = dlt;
            go.GetComponent<fall>().sx = dlt.x * 6;
            go.GetComponent<fall>().sy = dlt.y * 6;

        }
    }

    // Update is called once per frame
    void Update ()
    {

        t += Time.deltaTime;
        t1 += Time.deltaTime;

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        //we travel 
        {

            if (points.Length == 0)
            {
                points = GameObject.FindGameObjectsWithTag("point");
            }
            else
            {
                var dt = (points[nextTrvl].transform.position - transform.position).magnitude;
                if (dt < spd * Time.deltaTime)
                {
                    transform.position = points[nextTrvl].transform.position;
                    nextTrvl = (nextTrvl + 1) % points.Length;
                }
                else
                {
                    transform.position += (points[nextTrvl].transform.position - transform.position).normalized * spd * Time.deltaTime;
                }
            }
                    
        }

        /*
        if (t > tMax)
        {
            t = 0;
            state = "laser";
            alpha = 0;
        }
        */

        if (t1 > tMax1)
        {
            t1 = 0;
            for (int k = 0; k < 6; k++)
                Invoke("PrepareSet", k * 0.2f);
        }

        /*
        if (Input.GetKeyDown("g"))
        {
            state = "laser";
            alpha = 0;
        }

        if (Input.GetKeyDown("h"))
        {
            //we make 5 spikes and send them
            var dlt = player.transform.position - transform.position;
            dlt.Normalize();
            Vector2 dlt1 = new Vector2(-dlt.y, dlt.x);
            dlt1.Normalize();

            for (int i = -2; i < 3; i++)
            {
                var go = (GameObject)Instantiate(spike);
                go.transform.position = transform.position + new Vector3(dlt1.x * i, dlt1.y * i, 0);
                go.transform.up = dlt;
                go.GetComponent<fall>().sx = dlt.x * 6;
                go.GetComponent<fall>().sy = dlt.y * 6;

            }

        }
        */


        if (state == "laser")
        {
            alpha += spd;
            rotLaser.SetActive(true);
            
            if (alpha > 360)
            {
                rotLaser.SetActive(false);
                //Hide();
                state = "idle";
                return;
            }

            /*
            float rad = 1.0f;
            float rd = Mathf.PI * alpha / 180.0f;
            Vector2 dir = new Vector2(rad * Mathf.Sin(rd), rad * Mathf.Cos(rd));

            rh = Physics2D.Raycast(transform.position, dir, 100f, 1 << LayerMask.NameToLayer("Platforms"));

            if (rh.collider != null)
            {
                //retranslate stars

                Retranslate(rh.point);

            }
            */

        }
		
	}
}
