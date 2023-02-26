using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LifePlayerControl : MonoBehaviour
{
    private int maxLifes = 3;
    public static int curLifeNum = 3;
    private Transform[] myLifeObj;

    public float delayToLoad = 1f;
    private bool gonext = false;


    private GameObject saveContr;
    private DoRestart saveScr;
    private BuiSpaumer saveScr2;
    
    // Start is called before the first frame update
    void Start()
    {
       myLifeObj = new Transform[transform.childCount];
        
        

       for(int i =0; i< transform.childCount; i++)
       {
           myLifeObj[i] = transform.GetChild(i);

           if(i >= curLifeNum) myLifeObj[i].gameObject.SetActive(false);
           
             else myLifeObj[i].gameObject.SetActive(true);
       }

        saveContr = GameObject.FindWithTag("SaveController");
        saveScr = saveContr.GetComponent<DoRestart>();
        saveScr2 = saveContr.GetComponent<BuiSpaumer>();
    }

    // Update is called once per frame
    void Update()
    {

        if (gonext)
        {
            delayToLoad -= Time.deltaTime;
            
            if(delayToLoad < 0)  saveScr.GoRestartScene();

        }

        //if(Input.GetKeyDown("j")) goNextTry(); 
    }


    public void goNextTry()
    {

        if (curLifeNum == 1)
        {
            myLifeObj[0].gameObject.SetActive(false);
            
            return;
        }
        curLifeNum--;
        myLifeObj[curLifeNum].gameObject.SetActive(false);

        gonext = true;
        
       

    }


    public void MaxCurLife()
    {

        curLifeNum = maxLifes;

        saveScr2.checkPointNum = -1;
        TimeController.instance.tm = 0f;

        saveScr.ClearProps();

    }
}
