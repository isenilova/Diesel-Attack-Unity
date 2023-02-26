using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIActTutorial : MonoBehaviour
{
    public GameObject myTutorial;
    
    // Start is called before the first frame update
    void Start()
    {
       if(!PlayerPrefs.HasKey("FirstPlay")) PlayerPrefs.SetInt("FirstPlay", 0); 
        
        if(PlayerPrefs.GetInt("FirstPlay") == 0) myTutorial.SetActive(true);
        
        
        PlayerPrefs.SetInt("FirstPlay", 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
