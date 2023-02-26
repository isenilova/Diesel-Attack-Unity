using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveParabolic : MonoBehaviour
{

    public float mySpeed = 0.3f;

    public float myAngel = 45f;

    private float deltax;

    private Rigidbody myPhis;

    public Vector3 myVell = new Vector3(0f,0f,0f);


    public float lifeTime = 5f;

    public bool useForward = false;
    
    // Start is called before the first frame update
    void Start()
    {
        myPhis = gameObject.GetComponent<Rigidbody>();
        //myPhis.velocity = new Vector3(-mySpeed*Mathf.Cos(myAngel* Mathf.PI/180f), mySpeed*Mathf.Sin(myAngel* Mathf.PI/180f), 0);
        if (useForward) myVell = transform.forward;
        
        myPhis.velocity = mySpeed * myVell;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        
        if(lifeTime < 0f) Destroy(gameObject);


        
        if(transform.childCount == 0) Destroy(gameObject);
        
        
        // transform.Translate();

    }
}
