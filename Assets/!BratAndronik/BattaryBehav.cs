using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattaryBehav : MonoBehaviour
{
    bool active = true;

    private GameObject myPlayer;
    private OneHealth playerScr;

    public GameObject myLight;
    
    // Start is called before the first frame update
    void Start()
    {
        myPlayer = GameObject.FindGameObjectWithTag("Player");
        playerScr = myPlayer.GetComponent<OneHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!active) return;


        if (myPlayer.transform.position.x >= gameObject.transform.position.x)
        {
            active = false;

            playerScr.curHealth = playerScr.maxHealth;
            
            myLight.SetActive(true);


        }


    }
}
