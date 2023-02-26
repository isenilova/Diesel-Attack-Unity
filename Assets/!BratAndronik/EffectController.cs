using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{

public float LifeTimer = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        LifeTimer -= Time.deltaTime;
        if(LifeTimer <= 0f) Destroy(gameObject);


    }
}
