using System.Collections;
using System.Collections.Generic;
using MoreMountains.CorgiEngine;
using UnityEngine;

public class DoDamage_CameraShake : DoDamage
{

    public float CamTimeEffect = 2f;
    private float myTimer;

    private GameObject mCam;
    private CameraShake shScr;

    void Start()
    {
        
        mCam = GameObject.FindGameObjectWithTag("MainCamera");
        shScr = mCam.GetComponent<CameraShake>();

    }


    // Start is called before the first frame update
    public override void Do(float val)
    {
        myTimer = 0f;

        shScr.ShakeCamera2(CamTimeEffect);

       //StartCoroutine("ShakeControl");

    }


    private IEnumerator ShakeControl()
    {
        while (myTimer <= CamTimeEffect)
        {
            myTimer += Time.deltaTime;
            yield return null;


           
        }

        
        yield return null;
    }

}