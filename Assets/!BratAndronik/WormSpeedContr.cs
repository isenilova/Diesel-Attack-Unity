using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormSpeedContr : MonoBehaviour
{
    private float curhealf;
    private float dlthp = 0f;

    public GameObject myPlr;
    public float maxdist = 100f;

    public float dmgSpdCoef = 0.05f;

    public float minspeed = -1f;
    
    // Start is called before the first frame update
    void Start()
    {
        curhealf = gameObject.GetComponent<OneHealth>().curHealth;
        myPlr = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        if( curhealf == gameObject.GetComponent<OneHealth>().curHealth) return;


        if ((transform.position - myPlr.transform.position).magnitude > maxdist)
        {
            curhealf = gameObject.GetComponent<OneHealth>().curHealth;
            return;
        }
        
        

        if (curhealf > gameObject.GetComponent<OneHealth>().curHealth)
        {
            dlthp =  curhealf - gameObject.GetComponent<OneHealth>().curHealth;
            curhealf = gameObject.GetComponent<OneHealth>().curHealth;

            transform.parent.gameObject.GetComponent<MoveControl>().addSpeed -= dlthp * dmgSpdCoef;

            if (transform.parent.gameObject.GetComponent<MoveControl>().addSpeed < minspeed)
                transform.parent.gameObject.GetComponent<MoveControl>().addSpeed = minspeed;


        }

    }
}
