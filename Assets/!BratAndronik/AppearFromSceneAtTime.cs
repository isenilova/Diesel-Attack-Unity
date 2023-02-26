using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearFromSceneAtTime : MonoBehaviour
{
    public float appearTime = 5f;

    public string myState = "empty";

    private MoveControl moveScr;

    public float addSpeedOnAppear = 0f;

    public float appearx = 0f;
    public float appeary = 0f;

    public GameObject[] activeOnAppear;
    
    // Start is called before the first frame update
    void Start()
    {
        moveScr = gameObject.GetComponent<MoveControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if((TimeController.instance.tm > appearTime) && (TimeController.instance.tm < (appearTime + 0.5f))&&(myState == "empty")) myAppear();
    }
    
    
    void myAppear()
    {
       
        gameObject.transform.position = new Vector3(CamBound.instance.hix.transform.position.x+ appearx, appeary, 0f);
        moveScr.addSpeed = addSpeedOnAppear;

        
        myState = "appear";


      if(activeOnAppear != null) { 
          for (int i = 0; i < activeOnAppear.Length; i++)
        {
            
            
           if(activeOnAppear[i] != null) activeOnAppear[i].SetActive(true);
        }
          
      }

    }
    
}
