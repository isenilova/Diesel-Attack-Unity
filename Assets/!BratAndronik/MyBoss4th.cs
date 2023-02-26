using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MyBoss4th : MonoBehaviour
{
    public string myState = "empty";
    
    public float appearTime = 5f;
    
    public float stopTime = 7f;
    
    public float appearx = 0f;
    public float appeary = 0f;
    public float addSpeedOnAppear = 7f;
    public float addspeadOnStop = -1f;
    
    public float addspeadOnDeath = -5f;
    
    
    public float shakeTime = 2f;
    
    
    private MoveControl moveScr;


    public float timeridleAttack = 10f;
    public float timerAttack = 15f;
    public float timerDown = 15f;
    public float tm = 0;

    public bool barrelsKilled = false;


    public Material damMat;

    public bool dead = false;

    public GameObject[] barrels;

    public GameObject myHead;
    public GameObject myHeadBone;

    public GameObject explpref;

    public GameObject headGoal;

    public float startenergy = 1f;
    public float endenergy = 2.2f;
    public float startfire = 4.1f;
    public float endfire = 6f;

    public float attPartsTimer = 0f;

    public GameObject fireEff;
    public GameObject energyeff;

    public GameObject myBullets;

    public float startBullet = 1f;
    public float endBullet = 4f;
    private float att2bulletTimer = 0f;

    public Material CapsuleDam;

    public GameObject HeadToDis;
    
    
    // Start is called before the first frame update
    void Start()
    {
        moveScr = gameObject.GetComponent<MoveControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if((TimeController.instance.tm > appearTime)&&(myState == "empty")) myAppear();
        
        if((TimeController.instance.tm > stopTime)&&(myState == "appear")) myStop();


        if (myState == "idle")
        {
            tm += Time.deltaTime;

            if (tm > timeridleAttack) goAttack();
        
        }
        
        if (myState == "attack")
        {
            tm += Time.deltaTime;
            attPartsTimer += Time.deltaTime;
            
            if(attPartsTimer > startenergy) energyeff.SetActive(true);
            if(attPartsTimer > endenergy) energyeff.SetActive(false);
            if(attPartsTimer > startfire) fireEff.SetActive(true);
            if(attPartsTimer > endfire) fireEff.SetActive(false);

            if (tm > timerDown) goDown();
        
        }

        if (myState == "down")
        {
            tm += Time.deltaTime;

            if (tm > timerAttack) goAttack();
        
        }


        if (!barrelsKilled && CheckBarrels())
        {
            
            transform.GetChild(0).gameObject.GetComponent<Animator>().CrossFade("Idle", 0.1f);

            barrelsKilled = true;

            myState = "idle1";
            
            transform.GetChild(2).gameObject.SetActive(true);


            transform.GetChild(0).gameObject.GetComponentsInChildren<Renderer>().ToList().ForEach(x =>
            {
                if (x.tag != "NCM" && x.GetComponent<ParticleSystem>() == null) x.material = damMat;
            });
            transform.GetChild(1).gameObject.GetComponentsInChildren<Renderer>().ToList().ForEach(x => 
            {
                if (x.tag != "NCM" && x.GetComponent<ParticleSystem>() == null) x.material = damMat;
            });
            
            headGoal.SetActive(true);



            for (int i = 0; i < barrels.Length; i++) barrels[i].GetComponent<Renderer>().material = CapsuleDam;

        }
        
        if (myState == "idle1")
        {
            tm += Time.deltaTime;

            if (tm > timeridleAttack) goAttack1();
        
        }
        
        if (myState == "attack1")
        {
            tm += Time.deltaTime;
            
            
            att2bulletTimer += Time.deltaTime;
            
            if(att2bulletTimer  > startBullet) myBullets.SetActive(true);
            if(att2bulletTimer  > endBullet) myBullets.SetActive(false);

            if (tm > timerDown) goDown1();
        
        }

        if (myState == "down1")
        {
            tm += Time.deltaTime;

            if (tm > timerAttack) goAttack1();
        
        }


        if (CheckHead() && barrelsKilled && !dead)
        {
            dead = true;

            myState = "dead";

            StartCoroutine(myDeath());
        }


    }
    
    
    void myAppear()
    {
        
        FindObjectOfType<CameraShake>().ShakeCamera2(shakeTime);
        
       transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        
        
        gameObject.transform.position = new Vector3(CamBound.instance.lox.transform.position.x+ appearx, appeary, 0f);
        moveScr.addSpeed = addSpeedOnAppear;

        
        myState = "appear";
    }

    void myStop()
    {


        moveScr.addSpeed = addspeadOnStop;
        
        
        myState = "idle";
    }

    void goAttack()
    {
        tm = 0;
        attPartsTimer = 0;
        myState= "attack";
        
        transform.GetChild(0).gameObject.GetComponent<Animator>().CrossFade("Attack1", 0.1f);


    }
    
    void goDown()
    {
        tm = 0;
        
        myState= "down";
        
        transform.GetChild(0).gameObject.GetComponent<Animator>().CrossFade("Down", 0.1f);

        fireEff.SetActive(false);
        energyeff.SetActive(false);
        attPartsTimer = 0f;
    }
    
    
    void goAttack1()
    {
        tm = 0;
        
        myState= "attack1";

        att2bulletTimer = 0f;
        
        transform.GetChild(0).gameObject.GetComponent<Animator>().CrossFade("Attack2", 0.1f);


    }
    
    void goDown1()
    {
        tm = 0;
        
        myState= "down1";
        
        myBullets.SetActive(false);
        transform.GetChild(0).gameObject.GetComponent<Animator>().CrossFade("Down", 0.1f);


    }

    bool CheckBarrels()
    {
        if ((barrels[0].GetComponent<OneHealth>().curHealth <= 0) &&
            (barrels[1].GetComponent<OneHealth>().curHealth <= 0) &&
            (barrels[2].GetComponent<OneHealth>().curHealth <= 0)) return true;
       


        return false;
    }


    bool CheckHead()
    {
        if (headGoal.transform.parent.gameObject.GetComponent<OneHealth>().curHealth <= 0) return true;

        return false;
    }

    public SkinnedMeshRenderer neck;
    public GameObject tmpRo;
    IEnumerator myDeath()
    {

        myBullets.SetActive(false);
        
        transform.GetChild(3).gameObject.SetActive(true);
        
        for (int i = 0; i < barrels.Length; i++)
        {
            Instantiate(explpref, barrels[i].transform.position, Quaternion.identity, barrels[i].transform.parent);
            
            barrels[i].SetActive(false);

            yield return new WaitForSeconds(1f);

        }

        var go = Instantiate(explpref, myHeadBone.transform.position, Quaternion.identity, myHeadBone.transform);
        go.transform.localPosition = Vector3.zero;
        
        if(HeadToDis != null ) HeadToDis.SetActive(false);
        
        yield return new WaitForSeconds(1f);


        Mesh m = new Mesh();
        neck.BakeMesh(m);
        tmpRo.GetComponent<MeshFilter>().mesh = m;
        
        neck.gameObject.SetActive(false);
        tmpRo.tag = "Exploder";
        ExplControl.instance.ExplodeObject(tmpRo);


        
        moveScr.addSpeed = addspeadOnDeath;


        yield return null;
    }

}
