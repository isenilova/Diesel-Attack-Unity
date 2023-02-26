using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIShopSlots : MonoBehaviour
{
    public int slotVal;

    public Sprite empty;
    public Sprite defWeapon;

    public Sprite[] weaponsUpgr;

    public int[] config;

    private int myCode;

    
    
    
    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("W")) PlayerPrefs.SetInt("W", 0);
        slotVal = PlayerPrefs.GetInt("W");

        for (int i = 0; i < slotVal; i++)
        {
            
            transform.GetChild(i).GetComponent<GUIShopItm>().GetBuyed();
            
        }
        
        
        config = new int[3];
       
        
        if(!PlayerPrefs.HasKey("WeaponList")) PlayerPrefs.SetInt("WeaponList", 111);
        
        ParseToArray();
        
        FillSlots();

        if (config[0] < 0) config[0] = 0;


    }

    void ParseToArray()
    {

        int weapDops;
        myCode = PlayerPrefs.GetInt("WeaponList");

       // Debug.Log(myCode);
        
       // Debug.Log((config[0]+2).ToString()+(config[1]+2).ToString()+(config[2]+2).ToString());
        
        weapDops = myCode;
        
       config[2] = weapDops % 10 - 2;

        weapDops = weapDops / 10;

        config[1] = weapDops % 10 - 2;

        weapDops = weapDops / 10;

       config[0] = weapDops - 2;




    }


    void FillSlots()
    {


        //for (int i = 0; i < 3; i++)
            
               // Debug.Log(config[i]);
    


    for (int i = 0; i < transform.childCount; i++)
        {

            if (config[i] >= 0)
                transform.GetChild(i).GetChild(2).gameObject.GetComponent<Image>().overrideSprite =
                    weaponsUpgr[config[i]];

            else transform.GetChild(i).GetChild(2).gameObject.GetComponent<Image>().overrideSprite = defWeapon;


        }


    }


    void ParseToNumber()
    {


        myCode = (config[0] + 2) * 100 + (config[1] + 2) * 10 + (config[2] + 2);
        
        
        PlayerPrefs.SetInt("WeaponList", myCode);

        //ParseToArray();




    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void RewriteSlot(int num, int weap)
    {
        //ParseToArray();

        Debug.Log(num + " " + weap);

        config[num] = weap;
        
        
        FillSlots();
        
        ParseToNumber();
        
        

    }

}
