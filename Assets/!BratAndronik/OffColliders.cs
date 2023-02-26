using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffColliders : MonoBehaviour
{

    public GameObject myGreen;

    private bool isOff = false;

    public GameObject[] myCollsOff;

    public GameObject[] myCollsOn;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((myGreen == null) && !isOff)
        {
            isOff = true;
            
          if(myCollsOff != null)  
            for(int i = 0; i < myCollsOff.Length; i++)  myCollsOff[i].SetActive(false);
            
            
            
            if(myCollsOn != null)
                for(int i = 0; i < myCollsOn.Length; i++) myCollsOn[i].SetActive(true);




        }



    }
}
