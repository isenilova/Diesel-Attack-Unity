using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketBehav : MonoBehaviour
{
    private bool act = false;
    private Collider2D myColl;

    public GameObject myShip;
    private OneHealth shLifeScr;


    public GameObject MainShipBucket;
    
    // Start is called before the first frame update
    void Start()
    {
        shLifeScr = myShip.GetComponent<OneHealth>();
        myColl = gameObject.GetComponent<Collider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!act && (shLifeScr.curHealth <= 0))
        {
            act = true;
            myColl.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       if(!act) return;
        
        
        if(other.tag != "Player") return;
        
        MainShipBucket.SetActive(true);
        
        Destroy(gameObject);
        
        
    }
}
