using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletDops : MonoBehaviour
{
    public float explTimer;
    public GameObject explEffect;
    private Mina minascr;
    private bool destroy = false;
    
    // Start is called before the first frame update
    void Start()
    {
        minascr = gameObject.GetComponent<Mina>();
    }

    // Update is called once per frame
    void Update()
    {
        if(destroy) Destroy(gameObject);
        
        explTimer -= Time.deltaTime;
        
        if(explTimer > 0f) return;


        if (explEffect != null) Instantiate(explEffect, transform.position, transform.rotation);
        minascr.enabled = true;

        destroy = true;

    }
}
