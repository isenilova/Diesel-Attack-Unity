using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartController : MonoBehaviour
{
    public string myState = "empty";
    public float appearTime = 5f;
    
    public float stopTime = 7f;

    public float appearx = 10f;
    public float appeary = 10f;
    public float addSpeedOnAppear = -7f;
    public float addspeadOnStop = 1f;
    
    public float shakeTime = 2f;
    
    public GameObject myBoss;
    private MoveControl moveScr;

    public GameObject mycoldr;
    private OneHealth myhpScr;

    public float changeAnimatHp;
    private bool changeAn = false;
    public GameObject[] objToChangeAnimat;

    public GameObject EffectsFolder;
    private bool effon = false;
    public float dmgToEffect = 1000f;

    public Material damMat;
    public float dmgToMat = 800f;
    public GameObject[] objToChMat;
    private bool changeMat = false;

    public GameObject BigGearToExplode;
    public GameObject EffectOnDeath;
    public float addSpeedOnDeath;
    
    // Start is called before the first frame update
    void Start()
    {
        moveScr = gameObject.GetComponentInParent<MoveControl>();
        myhpScr = myBoss.GetComponent<OneHealth>();
    }

    // Update is called once per frame
    void Update()
    {

        if ((TimeController.instance.tm > appearTime) && (myState == "empty"))
        {
            myAppear();
            return;
        }

        if ((TimeController.instance.tm > stopTime) && (myState == "appear"))
        {
            myStop();
            return;
        }

        if (myState == "stop")
        {
            if (!changeAn && (myhpScr.curHealth <= changeAnimatHp))
            {
                ChangeAnimations();
                changeAn = true;
            }

            if (!effon && (myhpScr.curHealth <= dmgToEffect))
            {
                EffectsFolder.SetActive(true);
                effon = true;
            }


            if (!changeMat && (myhpScr.curHealth <= dmgToMat))
            {
                ChangeMaterial();
                changeMat = true;
            }


            if (myhpScr.curHealth <= 0)
            {
                myState = "dead";
                
                mycoldr.SetActive(false);

                Instantiate(EffectOnDeath, mycoldr.transform.position, Camera.main.transform.rotation);
                moveScr.addSpeed = addSpeedOnDeath;


                BigGearToExplode.GetComponent<Collider>().enabled = true;
                StartCoroutine(DelayedExpl(0.1f, BigGearToExplode));

            }


        }


    }
    
    void myAppear()
    {
        
        FindObjectOfType<CameraShake>().ShakeCamera2(shakeTime);
        
        myBoss.SetActive(true);
        gameObject.transform.position = new Vector3(CamBound.instance.hix.transform.position.x+ appearx, appeary, 0f);
        moveScr.addSpeed = addSpeedOnAppear;

        
        myState = "appear";
    }

    void myStop()
    {


        moveScr.addSpeed = addspeadOnStop;
        
        mycoldr.SetActive(true);
        myState = "stop";
    }


    void ChangeAnimations()
    {

        for (int i = 0; i < objToChangeAnimat.Length; i++)
        {
            objToChangeAnimat[i].GetComponent<Animator>().CrossFadeInFixedTime("Damage", 0.1f);
            
            
        }



    }


    void ChangeMaterial()
    {
        
        for (int i = 0; i < objToChMat.Length; i++)
        {
            objToChMat[i].GetComponent<Renderer>().material = damMat;


        }
        
        
    }
    
    
    
    public IEnumerator DelayedExpl(float t, GameObject obj)
    {
        yield return new WaitForSeconds(t);
        
        obj.tag = "Exploder";                
        Debug.Log("Exploded ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
        Debug.Log(obj);
        ExplControl.instance.ExplodeObject(obj);        
        
    }

}
