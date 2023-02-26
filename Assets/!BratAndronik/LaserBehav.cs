using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class LaserBehav : MonoBehaviour
{
    public float startTimer = 1f;

    public float overtimeTimer = 5f;
    private float myTimer;

    public float mySpeed = 0.05f;

    public float maxScale = 25f;

    public float lifeTimeronMax = 1f;


    public GameObject myLight;
    private LightByCall lightSsr;
    public  float LightTimerBefore = 0.5f;
    private bool lighton = false;


   
    private Vector3 myStartPosition;

    private Quaternion myStartRotation;
    
   Vector3 myEndPosition = new Vector3(0f, 0f, 0f);
    public GameObject myEndObjPos;
   
    
    // Start is called before the first frame update
    void Start()
    {
        myTimer = overtimeTimer + 0.1f;

        myStartPosition = transform.localPosition;

        myStartRotation = transform.localRotation;


    }

    // Update is called once per frame
    void Update()
    {

        if (((overtimeTimer - myTimer) <= LightTimerBefore) && (!lighton))
        {
            lighton = true;


            if (myLight != null)
            {
                lightSsr = myLight.GetComponent<LightByCall>();
                
                lightSsr.onMyLight();


            }


        }



        startTimer -= Time.deltaTime;
        if(startTimer >0f) return;


        myTimer += Time.deltaTime;

        if (myTimer > overtimeTimer)
        {
            myTimer = 0f;
            lighton = false;
            StartCoroutine("myScale");


        }
    }


    IEnumerator myScale()
    {
        
        
       transform.localScale = new Vector3(transform.localScale.x, 0f, transform.localScale.z);

        float tim = 0f;
        


        while (transform.localScale.y < maxScale)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + mySpeed*Time.deltaTime, transform.localScale.z);
            
            yield return null;
        }

        while (tim < lifeTimeronMax)
        {
            tim += Time.deltaTime;
            yield return null;

        }
        
        transform.Rotate(0f, 0f, 180f, Space.Self);

       // Debug.Break();

        if (myEndObjPos != null)
            transform.position = myEndObjPos.transform.position;
        else transform.position = myEndPosition;
        
       // transform.localScale = new Vector3(transform.localScale.x, 0f, transform.localScale.z);
        
        
        while (transform.localScale.y >= 0.05f)
        {
            
            if((transform.localScale.y - mySpeed*Time.deltaTime)> 0f)
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - mySpeed*Time.deltaTime, transform.localScale.z);

            else
            {
                transform.localScale = new Vector3(transform.localScale.x, 0f, transform.localScale.z);
            }
            
            yield return null;
        }

        
       
        transform.localPosition = myStartPosition;
        transform.localRotation = myStartRotation;
        

        yield return null;
        
        //Debug.Break();
    }
    
}
