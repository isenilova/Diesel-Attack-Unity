using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleDeath : MonoBehaviour
{
    private OneHealth lifeScr;
    private MoveControl moveScr;


    public float fallSpeed = 0.1f;

    public float yToFall = -10f;


    public float leftSpeed = 0.2f;
    

    private bool dead = false;
    private bool fallen = false;

    private bool moveOff = false;


    private GameObject mCam;
    private CameraShake shScr;
    public float ShEffectTime = 15f;


    public GameObject myDeathEffects;


    public float FallDelay = 4f;
    
    // Start is called before the first frame update
    void Start()
    {
        lifeScr = gameObject.GetComponent<OneHealth>();

        moveScr = gameObject.GetComponent<MoveControl>();
        
        
        mCam = GameObject.FindGameObjectWithTag("MainCamera");

        shScr = mCam.GetComponent<CameraShake>();

    }

    // Update is called once per frame
    void Update()
    {

        if ((lifeScr.curHealth <= 0.0001f)&&(!dead))
        {
            
            shScr.ShakeCamera2(ShEffectTime);
            
            myDeathEffects.SetActive(true);
            
            StartCoroutine("FallFafe");
            dead = true;


        }


        if (fallen&& !moveOff)
        {

            moveOff = true;

            moveScr.enabled = false;

            StartCoroutine("LeftFade");

        }

    }


    IEnumerator FallFafe()
    {
        while (FallDelay >= 0f)
        {
            FallDelay -= Time.deltaTime;

            yield return null;



        }



        while (transform.position.y > yToFall)
        {
            transform.Translate(0, -50*Time.deltaTime*fallSpeed, 0);

            yield return null;

        }


        fallen = true;


    }

    IEnumerator LeftFade()
    {
        while (true)
        {

            transform.Translate(50*Time.deltaTime*leftSpeed, 0, 0);

            yield return null;
        }
        
        
        
    }

}
