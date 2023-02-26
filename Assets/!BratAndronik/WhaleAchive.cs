using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleAchive : MonoBehaviour
{
    private OneHealth myhp;
    private bool done = false;
    
    // Start is called before the first frame update
    void Start()
    {
        myhp = gameObject.GetComponent<OneHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if(done) return;
        if(myhp.curHealth >0) return;
        
        
        GameObject.FindGameObjectWithTag("SaveController").GetComponent<AchivementController>().KillWhale = 1;
        PlayerPrefs.SetInt("KillWhale", 1);
        
        done = true;
        if (PlayerPrefs.GetString("LastLevel") == "None")
        {
            PlayerPrefs.SetString("LastLevel", "Level_1_1");
            
        }




        if (GameObject.FindGameObjectWithTag("SaveController").GetComponent<AchivementController>().LastLevelCompl == 0)
            GameObject.FindGameObjectWithTag("SaveController").GetComponent<AchivementController>().LastLevelCompl =1;
        
        
        
    }
}
