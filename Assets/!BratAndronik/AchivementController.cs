using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchivementController : MonoBehaviour
{
    public int LastLevelCompl = 0;
    public int KillWhale = 0;
    public int KillWorm = 0;
    public int DeathCount = 0;
    
    // Start is called before the first frame update
    void Start()
    {
       if(!PlayerPrefs.HasKey("KillWhale")) PlayerPrefs.SetInt("KillWhale", KillWhale);
        KillWhale = PlayerPrefs.GetInt("KillWhale");
        
        if(!PlayerPrefs.HasKey("LastLevel")) PlayerPrefs.SetString("LastLevel", "None");
        
        if(!PlayerPrefs.HasKey("DeathCount")) PlayerPrefs.SetInt("DeathCount", DeathCount);
        DeathCount = PlayerPrefs.GetInt("DeathCount");

        
        if(!PlayerPrefs.HasKey("KillWorm")) PlayerPrefs.SetInt("KillWorm", KillWorm);
        KillWorm = PlayerPrefs.GetInt("KillWorm");
        
        
        switch (PlayerPrefs.GetString("LastLevel"))
        {
            case "None":
            {
                LastLevelCompl = 0;
                break;
            }
            
            case "Level_1_1":
            {
                LastLevelCompl = 1;
                break;
            }
            
            case "Level_2_1":
            {
                LastLevelCompl = 2;
                break;
            }
            
            case "Level_3_1":
            {
                LastLevelCompl = 3;
                break;
            }
            

        }
        
        
        
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        Start();
    }
}
