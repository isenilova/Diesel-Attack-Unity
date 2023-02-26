using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehav_Whale : MonoBehaviour
{

    public GameObject dusts;
    
    public bool cheat = false;

    public GameObject who;
    public Transform loPoint;

    public float tm = 20.0f;


    public float tmCome = 10;
    public float tmPlatform = 8;
    public float tmPerc = 0.6f;
    public float tmComeSpeed = -5;


    public float bossActivate = 2.0f;

    bool isSpawned = false;
    public bool isComing = false;
    bool isExploded = false;
    bool isBossActivated = false;
    private bool bossStart = false;

    public float t = 0;


    public GameObject[] fireStarters;

    public GameObject[] lives;

    public GameObject[] toExplode;
    public GameObject[] toHide;


    public BossBehavC2 boss;
    public GameObject coldr;

    public Transform sparksT;


    public Transform hib;
    public Transform lob;

    public GameObject barrel;
    public GameObject[] platform;

    public float barTlo = 0;
    public float barTHi = 5;
    public float platformTlo = 0;
    public float platformTHi = 5;
    public float barSpdlo = 0;
    public float barSpdHi = 10;


    public Animator botOpen;
    
    // Use this for initialization
	void Start ()
    {
	
        if (cheat)
        {
            tm = 10;
            for (int i = 0; i < lives.Length; i++)
            {
                lives[i].GetComponent<OneHealth>().curHealth = 1;
            }
        }
        
	}

    public IEnumerator SpawnBarrels()
    {
        float t = 0;
        t = Random.Range(barTlo, barTHi);

        while (t > 0)
        {
            t -= Time.deltaTime;
            yield return null;
            
            
        }

        float dir = -1;
        /*
        if (Random.Range(0, 0.99f) < 0.5f)
        {
            dir = -1;
        }
        */

        if (dir < 0)
        {
            var go = Instantiate(barrel);
            go.transform.position = new Vector3(CamBound.instance.hix.position.x, Random.Range(lob.transform.position.y, hib.transform.position.y), 0);
            go.GetComponent<MoveControl>().addSpeed = dir * Random.Range(barSpdlo, barSpdHi) + dir * Camera.main.GetComponent<MoveControl>().addSpeed;
        }
        else
        {
            var go = Instantiate(barrel);
            go.transform.position = new Vector3(CamBound.instance.lox.position.x, Random.Range(lob.transform.position.y, hib.transform.position.y), 0);
            go.GetComponent<MoveControl>().addSpeed = dir * Random.Range(barSpdlo, barSpdHi) + dir * Camera.main.GetComponent<MoveControl>().addSpeed;
        }
        
        yield return null;

        StartCoroutine(SpawnBarrels());
    }

    public IEnumerator SpawnPlat()
    {
        yield return new WaitForSeconds(tmPlatform);
        
        StartCoroutine(SpawnBarrels());
        //StartCoroutine(SpawnPlatforms());
    }
    
    public IEnumerator SpawnPlatforms()
    {   
        float t = 0;
        t = Random.Range(platformTlo, platformTHi);

        while (t > 0)
        {
            t -= Time.deltaTime;
            yield return null;
            
            
        }

        float dir = 1;
        if (Random.Range(0, 0.99f) < 0.5f)
        {
            dir = -1;
        }

        if (dir < 0)
        {
            var go = Instantiate(platform[0]);
            go.transform.position = new Vector3(CamBound.instance.hix.position.x, Random.Range(lob.transform.position.y, hib.transform.position.y), 0);
            go.GetComponent<MoveControl>().addSpeed = dir * Random.Range(barSpdlo, barSpdHi) + dir * Camera.main.GetComponent<MoveControl>().addSpeed;
        }
        else
        {
            var go = Instantiate(platform[1]);
            go.transform.position = new Vector3(CamBound.instance.lox.position.x, Random.Range(lob.transform.position.y, hib.transform.position.y), 0);
            go.GetComponent<MoveControl>().addSpeed = dir * Random.Range(barSpdlo, barSpdHi) + dir * Camera.main.GetComponent<MoveControl>().addSpeed;
        }
        
        yield return null;

        StartCoroutine(SpawnPlatforms());
    }

	
	// Update is called once per frame
	void Update ()
    {
		if (TimeController.instance.tm > tm && !isSpawned)
        {            
            FindObjectOfType<CameraShake>().ShakeCamera();
            isSpawned = true;
            who.SetActive(true);

            who.transform.position = new Vector3(CamBound.instance.lox.position.x, who.transform.position.y, who.transform.position.z) - new Vector3(who.transform.position.x - loPoint.position.x, 0, 0);

            isComing = true;
            t = tmCome;
            who.GetComponent<MoveControl>().addSpeed = tmComeSpeed;

            StartCoroutine(SpawnPlat());
        }

        if (!isSpawned) return;

        if (isComing)
        {
            float prc = (who.transform.position.x - CamBound.instance.lox.position.x)/(CamBound.instance.hix.position.x - CamBound.instance.lox.position.x);
            //Debug.Log(prc);
            if (prc > tmPerc) t = 0;
        }

        t -= Time.deltaTime;
        if (t < 0 && isComing)
        {
            //we remove sparks
            for (int j = 0; j < sparksT.childCount; j++)
            {
                sparksT.GetChild(j).gameObject.SetActive(false);
            }
            
            isComing = false;
            dusts.SetActive(true);
            who.GetComponent<MoveControl>().addSpeed = Camera.main.GetComponent<MoveControl>().addSpeed;

            bossStart = true;
            botOpen.SetBool("opened", true);
            //StartCoroutine(SpawnBarrels());
            //StartCoroutine(SpawnPlatforms());
            
            boss.ActivateShooters();
            

        }


        bool q = false;
        for (int i = 0; i < lives.Length; i++)
        {
            if (lives[i] == null) continue;

            if (lives[i].GetComponent<OneHealth>().curHealth > 0)
            {
                q = true;
            }
        }

        if (!bossStart) return;
        
        //spawn cosmic musor
        //spawn barrels

    }
}
