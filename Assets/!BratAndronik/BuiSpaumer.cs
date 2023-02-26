using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuiSpaumer : MonoBehaviour
{
    public float[] timeBuiSpaum;

    public GameObject BuiObj;

    private GameObject[] myBuis;

    private int nextSpaum;

    public int checkPointNum;
    public float myTimeStart = 0f;
    private SaveBuiLight buiScr;

    public float deltaYSpaum = 0f;
    
    

    private CamBound camScr;

    private GameObject saveObj;
    private DoRestart saveScr;


    
    
    // Start is called before the first frame update
    void Start()
    {
        checkPointNum = DoRestart.checkPNum;
        
        saveObj = GameObject.FindGameObjectWithTag("SaveController");
        saveScr = saveObj.GetComponent<DoRestart>();
        
        
        myBuis = new GameObject[timeBuiSpaum.Length];
        nextSpaum = 0;
        checkPointNum = -1;


        camScr = Camera.main.GetComponent<CamBound>();


        nextSpaum = instNextSpaum();

    }

    // Update is called once per frame
    void Update()
    {
        if(nextSpaum >= timeBuiSpaum.Length) return;
        
        
        if (TimeController.instance.tm > timeBuiSpaum[nextSpaum])
        {
           // Debug.Log(nextSpaum);
            myBuis[nextSpaum] = Instantiate(BuiObj);
             myBuis[nextSpaum].transform.position = new Vector3(camScr.hix.position.x,deltaYSpaum,0f);
            buiScr = myBuis[nextSpaum].GetComponent<SaveBuiLight>();
            buiScr.myOrderNum = nextSpaum;
            if (checkPointNum >= nextSpaum) buiScr.mySave = true;
            else buiScr.mySave = false;
            nextSpaum++;
            //Debug.Break();
        }

    }


    public void RewriteCheckPoint(int i, float x)
    {
        
       // Debug.Log("Bui_"+i.ToString());
        if (i > checkPointNum)
        {
            checkPointNum = i;

            //myTimeStart = TimeController.instance.tm;
            
            saveScr.RewriteProps();
        }


    }

    int instNextSpaum()
    {
        int next = 0;


        for (int i = 0; i < timeBuiSpaum.Length; i++)
        {
            if (TimeController.instance.tm > timeBuiSpaum[i]) next = i + 1;



        }



        return next;
    }

}
