using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderRun : MonoBehaviour
{

    public float mySpeed = 20f;
    public float deltaChangeDir = 4f;
    private float tm = 0;
    private bool rot = false;
    private Vector3 newForw = new Vector3(0, 0, 0);
    private float a;
    public float rotSpeed = 0.5f;
    Vector3 savedForw= new Vector3(0,0,0);
    private float angel = 0.01f;
    
    // Start is called before the first frame update
    void Start()
    {
        //ok almost all
        //transform.right = -Camera.main.transform.forward;

        if (transform.forward.z > 0.1f)
        {
            transform.Rotate(0, 90, 0);
        }
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * mySpeed * Time.deltaTime;

        tm += Time.deltaTime;

        if ((tm >= deltaChangeDir) && !rot)
        {
            rot = true;
            tm = 0;


            a = Random.Range(0.2f, 0.99f);

            newForw = a * transform.forward + (1 - a) * transform.up;
           

            StartCoroutine(RotSpider());

        }


    }


    IEnumerator RotSpider()
    {


        while ((1 - Mathf.Abs((transform.forward.x * newForw.x + transform.forward.y * newForw.y +
                     transform.forward.z * newForw.z) / (newForw.magnitude * transform.forward.magnitude))) > angel)
        {
            
          

           transform.forward = (1-rotSpeed*Time.deltaTime*0.01f) * transform.forward + (rotSpeed*Time.deltaTime*0.01f) * newForw;

           // transform.right = -Camera.main.transform.forward;
            
            
            if (transform.right.z > 0)
            {
                transform.Rotate(0, 0, 180);
            }
            
            
            yield return null;

        }


        rot = false;
        yield return null;
    }
}
