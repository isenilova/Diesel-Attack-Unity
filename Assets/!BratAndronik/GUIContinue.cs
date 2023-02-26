using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GUIContinue : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void onMyClick()
    {
        string a;
        if(!PlayerPrefs.HasKey("LastLevel")) PlayerPrefs.SetString("LastLevel", "Level_1_1");

        a = PlayerPrefs.GetString("LastLevel");

        SceneManager.LoadScene(a);

    }
}
