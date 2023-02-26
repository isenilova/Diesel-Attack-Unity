using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixColladers : MonoBehaviour
{
    public GameObject[] offColls;

    private OneHealth lifeScr;

    private Collider2D myColl;

    private bool isDead = false;
    
    // Start is called before the first frame update
    void Start()
    {
        lifeScr = gameObject.GetComponent<OneHealth>();


    }

    // Update is called once per frame
    void Update()
    {

        if (!isDead && (lifeScr.curHealth <= 0f))
        {
            isDead = true;
            offAllColls();


        }

    }


    void offAllColls()
    {
        for (int i = 0; i < offColls.Length; i++)
        {
           // myColl = offColls[i].GetComponent<Collider2D>();

            offColls[i].SetActive(false);




        }





    }
}
