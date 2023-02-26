using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIOptions : MonoBehaviour
{
    public static GUIOptions instance;
    public Text myLastLvl;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        string a;
        if(!PlayerPrefs.HasKey("LastLevel")) PlayerPrefs.SetString("LastLevel", "None");

        a = PlayerPrefs.GetString("LastLevel");

        myLastLvl.text = a;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
