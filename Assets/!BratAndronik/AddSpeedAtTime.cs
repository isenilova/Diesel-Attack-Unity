using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSpeedAtTime : MonoBehaviour
{

    public float deltaTime = 2f;

    public float addSpeed = 0.5f;

    private float tm = 0f;


    public float maxdistFromleft = 15;

    public float mindistFromcleft = 3f;

    public float defSpeed = 5f;

    public float defSpeedformin = 3f;
    private GameObject myPlr;
    
    
    public float deltaTimeToFight = 5f;
    private float fithtTimer;
    private bool inFight = false;
    public GameObject ObjToAttack;

    public float startFire = 1f;
    public float endFire = 3f;
    private bool fireAct = false;

    public GameObject WormStart;

    public GameObject myFire;
    
    // Start is called before the first frame update
    void Start()
    {
        myPlr = GameObject.FindGameObjectWithTag("Player");
        fithtTimer = deltaTimeToFight + 0.1f;


    }

    // Update is called once per frame
    void Update()
    {


        if (inFight)
        {
            fithtTimer += Time.deltaTime;

            
            if((fithtTimer>startFire)&&!myFire.activeSelf) myFire.SetActive(true);
            
            if((fithtTimer> endFire)&&myFire.activeSelf) myFire.SetActive(false);
            
            

            if (fithtTimer >= deltaTimeToFight)
            {
                fithtTimer = 0f;

                inFight = false;

            }


        }

        if ((WormStart.transform.position.x - CamBound.instance.lox.transform.position.x) < mindistFromcleft)
        {
            if (gameObject.GetComponent<MoveControl>().addSpeed < defSpeedformin)
                gameObject.GetComponent<MoveControl>().addSpeed = defSpeedformin;
            
            
            
            
            return;


        }

        if ((WormStart.transform.position.x - CamBound.instance.lox.transform.position.x) > maxdistFromleft)
        {
            if (gameObject.GetComponent<MoveControl>().addSpeed > defSpeed)
                gameObject.GetComponent<MoveControl>().addSpeed = defSpeed;



            if (!inFight)
            {
                inFight = true;
                fithtTimer = 0f;
                
                ObjToAttack.GetComponent<Animator>().CrossFadeInFixedTime("attack", 0.1f);



            }



            return;


        }

        tm += Time.deltaTime;


        if (tm > deltaTime)
        {
            tm = 0f;

            gameObject.GetComponent<MoveControl>().addSpeed += addSpeed;



        }
        
        
        
        
        
    }
}
