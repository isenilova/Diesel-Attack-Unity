using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockLevel : MonoBehaviour
{
    public GameObject[] lvlbuts;

    public int lastlvl = 0;
    public Color grey;

    public bool TESTMODE = false;
    
    // Start is called before the first frame update
    void Start()
    {
        if(TESTMODE) return;
        
        if(!PlayerPrefs.HasKey("LastLevel")) PlayerPrefs.SetString("LastLevel", "None");
        switch (PlayerPrefs.GetString("LastLevel"))
        {
            case "None":
            {
                lastlvl = 0;
                break;
            }
            case "Level_1_1":
            {
                lastlvl = 1;
                break;
            }
            case "Level_2_1":
            {
                lastlvl = 2;
                break;
            }
            case "Level_3_1":
            {
                lastlvl = 3;
                break;
            }
            case "Level_4_1":
            {
                lastlvl = 4;
                break;
            }
            case "Level_5_1":
            {
                lastlvl = 5;
                break;
            }
            case "Level_6_1":
            {
                lastlvl = 6;
                break;
            }


        }


        for (int i = 0; i < lvlbuts.Length; i++)
        {
            if (i <= lastlvl)
            {
                lvlbuts[i].GetComponent<Image>().color = Color.white;
                lvlbuts[i].GetComponent<Button>().enabled = true;
            }
            else
            {
                lvlbuts[i].GetComponent<Image>().color = grey;
                lvlbuts[i].GetComponent<Button>().enabled = false;
                
            }



        }






    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
