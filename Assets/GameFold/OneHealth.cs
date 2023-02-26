using System.Collections;
using System.Collections.Generic;
using FXV;
//using Steamworks;
using UnityEngine;

public class OneHealth : MonoBehaviour {

    public float curHealth = 100;
    public float maxHealth = 100;
    float prevHealth = 0;

    public bool explodeOnDeath = false;
    public bool justDestroy = true;
    public bool customDeathExplode = false;
    public float dltzExpl = 0.0f;
    public bool customLogic = false;

    public GameObject mySimpleExplosion;
    public GameObject ObjRotToExplode;
    public bool ExplodeInParent = false;
    
    
    public Vector3 deltaSimpleExplosion = new Vector3(0,1,0);
    
    public GameObject explodePrefab;
    public float fadeTime = 0.2f;

    public bool isDead = false;
    
    
    float curShield = 0;
    public float maxShield = 100;

    public GameObject shield;

    private float shieldDur = 0;
    private int shieldNum = 0;

    public AudioClip clipDeath;

    public DoDamage[] damagers;

    public AfterDeath[] afterDeaths;

    public Transform[] childDestroy;


    public int moneyForMe = 10;

    private GameObject myScore;
    private GUIScore scoreScr;

    public bool isDebug;
    
    private void Start()
    {
        if (tag != "Player")
        {
            maxHealth = (maxHealth * Cooper.healthMult);
            curHealth = (curHealth * Cooper.healthMult);
        }
        
        clipDeath = Resources.Load<AudioClip>("Audio/explosion");
        prevHealth = curHealth;
    }

    public float GetCurShield()
    {
        return curShield;
    }

    public void AddShield(float amount, float duration, int num = 0)
    {
        shieldDur = duration;
        shieldNum = num;
        
        if (shield != null)
        {
            shield.transform.GetChild(num).gameObject.SetActive(true);    
        }

        curShield = maxShield;
        
        /*
        if (curShield + amount > maxShield)
        {
            curShield = maxShield;
        }
        else
        {
            curShield += amount;
        }
        */
    }

    public float StopShield()
    {
        if (shieldDur <= 0) return 0;

        float curdur = shieldDur;

        shieldDur = 0;

        return curdur;


    }


    public void AddHealth(float amount)
    {
        if (curHealth + amount > maxHealth)
        {
            curHealth = maxHealth;
        }
        else
        {
            curHealth += amount;
        }
    }

    public void DoDamage(float amount)
    {

        if (isDebug)
        {
            int u = 56;
        }
        
        if (curShield > 0)
        {
            curShield -= 0;
            if (shield != null)
            {
                shield.transform.GetChild(shieldNum).GetComponent<FXVShield>().OnHit(transform.position, 10);
            }
            
            
            if (curShield <= 0)
            {
                curShield = 0;
                if (shield != null)
                {
                    shield.SetActive(false);    
                }
            }
            
        }
        else
        {
            curHealth -= amount;
        }
    }

    public void Terminate()
    {
        curHealth = 0;
    }
    
    // Update is called once per frame
    void Update ()
    {

        shieldDur -= Time.deltaTime;
        if (shieldDur < 0)
        {
            if (shield != null && shield.transform.GetChild(shieldNum).gameObject.activeSelf) shield.transform.GetChild(shieldNum).gameObject.SetActive(false);

            curShield = 0;

        }

        if (curHealth != prevHealth)
        {
            if (damagers != null && damagers.Length > 0)
            {
                for (int i = 0; i < damagers.Length; i++)
                {
                   if(damagers[i] != null) damagers[i].Do(curHealth);
                }
            }
            else if (GetComponent<DoDamage>() != null)
            {
                var tos = GetComponentsInChildren<DoDamage>();
                Debug.Log(tos.Length);
                for (int i = 0; i < tos.Length; i++)
                {
                    tos[i].GetComponent<DoDamage>().Do(curHealth);
                }
            }

            if (GetComponent<DoDestroy>() != null)
            {
                GetComponent<DoDestroy>().Do(curHealth/ maxHealth);
            }

            prevHealth = curHealth;
        }
		
        if (curHealth <= 0 && !isDead)
        {
            
            
            ///////////////////////////score
            ///
            ///
            ///
          
                myScore = GameObject.FindGameObjectWithTag("Score");
            scoreScr = myScore.GetComponent<GUIScore>();
            scoreScr.addScore(moneyForMe);
            

            if (mySimpleExplosion != null)
            {

                if (ObjRotToExplode == null)
                {
                    if (!ExplodeInParent)
                    {
                        Instantiate(mySimpleExplosion, transform.position + deltaSimpleExplosion, transform.rotation);
                    }
                    else
                    {

                        Instantiate(mySimpleExplosion, transform.position + deltaSimpleExplosion, transform.rotation,
                            gameObject.transform);

                    }
                }
                else
                {
                    
                    if (!ExplodeInParent)
                    {
                        Instantiate(mySimpleExplosion, transform.position + deltaSimpleExplosion, ObjRotToExplode.transform.rotation);
                    }
                    else
                    {

                        Instantiate(mySimpleExplosion, transform.position + deltaSimpleExplosion, ObjRotToExplode.transform.rotation,
                            gameObject.transform);

                    }
                    
                    
                  
                }
            }
            
            isDead = true;
            if (clipDeath != null)
            {
                AudioSource.PlayClipAtPoint(clipDeath, Camera.main.transform.position);
                
                
                
            }

            if (GetComponent<OneScore>() != null)
            {
                ParamsEvt ev = new ParamsEvt();
                ev.score = GetComponent<OneScore>().score;
                EventManager.TriggerEvent(EvtConsts.PLAYER_GET_SCORE, ev);
            }

            if (afterDeaths != null)
            {
                for (int i = 0; i < afterDeaths.Length; i++)
                {
                    afterDeaths[i].Do();
                }
            }

            if (GetComponent<DoAction>() != null)
            {
                var tos = GetComponents<DoAction>();
                for (int i = 0; i < tos.Length; i++)
                {
                    tos[i].GetComponent<DoAction>().Do();
                }
            }

            if (explodeOnDeath)
            {

                //we disable objects which are not for destr
                
                
                if (childDestroy != null)
                {
                    for (int i = 0; i < childDestroy.Length; i++)
                    {
                        for (int j = 0; j < childDestroy[i].childCount; j++)
                        {
                            Destroy(childDestroy[i].GetChild(j).gameObject);
                        }
                    }
                }

                StartCoroutine(DelayedExpl(0.01f));

            }
            else if (customDeathExplode)
            {
                var go = (GameObject) Instantiate(explodePrefab);
                go.transform.position = transform.position + new Vector3(0,0,dltzExpl);
                Destroy(go, fadeTime);
                Destroy(gameObject);
            }
            else if (justDestroy)
            {
                Destroy(gameObject);
            }
            else if (customLogic)
            {
                GetComponentInParent<MidPlatform>().CustomDeath();
            }
        }

	}

    public IEnumerator DelayedExpl(float t)
    {
        yield return new WaitForSeconds(t);
        
                tag = "Exploder";                
                Debug.Log("Exploded ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                Debug.Log(gameObject);
                ExplControl.instance.ExplodeObject(gameObject);        
        
    }
}
