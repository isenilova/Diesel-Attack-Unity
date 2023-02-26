using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FithLvlWormGearController : MonoBehaviour
{
    public GameObject[] sideGears;
    public float[] sideGearTimers;

    private int lastExplod = 0;

    public GameObject BigGear;
    public float bigDerTimer = 10f;
    private bool bigGearflag = false;

    public GameObject RowGear;
    public float RowGearTimer = 10f;
    private bool rowgearflag = false;



    public GameObject[] matToChange;

    public float[] matTimer;
    public Material damMat;
    private int lastMatChange = 0;


    public Material damMatBack;
    public float Mat2Timer = 5f;
    public GameObject mat2Obj;
    private bool chMat2 = false;
    


    public float[] shakeAtTime;
    private int lastShake = 0;

    private GameObject myCam;


    public GameObject[] EffectsToAct;
    public float[] effTimers;
    private int lastEff = 0;

    // Start is called before the first frame update
    void Start()
    {
        myCam = GameObject.FindGameObjectWithTag("MainCamera");
       
    }

    // Update is called once per frame
    void Update()
    {
        if (lastExplod < sideGears.Length)
        {
            if (TimeController.instance.tm > sideGearTimers[lastExplod])
            {
                sideGears[lastExplod].GetComponent<OneHealth>().curHealth = -0.01f;
                lastExplod++;


            }
        }
        
        
        if (lastEff < EffectsToAct.Length)
        {
            if (TimeController.instance.tm > effTimers[lastEff])
            {
               EffectsToAct[lastEff].SetActive(true);
                lastEff++;


            }
        }

        if (lastShake < shakeAtTime.Length)
        {
            if (TimeController.instance.tm > shakeAtTime[lastShake])
            {
                myCam.GetComponent<CameraShake>().ShakeCamera();
                lastShake++;


            }
        }

        if (!bigGearflag)
        {
            if (TimeController.instance.tm > bigDerTimer)
            {

                BigGear.GetComponent<Animator>().CrossFadeInFixedTime("brocken", 0.1f);

                bigGearflag = true;

            }


        }
        
        
        if (!chMat2)
        {
            if (TimeController.instance.tm > Mat2Timer)
            {

                mat2Obj.GetComponent<Renderer>().material = damMatBack;

                chMat2 = true;

            }


        }
        
        if (!rowgearflag)
        {
            if (TimeController.instance.tm > RowGearTimer)
            {

                RowGear.GetComponent<Animator>().CrossFadeInFixedTime("brocken", 0.1f);

               rowgearflag = true;

            }


        }
        
        
        if (lastMatChange < matToChange.Length)
        {
            if (TimeController.instance.tm > matTimer[lastMatChange])
            {

                matToChange[lastMatChange].GetComponent<Renderer>().material = damMat;

                lastMatChange++;

            }


        }


    }

    }

