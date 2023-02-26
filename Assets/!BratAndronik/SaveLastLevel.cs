using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLastLevel : MonoBehaviour
{
    public string myLevel;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("LastLevel") || (PlayerPrefs.HasKey("LastLevel") && PlayerPrefs.GetString("LastLevel") == "None" ) || (PlayerPrefs.HasKey("LastLevel") && string.Compare(myLevel, PlayerPrefs.GetString("LastLevel")) < 0 )) 
       PlayerPrefs.SetString("LastLevel", myLevel); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
