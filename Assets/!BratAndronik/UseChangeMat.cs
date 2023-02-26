using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseChangeMat : MonoBehaviour
{

    public float changeHp = 300f;
    private bool canChange = true;

    private OneHealth hpScr;

    public GameObject objToChange;

    private ChangeMatByCall matScr;
    
    // Start is called before the first frame update
    void Start()
    {
        hpScr = gameObject.GetComponent<OneHealth>();


        matScr = objToChange.GetComponent<ChangeMatByCall>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!canChange) return;
        
        
        if(hpScr.curHealth > changeHp) return;
        
        
        matScr.ChangeMatFunc(0);
        canChange = false;


    }
}
