using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerData
{
    public static Playero player = new Playero();
    


    public static void BuySlot(int num)
    {
        if (num == 1) player.slot1 = 1;
        if (num == 2) player.slot2 = 1;
        
        Save();
    }

    public static void Save()
    {
        var st = JsonUtility.ToJson(player);
        PlayerPrefs.SetString("weaponx", st);
        
    }

    public static void Equip(int num, int weap)
    {
        if (num == 0) player.slot0 = weap + 1;
        if (num == 1) player.slot1 = weap + 1;
        if (num == 2) player.slot2 = weap + 1;
        Save();
    }

    public static void Load()
    {
        if (PlayerPrefs.HasKey("weaponx"))
        {
            var st = JsonUtility.FromJson<Playero>(PlayerPrefs.GetString("weaponx"));
            player = st;
        }
        else
        {
            Save();
        }
        
        PlayerPrefs.SetInt("S", 1);
        PlayerPrefs.SetInt("M", 1);
    }
}

[System.Serializable]
public class Playero
{
    public int slot0 = 1;
    public int slot1 = 0;
    public int slot2 = 0;    
    
    //0 - is not buyed 
    //1 - default weapon
    
    //when slot is buyed its equipped with default weapon   
    
    
    //curship
    public int curShip = 0;

    public int maxShip = 1;
}
