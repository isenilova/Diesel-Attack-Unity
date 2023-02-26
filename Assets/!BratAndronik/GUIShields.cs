using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIShields : MonoBehaviour
{
 

    public int myCurShield;


    public float[] myShieldDurations;

    public float myCurDurat;

    public bool myShieldState = false;
    public bool isShield = false;


    public GameObject myPlayer;
    private OneHealth hpScr;

    public string myButton = "e";
    string myjoystik = "joystick button 0";
    
    
    
    // Start is called before the first frame update
    public void Start()
    {
        myCurShield = PlayerPrefs.GetInt("S");
        
        if((myCurShield<=0)||(myCurShield >3)) return;

        isShield = true;
        myShieldState = true;
        
        myCurDurat = myShieldDurations[myCurShield - 1];


        hpScr = myPlayer.GetComponent<OneHealth>();
        hpScr.AddShield(1000, myCurDurat, myCurShield-1);
        
        
        SwitchShield();

    }

    // Update is called once per frame
    void Update()
    {
        if(!isShield) return;
        
        if(Input.GetKeyDown(myButton)|| Input.GetKeyDown(myjoystik)) SwitchShield();
    }

    void SwitchShield()
    {
        if (myShieldState)
        {

            myCurDurat = hpScr.StopShield();
            myShieldState = false;
            return;
        }

        myShieldState = true;
        hpScr.AddShield(1000, myCurDurat, myCurShield-1);

    }


}
