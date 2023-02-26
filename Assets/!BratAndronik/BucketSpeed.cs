using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketSpeed : MonoBehaviour
{
    private MoveControl moveScr;

    public float newBucketSpeed = 1f;

    private OneHealth lifeScr;

    private bool isChange = false;
    
    // Start is called before the first frame update
    void Start()
    {
        moveScr = transform.parent.GetComponent<MoveControl>();

        lifeScr = gameObject.GetComponent<OneHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((!isChange) && (lifeScr.curHealth <= 0f))
        {

            isChange = true;


            moveScr.addSpeed = newBucketSpeed;

        }


    }
}
