using System.Collections;
using System.Collections.Generic;
//using Steamworks;
using UnityEngine;

public class AppearObjAtTime : MonoBehaviour
{

    public GameObject[] pref;
    public float deltaTime = 10f;
    public float deltax = 1f;
    public float deltay = 1f;
   float starttime = 0f;

    public float MyStartTimer = 0f;
    

    public bool spLeft = false;
    public int maxCountSp = 50;
    private int cnr = 0;

    private int lastInstant = 0;

    private Vector3 posit = new Vector3(0,0,0);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MyStartTimer -= Time.deltaTime;
        if(MyStartTimer > 0f) return;
        
        
        if(cnr >= maxCountSp) return;
        
        
        starttime += Time.deltaTime;

        if (starttime >= deltaTime)
        {
            starttime = 0;

            if (!spLeft)
            {
                posit = new Vector3(CamBound.instance.hix.transform.position.x + deltax, deltay, 0f);
            }

            else
            {
                posit = new Vector3(CamBound.instance.lox.transform.position.x + deltax, deltay, 0f);
                
            }

            Instantiate(pref[lastInstant], posit, Quaternion.identity);


            lastInstant++;

            if (lastInstant >= pref.Length) lastInstant = 0;

            cnr++;


        }


    }
}
