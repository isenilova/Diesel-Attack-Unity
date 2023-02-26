using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchieveLvl : MonoBehaviour
{
    public string myLvl;
    
    
    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("LastLevel")) PlayerPrefs.SetString("LastLevel", "None");

        if ((PlayerPrefs.GetString("LastLevel") == "Level_1_1") && (myLvl == "Level_2_1"))
        {

            GameObject.FindGameObjectWithTag("SaveController").GetComponent<AchivementController>().LastLevelCompl = 2;
            PlayerPrefs.SetString("LastLevel", "Level_2_1");
            return;
        }
        
        if ((PlayerPrefs.GetString("LastLevel") == "Level_2_1") && (myLvl == "Level_3_1"))
        {

            GameObject.FindGameObjectWithTag("SaveController").GetComponent<AchivementController>().LastLevelCompl = 3;
            PlayerPrefs.SetString("LastLevel", "Level_3_1");
           
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
