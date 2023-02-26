using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownGun : MonoBehaviour
{
    public GameObject myArrow;

    private GameObject curArrow;
    private MoveParabolic moveScr;
    
    public GameObject spawmObj;
    public float startDelay = 2f;

    public float bulletDelay = 1f;
    private float mydel = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        mydel = 5f;
    }

    // Update is called once per frame
    void Update()
    {

        startDelay -= Time.deltaTime;
        
        
        if(startDelay > 0f) return;
        
        mydel += Time.deltaTime;

        if (mydel >= bulletDelay)
        {
           curArrow = Instantiate(myArrow, spawmObj.transform.position, spawmObj.transform.rotation);

            moveScr = curArrow.GetComponent<MoveParabolic>();

            moveScr.myVell = spawmObj.transform.forward;
            mydel = 0f;
        }

    }
}
