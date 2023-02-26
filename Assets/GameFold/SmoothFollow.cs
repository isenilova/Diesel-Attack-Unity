using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour {

    public float speed = 10.0f;
    public float rotSpeed = 10.0f;

    public GameObject plr;
    public Transform forw;
    // Use this for initialization
	void Start ()
    {
        plr = GameObject.FindGameObjectWithTag("Player");
        
        if(plr == null) return;
        forw.transform.localPosition = new Vector3(forw.transform.localPosition.x, forw.transform.localPosition.y, 0);

	}
	
	// Update is called once per frame
	void Update ()
    {
        if (plr == null)
        {
            Start();
            return;
        }

        int q = 0;
        float dist = (forw.position - plr.transform.position).magnitude;

        float rad = (forw.position - transform.position).magnitude;

        float angl = rotSpeed * Time.deltaTime / (2 * Mathf.PI);

        float dx1 = forw.position.x - transform.position.x;
        float dy1 = forw.position.y - transform.position.y;


        float nx = dx1 * Mathf.Cos(angl) - dy1 * Mathf.Sin(angl);
        float ny = dx1 * Mathf.Sin(angl) + dy1 * Mathf.Cos(angl);

        Vector3 np = new Vector3(transform.position.x + nx, transform.position.y + ny, transform.position.z); 

        if ((np - plr.transform.position).magnitude < dist)
        {
            q = 1;
            dist = (np - plr.transform.position).magnitude;
        }

        nx = dx1 * Mathf.Cos(-angl) - dy1 * Mathf.Sin(-angl);
        ny = dx1 * Mathf.Sin(-angl) + dy1 * Mathf.Cos(-angl);

        np = new Vector3(transform.position.x + nx, transform.position.y + ny, transform.position.z);


        if ((np - plr.transform.position).magnitude < dist)
        {
            q = 2;
            dist = (np - plr.transform.position).magnitude;
        }
        
        //Debug.Log(q);

        if (q == 1)
        {
            transform.Rotate(0, 0, -rotSpeed * Time.deltaTime);
        }
        else if (q == 2)
        {
            transform.Rotate(0, 0, +rotSpeed * Time.deltaTime);
        }

        var vec = (forw.position - transform.position).normalized;
        transform.position += speed * Time.deltaTime * vec;
    }
}
