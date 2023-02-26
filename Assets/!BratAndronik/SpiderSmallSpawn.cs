using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderSmallSpawn : MonoBehaviour
{

    public GameObject smallSpider;

    private GameObject curSp;

    private bool spawn = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        
        if(gameObject.GetComponent<OneHealth>().curHealth >0f) return;
        if(spawn) return;


       curSp = Instantiate(smallSpider, transform.position, transform.rotation);
       curSp = Instantiate(smallSpider, transform.position, transform.rotation);
        curSp.transform.Rotate(90, 0, 0);
        
        curSp = Instantiate(smallSpider, transform.position, transform.rotation);
        curSp.transform.Rotate(-90, 0, 0);
        
        curSp = Instantiate(smallSpider, transform.position, transform.rotation);
        curSp.transform.Rotate(180, 0, 0);

        spawn = true;

       // Debug.Break();
    }
}
