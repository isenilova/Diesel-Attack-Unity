using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIDisableObj : MonoBehaviour
{
    public GameObject DisObj;
    
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
        
        
        DisObj.SetActive(false);
        
        
    }


}
