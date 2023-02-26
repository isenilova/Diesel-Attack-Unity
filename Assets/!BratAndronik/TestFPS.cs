using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFPS : MonoBehaviour
{
    public bool test = false;

    public int maxFps = 10;


    private void Awake()
    {
        if(!test) return;


        QualitySettings.vSyncCount = 2;
        Application.targetFrameRate = maxFps;

    }

    // Start is called before the first frame update
    void Start()
    {
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
