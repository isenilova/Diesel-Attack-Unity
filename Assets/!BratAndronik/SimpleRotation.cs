using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotation : MonoBehaviour
{
    public float startDelay = 3f;

    public float maxangel = 45f;

    public float myspeed = 2f;

    private bool toup = true;
    private float myAngel = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        startDelay -= Time.deltaTime;
        if(startDelay > 0f) return;
        
        
        if (toup)
        {
            transform.Rotate(0, -myspeed, 0);

            myAngel += myspeed;


            if (myAngel > maxangel)
            {
                myAngel = 0f;
                toup = false;

            }



        }
        else
        {
            
            
            transform.Rotate(0, myspeed, 0);

            myAngel += myspeed;


            if (myAngel > maxangel)
            {
                myAngel = 0f;
                toup = true;

            }
            
            
            
            
            
        }



    }
}
