using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheater : MonoBehaviour
{
    public OneHealth oh;

    public GameObject neck;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("g"))
        {
            //oh.curHealth = 0;
            neck.tag = "Exploder";
            ExplControl.instance.ExplodeObject(neck);  
            Debug.Break();
        }
    }
}
