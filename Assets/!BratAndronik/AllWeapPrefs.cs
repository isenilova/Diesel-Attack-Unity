using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllWeapPrefs : MonoBehaviour
{
    public bool test = true;

    public int testVal = 3;
    
     
    
    // Start is called before the first frame update
    void Start()
    {
        
        PlayerData.Load();
        Debug.Log(PlayerData.player.slot0);
        Debug.Log(PlayerData.player.slot1);
        Debug.Log(PlayerData.player.slot2);
        
        FindObjectOfType<Checko>().ClickShip(PlayerData.player.curShip);
        
        if(!PlayerPrefs.HasKey("Weap")) PlayerPrefs.SetInt("Weap", 0);
        
        if (test) PlayerPrefs.SetInt("Weap", testVal);

/*
        for (int i = 0; i < PlayerPrefs.GetInt("Weap"); i++) {
            
            
            transform.GetChild(i).gameObject.GetComponent<GUIShopItm>().GetBuyed();
        }
        */

        int yy = PlayerPrefs.GetInt("Weap");
        for (int i = 0; i < 5; i++)
        {
            int uu = 1 << i;
            var t0 = uu & yy;
            if (t0 > 0)
            transform.GetChild(i).gameObject.GetComponent<GUIShopItm>().GetBuyed();
        }

    }


}
