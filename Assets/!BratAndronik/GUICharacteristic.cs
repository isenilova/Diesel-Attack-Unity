using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUICharacteristic : MonoBehaviour
{
    public GameObject[] myGUIObj;
    public GameObject[] myUpgrObj;

    public string myName;

    public int myDefValue = 0;

    private int objNum;

    public bool test = true;
    
    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey(myName)) PlayerPrefs.SetInt(myName, 1);
        if(test) PlayerPrefs.SetInt(myName, myDefValue);
        

        objNum = PlayerPrefs.GetInt(myName);

        for (int i = 0; i < myGUIObj.Length; i++)
        {
            if (i < objNum)
            {
                myGUIObj[i].SetActive(true);
                
            }
            else
            {
                myGUIObj[i].SetActive(false);
                
                
            }


        }


        if (myUpgrObj != null)
        {

            for (int i = 0; i < myUpgrObj.Length; i++)
            {
                
                if(i == objNum) myUpgrObj[i].SetActive(true);
                
                else myUpgrObj[i].SetActive(false);
                
                
                
            }



        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
