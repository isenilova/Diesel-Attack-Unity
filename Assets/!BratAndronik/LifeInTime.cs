using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeInTime : MonoBehaviour
{

    public float LifeTime = 60f;
    private bool death = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(death) return;

        LifeTime -= Time.deltaTime;

        if (LifeTime < 0)
        {
            death = true;

            gameObject.GetComponent<OneHealth>().curHealth = -0.1f;


        }

    }
}
