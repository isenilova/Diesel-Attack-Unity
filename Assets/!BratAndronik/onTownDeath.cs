using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onTownDeath : MonoBehaviour
{
    private OneHealth lifeScr;
    public bool isDone = false;

    public GameObject myExplosions;

    public GameObject mainBoss;

    private MoveControl moveScr;

    public float myAddSpeed = 5f;

    public GameObject[] disObects;
    
    // Start is called before the first frame update
    void Start()
    {
        lifeScr = gameObject.GetComponent<OneHealth>();

        moveScr = mainBoss.GetComponent<MoveControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((lifeScr.curHealth <= 0f)&&(!isDone))
        {
            isDone = true;

            myExplosions.SetActive(true);


            moveScr.addSpeed = myAddSpeed;

           // for (int i = 0; i <= disObects.Length; i++)
            //{
              //  disObects[i].SetActive(false);
                
            //}

        }


    }
}
