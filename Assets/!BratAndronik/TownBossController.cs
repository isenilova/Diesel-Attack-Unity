using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownBossController : MonoBehaviour
{
    public string myState = "empty";
    
    public float appearTime = 5f;
    
    public float stopTime = 7f;

    public float appearx = 10f;
    public float appeary = 10f;
    public float addSpeedOnAppear = -7f;
    public float addspeadOnStop = 1f;
    public float addSreedOnBarrel = 5f;
    public float ballelKilledTimer = 3f;

    public float TimeForChange = 5f;
    public float GunSpeed = 0.1f;
    public float GunRunTimer = 1f;


    public float shakeTime = 2f;

    




    public GameObject myBoss;
    public GameObject myGun;
    private MoveControl moveScr;
    private TownOpen openScr;

    public GameObject damBarrel1;
    public GameObject damBarrel2;
    private OneHealth bar0Scr;
    private OneHealth bar1Scr;


    public GameObject[] ChangeMatObj;
    private ChangeMatByCall matsScr;


    public GameObject LasersObj;
    
    
    // Start is called before the first frame update
    void Start()
    {
        moveScr = gameObject.GetComponentInParent<MoveControl>();

        openScr = gameObject.GetComponent<TownOpen>();

    }

    // Update is called once per frame
    void Update()
    {
        if((TimeController.instance.tm > appearTime)&&(myState == "empty")) myAppear();
        
        if((TimeController.instance.tm > stopTime)&&(myState == "appear")) myStop();



        if (myState == "open")
        {
            StartCoroutine("Lasers");     
            damBarrel1.SetActive(true);

            bar0Scr = damBarrel1.GetComponent<OneHealth>();
            myState = "lasers";
        }

        if (myState == "lasers")
        {
            if (bar0Scr.curHealth <= 0) myState = "barrel";


        }


        if (myState == "barrel")
        {
            
            ChangeMats();
            KilledBarrel();
        }


        if (myState == "gungo")
        {
            
            damBarrel1.SetActive(false);
            damBarrel2.SetActive(true);
            damBarrel2.transform.position = damBarrel1.transform.position;
            bar1Scr = damBarrel2.GetComponent<OneHealth>();
            
            myState = "gun";
        }
        //if(myState == "pannel") Pannel();
        
        
        if (myState == "gun")
        {
            if (bar1Scr.curHealth <= 0) myState = "end";


        }

        if (myState == "end")
        {
            damBarrel2.SetActive(false);
            ToEnd();
        }

    }



    void myAppear()
    {
        
        FindObjectOfType<CameraShake>().ShakeCamera2(shakeTime);
        
        myBoss.SetActive(true);
        gameObject.transform.parent.transform.position = new Vector3(CamBound.instance.hix.transform.position.x+ appearx, appeary, 0f);
        moveScr.addSpeed = addSpeedOnAppear;

        
        myState = "appear";
    }

    void myStop()
    {


        moveScr.addSpeed = addspeadOnStop;
        
        openScr.stickMove("open", "both");
        myState = "stop";
    }

    IEnumerator Lasers()
    {
        LasersObj.SetActive(true);

        yield return 0;
    }


    void Pannel()
    {
        myState = "change";
        openScr.stickMove("down", "top");

        StartCoroutine("GoGun");
    }

    void KilledBarrel()
    {
        LasersObj.SetActive(false);
        
        myState = "change";
        openScr.stickMove("turn", "barrel");
        
        moveScr.addSpeed = addSreedOnBarrel;
        StartCoroutine("NormalSpeed");

    }

    IEnumerator NormalSpeed()
    {
        float time = 0f;

        while (time < ballelKilledTimer)
        {
            time += Time.deltaTime;
            yield return null;

        }

        moveScr.addSpeed = addspeadOnStop;   

        yield return null;
    }


    IEnumerator GoGun()
    {
        float alpha = 0;

        while (alpha< TimeForChange)
        {


            alpha += Time.deltaTime;

            yield return null;
        }
        
        myGun.SetActive(true);

        alpha = 0f;
        
        while (alpha< GunRunTimer)
        {
            myGun.transform.Translate(GunSpeed, 0f, 0f);

            alpha += Time.deltaTime;

            yield return null;
        }

        myState = "fight";

        yield return null;
    }


    void ToEnd()
    {
        //openScr.Barrels[1].SetActive(false);
        //StartCoroutine("HideGun");
        
        openScr.stickMove("close", "bot");

        myState = "win";
    }


    IEnumerator HideGun()
    {

       float alpha = 0f;
        
        while (alpha< GunRunTimer)
        {
            myGun.transform.Translate(-GunSpeed, 0f, 0f);

            alpha += Time.deltaTime;

            yield return null;
        }

        openScr.stickMove("close", "bot");
        yield return null;
    }


    void ChangeMats()
    {
        for (int i = 0; i < ChangeMatObj.Length; i++)
        {
            matsScr = ChangeMatObj[i].GetComponent<ChangeMatByCall>();
            matsScr.ChangeMatFunc(0);



        }


    }

}
