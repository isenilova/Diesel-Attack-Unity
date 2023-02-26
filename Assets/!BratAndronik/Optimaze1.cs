using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Optimaze1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if((transform.position.x < (CamBound.instance.lox.transform.position.x - 1))||  (transform.position.x > (CamBound.instance.hix.transform.position.x + 1)) 
           
           || (transform.position.y < (CamBound.instance.loy.transform.position.y - 1)) || (transform.position.y > (CamBound.instance.hiy.transform.position.y + 1))) Destroy(gameObject);
        
        
    }
}
