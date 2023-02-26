using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEffects : MonoBehaviour
{
    public float myScale = 10f;

    public bool forwScl = false;

    public GameObject forwObj;

    private Vector3 doBig;
    private Vector3 doSmall;

    private Vector3 myNormalScale;
    
    public bool upScl = false;

    public GameObject upObj;
    public GameObject upObj2;

    private Vector3 updoBig;
    private Vector3 updoSmall;

    private Vector3 upmyNormalScale;



    public float speedToChange = 0.1f;
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        doBig = new Vector3(1, 1, myScale);
        doSmall = new Vector3(1, 1, 1/myScale);
        
        myNormalScale = new Vector3(1, 1, 1);
        
        updoBig = new Vector3(1, 1, myScale);
        updoSmall = new Vector3(1, 1, 1/myScale);
        
        upmyNormalScale = new Vector3(1, 1, 1);

        if (forwObj != null)
        {
            myNormalScale = forwObj.transform.localScale;
            doBig = new Vector3(forwObj.transform.localScale.x, forwObj.transform.localScale.y, forwObj.transform.localScale.z*myScale);
            doSmall = new Vector3(forwObj.transform.localScale.x, forwObj.transform.localScale.y, forwObj.transform.localScale.z);
        }
        
        if (upObj != null)
        {
            upmyNormalScale = upObj.transform.localScale;
            updoBig = new Vector3(upObj.transform.localScale.x, upObj.transform.localScale.y, upObj.transform.localScale.z*myScale);
            updoSmall = new Vector3(upObj.transform.localScale.x, upObj.transform.localScale.y, upObj.transform.localScale.z);
        } 
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") > 0 /*Input.GetKey(KeyCode.RightArrow)*/)
        {
            if (!forwScl)
            {
                forwScl = true;
                if (forwObj != null) StartCoroutine("LargeScale");

            }


        }
        else
        {

            if (forwScl)
            {
                forwScl = false;
                if (forwObj != null) StartCoroutine("SmallScale");


            }



        }
        
        
        
        if (Input.GetAxis("Vertical") >0f)
        {
            if (!upScl)
            {
                upScl = true;
                if ((upObj != null)&&(upObj2 != null))
                {
                    StartCoroutine("UpLargeScale");

                }


            }


         
        }

        else
        {

            if (upScl)
            {
                upScl = false;
                if ((upObj != null)&&(upObj2 != null))
                {
                    StartCoroutine("UpSmallScale");
                }


            }



        }

    }


    IEnumerator LargeScale()
    {
       // Debug.Log("Enlarging started");
       // Debug.Log(forwObj.transform.localScale.z + " " + doBig.z + " " + forwScl);
        while ((forwObj.transform.localScale.z < doBig.z)&& (forwScl))
        {
            forwObj.transform.localScale += new Vector3(0,0, speedToChange*Time.deltaTime);
            //Debug.Log(forwObj.transform.localScale);
            
            
            
            yield return null;

        }



       if(forwScl) forwObj.transform.localScale = doBig;



        yield return null;
    }
    
    
    IEnumerator SmallScale()
    {
       // Debug.Log("De-larging started");
       // Debug.Log(forwObj.transform.localScale.z + " " + doSmall.z + " " + forwScl);
        while ((forwObj.transform.localScale.z > doSmall.z)&&(!forwScl))
        {
            forwObj.transform.localScale -= new Vector3(0,0, speedToChange*Time.deltaTime);

            yield return null;

        }



      if(!forwScl)  forwObj.transform.localScale = doSmall;



        yield return null;
    }
    
    
    IEnumerator UpLargeScale()
    {
        // Debug.Log("Enlarging started");
        // Debug.Log(forwObj.transform.localScale.z + " " + doBig.z + " " + forwScl);
        while ((upObj.transform.localScale.z < updoBig.z)&& (upScl))
        {
            upObj.transform.localScale += new Vector3(0,0, speedToChange*Time.deltaTime);
            
            upObj2.transform.localScale += new Vector3(0,0, speedToChange*Time.deltaTime);
            //Debug.Log(forwObj.transform.localScale);
            
            
            
            yield return null;

        }

        if (upScl)
        {
            upObj.transform.localScale = updoBig;
            upObj2.transform.localScale = updoBig;

        }



        yield return null;
    }
    
    
    IEnumerator UpSmallScale()
    {
        // Debug.Log("De-larging started");
        // Debug.Log(forwObj.transform.localScale.z + " " + doSmall.z + " " + forwScl);
        while ((upObj.transform.localScale.z > updoSmall.z)&&(!upScl))
        {
            upObj.transform.localScale -= new Vector3(0,0, speedToChange*Time.deltaTime);
            upObj2.transform.localScale -= new Vector3(0,0, speedToChange*Time.deltaTime);

            yield return null;

        }

        if (!upScl)
        {
            upObj.transform.localScale = updoSmall;
            upObj2.transform.localScale = updoSmall;
        }



        yield return null;
    }

    
    
    
    

}
