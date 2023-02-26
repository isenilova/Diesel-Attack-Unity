using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class ExplodOnDeath : MonoBehaviour
{
    public GameObject myDeathEffect;


    public GameObject[] disOnDeath;

    private OneHealth lifeScr;
    private bool isDeath = false;

    public GameObject toDestr = null;

    // Start is called before the first frame update
    void Start()
    {
        lifeScr = gameObject.GetComponent<OneHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((!isDeath) && (lifeScr.curHealth <= 0f))
        {
            isDeath = true;


            for (int i = 0; i < disOnDeath.Length; i++)
            {
                
                disOnDeath[i].SetActive(false);
                
            }


            Instantiate(myDeathEffect, transform.position, Camera.main.transform.rotation);
            
            
            if(toDestr != null) Destroy(toDestr);



        }



    }
}
