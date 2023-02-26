using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIWeapons : MonoBehaviour
{
    public int myCurW;

    
    //slots: med, up, down. Defolt = med= 1.

    public int myWeapons = 231;
    private int weapDops;
    
    public bool test = true;

    public GameObject[] mySlots;

    public int[] mySlotVals = new int[3];

    public GameObject defWeap;


    public GameObject[] myUpgrWeap;
    
    
    public Vector3 weaponRotation = Vector3.zero;

    public bool second = false;
    
    // Start is called before the first frame update
    public void Start()
    {
        PlayerData.Load();

            myCurW = PlayerPrefs.GetInt("W");


        if (test) PlayerPrefs.SetInt("WeaponList", myWeapons);
        if (!PlayerPrefs.HasKey("WeaponList"))
        {
            PlayerPrefs.SetInt("WeaponList", 111);
        }

        myWeapons = PlayerPrefs.GetInt("WeaponList");
        weapDops = myWeapons;
        
        ParseSlots();
        disSlots();
        
        collectSlots();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void ParseSlots()
    {
        /*
        mySlotVals[2] = weapDops % 10 - 2;

        weapDops = weapDops / 10;

        mySlotVals[1] = weapDops % 10 - 2;

        weapDops = weapDops / 10;

        mySlotVals[0] = weapDops - 2;
        */

        Debug.Log(PlayerData.player.slot2);
        Debug.Log(PlayerData.player.slot1);
        Debug.Log(PlayerData.player.slot0);
        
        mySlotVals[2] = PlayerData.player.slot2 - 1;
        mySlotVals[1] = PlayerData.player.slot1 - 1;
        mySlotVals[0] = PlayerData.player.slot0 - 1;


    }

    void disSlots()
    {
        if((myCurW <=0) || (myCurW > 3)) return;

        mySlots[0].SetActive(true);
        
        if(mySlotVals[0] != -1) defWeap.SetActive(false);
        
        if(myCurW == 1) return;
        
        mySlots[1].SetActive(true);

        if (myCurW == 2) return;
        
        mySlots[2].SetActive(true);


    }

    void collectSlots()
    {

        for (int i = 0; i < 3; i++)
        {

            if ((mySlotVals[i] >= 0) && (mySlotVals[i] < myUpgrWeap.Length))
            {
                if (mySlots[i].transform.childCount > 0)
                {
                    Destroy(mySlots[i].transform.GetChild(0).gameObject);
                }
                    
                var go = (GameObject)Instantiate(myUpgrWeap[mySlotVals[i]], mySlots[i].transform);

                if (!second)
                {
                    go.transform.localPosition = Vector3.zero;
                    go.transform.Rotate(weaponRotation);
                }

                go.name += "www";
            }


        }
    }

    [ContextMenu("Delete Prefs")]
    public void DeletePrefs()
    {
        PlayerPrefs.DeleteAll();
    }

}
