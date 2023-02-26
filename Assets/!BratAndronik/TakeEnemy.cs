using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeEnemy : MonoBehaviour
{
    public float spaumTimer = 2f;
    public float ShotTimer = 1f;
    public float AfterShot = 0.1f;

    private bool firstSpaum = true;
    
    
    public float spaumTM = 0f;
    public float shotTm = 0f;

    public bool isSpaum = true;


    public GameObject myBullet;

    private GameObject curBul;



    public GameObject bullParrent;
    public GameObject newParrent;

    private Rigidbody bullBody;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (isSpaum)
        {
            shotTm += Time.deltaTime;

            if (shotTm >= ShotTimer)
            {
                shotTm = 0f;


                    ////


                isSpaum = false;
                curBul.transform.parent = newParrent.transform;
                bullBody = curBul.GetComponent<Rigidbody>();
                bullBody.useGravity = true;
                
                curBul = null;
            }


        }

        else
        {
            spaumTM += Time.deltaTime;
            
            
            if (firstSpaum)
            {
                if (spaumTM >= spaumTimer)
                {
                    spaumTM = 0f;

                    isSpaum = true;



                    curBul = Instantiate(myBullet, bullParrent.transform);


                    firstSpaum = false;

                }

            }
            else
            {
                
                if (spaumTM >= (spaumTimer+AfterShot))
                {
                    spaumTM = 0f;

                    isSpaum = true;



                    curBul = Instantiate(myBullet, bullParrent.transform);


                    

                }
                
                
                
            }


        }

    }
}
