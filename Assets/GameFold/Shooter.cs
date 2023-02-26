using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {

    public GameObject player;
    public GameObject spike;

    float tMax = 3.0f;
    float t = 0;

    float trDist = 8.0f;

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
    // Use this for initialization
    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
    {

	
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            return;

        }

        t += Time.deltaTime;

        var dst = (transform.position - player.transform.position).magnitude;

        //Debug.Log(dst);
        if (dst < trDist && t > tMax)
        {
            t = 0;
            for (int k = 0; k < 6; k++)
                Invoke("PrepareSet", k * 0.2f);
        }
	}
}
