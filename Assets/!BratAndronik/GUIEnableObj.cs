using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIEnableObj : MonoBehaviour
{
    public GameObject ActObj;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void onMyClick()
    {
        
        ActObj.SetActive(true);
        
    }
}
