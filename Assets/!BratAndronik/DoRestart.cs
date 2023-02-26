using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoRestart : MonoBehaviour
{
    public GameObject myLifes;
    private LifePlayerControl lifesScr;

    bool isNext = false;


    public GameObject myPlayer;
    public OneHealth playerScr;
    
    
    public string thisScene = "Level_2_1";


   
        
        public static float curTime = 0f;
        public static float checkpointTime = 0f;
    public static int checkPNum = -1;


    private GameObject SaveContr;
    private BuiSpaumer saveScr;

    

    // Start is called before the first frame update
    public void Awake()
    {
        lifesScr = myLifes.GetComponent<LifePlayerControl>();
        myPlayer = GameObject.FindWithTag("Player");
        playerScr = myPlayer.GetComponent<OneHealth>();
        
        
        SaveContr = GameObject.FindWithTag("SaveController");
        saveScr = SaveContr.GetComponent<BuiSpaumer>();
    }
    

    // Update is called once per frame
    void Update()
    {



        if (playerScr.isDead && !isNext)
        {

            isNext = true;
            lifesScr.goNextTry();
        }
        
    }


    public void GoRestartScene()
    {
        
        
        
        SceneManager.LoadScene(thisScene);
        TimeController.instance.tm = curTime;
        saveScr.checkPointNum = checkPNum;
    }


    public void RewriteProps()
    {

        curTime = TimeController.instance.tm;
        checkPNum = saveScr.checkPointNum;
        // checkpointNum =


    }

    public void ClearProps()
    {

        curTime = 0f;
        checkPNum = -1;

    }




}
