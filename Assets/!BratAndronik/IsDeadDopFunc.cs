using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsDeadDopFunc : MonoBehaviour
{
    private OneHealth healthScr;

    public GameObject[] destroyObj;
    
    // Start is called before the first frame update
    void Start()
    {
        healthScr = gameObject.GetComponent<OneHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if(healthScr.curHealth > 0) return;
        
        
        for(int i =0; i < destroyObj.Length; i++)
            
            Destroy(destroyObj[i]);
        
        
        
    }
}
