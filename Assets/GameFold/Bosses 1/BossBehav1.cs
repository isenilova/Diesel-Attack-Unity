using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehav1 : MonoBehaviour {

    public bool cheat = false;

    public GameObject who;
    public Transform loPoint;

    public float tm = 20.0f;


    public float tmCome = 10;
    public float tmPerc = 0.6f;
    public float tmComeSpeed = -5;


    public float bossActivate = 2.0f;

    bool isSpawned = false;
    bool isComing = false;
    bool isExploded = false;
    bool isBossActivated = false;

    float t = 0;


    public GameObject[] fireStarters;

    public GameObject[] lives;

    public GameObject[] toExplode;
    public GameObject[] toHide;


    public BossBehavC1 boss;
    public GameObject coldr;

    public Transform sparksT;
    
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
	
	// Update is called once per frame
	void Update ()
    {
		if (TimeController.instance.tm > tm && !isSpawned)
        {            
            FindObjectOfType<CameraShake>().ShakeCamera();
            isSpawned = true;
            who.SetActive(true);

            who.transform.position = new Vector3(CamBound.instance.hix.position.x, who.transform.position.y, who.transform.position.z) + new Vector3(who.transform.position.x - loPoint.position.x, 0, 0);

            isComing = true;
            t = tmCome;
            who.GetComponent<MoveControl>().addSpeed = tmComeSpeed;
        }

        if (!isSpawned) return;

        if (isComing)
        {
            float prc = (who.transform.position.x - CamBound.instance.lox.position.x)/(CamBound.instance.hix.position.x - CamBound.instance.lox.position.x);
            if (prc < tmPerc) t = 0;
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
            who.GetComponent<MoveControl>().addSpeed = Camera.main.GetComponent<MoveControl>().addSpeed;

            for (int i = 0; i < fireStarters.Length; i++)
            {
                if (fireStarters[i].GetComponent<AllShoot>() != null)
                {
                    fireStarters[i].GetComponent<AllShoot>().enabled = true;
                }
            }

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

        if (!q && !isExploded)
        {
            isExploded = true;
            for (int i = 0; i < toExplode.Length; i++)
            {
                toExplode[i].tag = "Exploder";
                ExplControl.instance.ExplodeObject(toExplode[i]);
            }

            for (int i = 0; i < toHide.Length; i++)
            {
                if (toHide[i] != null)
                toHide[i].SetActive(false);
            }

            t = bossActivate;
        }


        if (t < 0 && isExploded && !isBossActivated)
        {
            isBossActivated = true;
            boss.enabled = true;
            coldr.SetActive(true);
            

        }

	}
}
