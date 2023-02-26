using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFly : MonoBehaviour
{

    public GameObject StartObj;
    public GameObject EndObj;

    public float delay = 2f;
    public float overtime = 5f;
    public float speed = 10f;


    private float tm = 0f;
    private float eps = 0.1f;
    private bool myFly = false;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        tm = overtime;
    }

    // Update is called once per frame
    void Update()
    {
        if (delay > 0f)
        {
            delay -= Time.deltaTime;
            return;

        }

        if(myFly) return;
        

        tm += Time.deltaTime;

        if (tm > overtime)
        {

            myFly = true;


            transform.position = StartObj.transform.position;

            
            tm = 0f;

            StartCoroutine(goFly());

            

        }


    }



    IEnumerator goFly()
    {

        while (Mathf.Abs(transform.position.x - EndObj.transform.position.x) > eps)
        {
            if (transform.position.x < EndObj.transform.position.x)
            {

                transform.Translate(speed*Time.deltaTime, 0f, 0f);

                yield return null;

            }
            else
            {

                transform.Translate(-speed*Time.deltaTime, 0f, 0f);


                yield return null;
            }


        }


        myFly = false;
        yield return null;
    }
}
