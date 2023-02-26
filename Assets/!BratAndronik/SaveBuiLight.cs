using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveBuiLight : MonoBehaviour
{
    public bool mySave = false;
    public GameObject[] myLights;
    public Color colorRed;
    public Color colorGreen;

    public float deltaEnter = 0f;
    public float deltaExit = 1f;
    
    

    private int lightNum;
    private Light[] lightComps;

    private GameObject mainChip;
    private float epsilon = 0.5f;

    private bool inSaveArea = false;


   public int myOrderNum;


    private GameObject SaveContr;
    private BuiSpaumer saveScr;
    
    // Start is called before the first frame update
    void Start()
    {
        SaveContr = GameObject.FindWithTag("SaveController");
        saveScr = SaveContr.GetComponent<BuiSpaumer>();
        
        lightNum = myLights.Length;
        lightComps = new Light[lightNum];

        saveScr.checkPointNum = DoRestart.checkPNum;

        for (int i = 0; i < lightNum; i++)
        {
            lightComps[i] = myLights[i].GetComponent<Light>();
            
            if(mySave) lightComps[i].color = colorGreen;
            else lightComps[i].color = colorRed;

        }


        mainChip = GameObject.FindGameObjectWithTag("Player");

        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(inSaveArea) return;
        if(mainChip == null) return;

        if (!mySave && (Mathf.Abs(transform.position.x - mainChip.transform.position.x) < deltaEnter + epsilon))
        {
            mySave = true;

            LightningOn();
            
             saveScr.RewriteCheckPoint(myOrderNum, TimeController.instance.tm);
            return;
        }

     /*   if (mySave && (Mathf.Abs(transform.position.x - mainChip.transform.position.x) > epsilon + deltaExit))
        {

            inSaveArea = true;
            LightningOff();

        }
*/

    }

    public void LightningOn()
    {
        
      for(int i = 0; i < lightNum; i++)  lightComps[i].color = colorGreen; 
        
    }


    public void LightningOff()
    {
        
        
        for(int i = 0; i < lightNum; i++)  lightComps[i].color = colorRed;
        
    }


    public void RewriteOrder(int i)
    {


        myOrderNum = i;
    }


}
