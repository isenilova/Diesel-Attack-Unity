using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{

    public string plNum = "0";
    public float amnt = 100;
    public string dmgTag = "Enemy";

    public bool destOnDmg = true;

    public bool useStay = false;
    public float dTime = 0.1f;
    float lastTime = 0;

  

    
    
    public bool useDebug = false;


    public bool destroyOnTime = false;

    public float LifeTimer = 10f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	    if (destroyOnTime)
	    {
	        LifeTimer -= Time.deltaTime;
	        
	        if(LifeTimer <= 0f) Destroy(gameObject);



	    }


	}

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (useDebug)
        {
            Debug.Log("<color=red>" + collision.tag + "</color>");
        }
        
        if (collision.tag == dmgTag)
        {
            lastTime = TimeController.instance.tm;

            Debug.Log(collision.gameObject);

            if (collision.GetComponentInParent<OneHealth>() != null && collision.GetComponentInParent<OneHealth>().isDebug)
            {

                int s = 47;
            }

            if(collision.GetComponentInParent<OneHealth>()!= null) collision.GetComponentInParent<OneHealth>().DoDamage(amnt);

            if (destOnDmg)
            {
                GetComponentInParent<OneHealth>().curHealth = 0;
            }

        }
        
        if (collision.tag == "BulletTarget")
        {
            lastTime = TimeController.instance.tm;

            collision.GetComponentInParent<OneHealth>().DoDamage(amnt);

            if (destOnDmg)
            {
                GetComponentInParent<OneHealth>().curHealth = 0;
            }

        }
        
        

        if (collision.tag == "Player" && dmgTag == "Enemy")
        {
            //we are blinking
            var rt = collision.GetComponentInParent<OneShip>().id;

            if (rt != plNum)
            {
                //ship is freezed
                //collision.GetComponentInParent<Freezer>().Freeze();
                GetComponent<OneHealth>().curHealth = 0;
            }
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (!useStay) return;

        if ((collision.tag == dmgTag)||(collision.tag == "BulletTarget"))
        {
            if (TimeController.instance.tm - lastTime < dTime)
            {
                return;
            }

            lastTime = TimeController.instance.tm;

            collision.GetComponentInParent<OneHealth>().DoDamage(amnt);

            if (destOnDmg)
            {
                GetComponentInParent<OneHealth>().curHealth = 0;
            }

        }
    }
}
