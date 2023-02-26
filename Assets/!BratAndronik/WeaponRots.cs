using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRots : MonoBehaviour
{
    private float delay = 0.5f;

    private bool done = false;

    public float newRotSpd = 2.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(done) return;
        delay -= Time.deltaTime;
        
        if(delay > 0f) return;

        GameObject.FindGameObjectWithTag("Player").GetComponent<RotControl>().spdRot = newRotSpd*75;

        done = true;


    }
}
