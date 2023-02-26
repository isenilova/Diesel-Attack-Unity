using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beton2 : MonoBehaviour
{
    private float tm = 2f;
    private bool sp = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tm -= Time.deltaTime;
        
        if(tm > 0) return;
        
        transform.GetChild(1).gameObject.SetActive(true);
        sp = true;

    }
}
