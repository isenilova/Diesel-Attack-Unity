using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievDeath : MonoBehaviour
{
    private bool done = false;

    private OneHealth myHp;

    private int num;
    
    // Start is called before the first frame update
    void Start()
    {
        myHp = gameObject.GetComponent<OneHealth>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(done) return;
        if(myHp.curHealth >0) return;

        num = GameObject.FindGameObjectWithTag("SaveController").GetComponent<AchivementController>().DeathCount;
        num++;
        PlayerPrefs.SetInt("DeathCount", num);
        
        GameObject.FindGameObjectWithTag("SaveController").GetComponent<AchivementController>().DeathCount = num;

        done = true;


    }
}
