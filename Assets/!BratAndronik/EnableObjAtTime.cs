using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObjAtTime : MonoBehaviour
{
    public GameObject[] toEnab;

    public float timeApp = 0f;

    private bool myApp = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(myApp) return;


        if (TimeController.instance.tm >= timeApp)
        {
            myApp = true;
            
            
            for(int i =0; i < toEnab.Length; i++) toEnab[i].SetActive(true);



        }


    }
}
