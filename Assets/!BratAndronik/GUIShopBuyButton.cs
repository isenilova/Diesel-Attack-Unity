using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIShopBuyButton : MonoBehaviour
{
    public GameObject[] shields;
    public GameObject[] weapons;
    public GameObject[] move;
    public GameObject[] slots;

    public int lastEquipSlot = 0;

    public string curTupe = "";
   public int curnum = 0;

    private GameObject myScore;

    public GUIShopSlots slotScr;
    
    
   
    
    // Start is called before the first frame update
    void Start()
    {
        myScore = GameObject.FindGameObjectWithTag("gold");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Select(int num)
    {

        Debug.Log(num);
        if (num<3)
        {
           LastHighLight();
            
            if(shields[num].GetComponent<GUIShopItm>().active) return;


            curTupe = "shields";
            curnum = num;
            

            shields[num].GetComponent<GUIShopItm>().GetHighLight();

        }
        
        
        
        if ((num>=3)&&(num<=7))
        {
            num -= 3;
            
            LastHighLight();
            //weapons[curnum].GetComponent<GUIShopItm>().OffHighLight();
            weapons[num].GetComponent<GUIShopItm>().GetHighLight();
            
            //if(weapons[num].GetComponent<GUIShopItm>().active) 


            curTupe = "weapons";
            curnum = num;
            

            //shields[num].GetComponent<GUIShopItm>().GetHighLight();

        }
        
        
        if ((num>=8)&&(num<=10))
        {
            num -= 8;
            LastHighLight();
            
            if(move[num].GetComponent<GUIShopItm>().active) return;


            curTupe = "move";
            curnum = num;
            

            move[num].GetComponent<GUIShopItm>().GetHighLight();

        }
        
        if ((num >= 11)&&(num <= 13))
        {
            num -= 11;
            LastHighLight();
            
            if(slots[num].GetComponent<GUIShopItm>().active) return;


            curTupe = "slots";
            curnum = num;
            

            slots[num].GetComponent<GUIShopItm>().GetHighLight();

        }


    }

    void LastHighLight()
    {

        if (curTupe == "shields")
        {
            
            if((curnum >=0)&&(curnum<=2)) shields[curnum].GetComponent<GUIShopItm>().OffHighLight();
            
        }
        
        if (curTupe == "weapons")
        {
            
            if((curnum >=0)&&(curnum<=4)) weapons[curnum].GetComponent<GUIShopItm>().OffHighLight();
            
        }
        
        if (curTupe == "move")
        {
            
            if((curnum >=0)&&(curnum<=2)) move[curnum].GetComponent<GUIShopItm>().OffHighLight();
            
        }
        
        if (curTupe == "slots")
        {
            
            if((curnum >=0)&&(curnum<=2)) slots[curnum].GetComponent<GUIShopItm>().OffHighLight();
            
        }


    }


    public void Buy()
    {
        if (curTupe == "shields")
        {
            
            if(shields[curnum].GetComponent<GUIShopItm>().active) return;
            
            if(curnum > 0) if(!shields[curnum -1].GetComponent<GUIShopItm>().active) return;
            
            if( shields[curnum].GetComponent<GUIShopItm>().cost >  PlayerPrefs.GetInt("Score")) return;
            
            //shields[curnum].GetComponent<GUIShopItm>().GetHighLight();
            
            shields[curnum].GetComponent<GUIShopItm>().OffHighLight();
            
            
            myScore.GetComponent<GUIShopScore>().BuySmt(shields[curnum].GetComponent<GUIShopItm>().cost);
            
            shields[curnum].GetComponent<GUIShopItm>().GetBuyed();
            
            PlayerPrefs.SetInt("S", curnum+1);
            
            
        }
        
        if (curTupe == "weapons")
        {
            
            if(weapons[curnum].GetComponent<GUIShopItm>().active) return;
            
            if(curnum > 0) if(!weapons[curnum -1].GetComponent<GUIShopItm>().active) return;
            
            if( weapons[curnum].GetComponent<GUIShopItm>().cost >  PlayerPrefs.GetInt("Score")) return;
            
            //shields[curnum].GetComponent<GUIShopItm>().GetHighLight();
            
            //if(curnum > 0) shields[curnum-1].GetComponent<GUIShopItm>().OffHighLight();
            
            
            myScore.GetComponent<GUIShopScore>().BuySmt(weapons[curnum].GetComponent<GUIShopItm>().cost);
            
            weapons[curnum].GetComponent<GUIShopItm>().GetBuyed();
            
            PlayerPrefs.SetInt("Weap", curnum+1);
            
            //Equip();
            
        }
        
        if (curTupe == "move")
        {
            
            if(move[curnum].GetComponent<GUIShopItm>().active) return;
            
            if(curnum > 0) if(!move[curnum -1].GetComponent<GUIShopItm>().active) return;
            
            if( move[curnum].GetComponent<GUIShopItm>().cost >  PlayerPrefs.GetInt("Score")) return;
            
            //shields[curnum].GetComponent<GUIShopItm>().GetHighLight();
            
            move[curnum].GetComponent<GUIShopItm>().OffHighLight();
            
            
            myScore.GetComponent<GUIShopScore>().BuySmt(move[curnum].GetComponent<GUIShopItm>().cost);
            
            move[curnum].GetComponent<GUIShopItm>().GetBuyed();
            
            PlayerPrefs.SetInt("M", curnum+1);
            
            
        }

        if (curTupe == "slots")
        {
            
            if(slots[curnum].GetComponent<GUIShopItm>().active) return;
            
            if(curnum > 0) if(!slots[curnum -1].GetComponent<GUIShopItm>().active) return;
            
            if( slots[curnum].GetComponent<GUIShopItm>().cost >  PlayerPrefs.GetInt("Score")) return;
            
            //shields[curnum].GetComponent<GUIShopItm>().GetHighLight();
            
            slots[curnum].GetComponent<GUIShopItm>().OffHighLight();
            
            
            myScore.GetComponent<GUIShopScore>().BuySmt(slots[curnum].GetComponent<GUIShopItm>().cost);
            
           slots[curnum].GetComponent<GUIShopItm>().GetBuyed();
            
            PlayerPrefs.SetInt("W", curnum+1);

            PlayerData.BuySlot(curnum);
            //Equip();

            //shitting technologies
            /*
            var f = PlayerPrefs.GetInt("WeaponList");
            if (curnum == 1) f += 10;
            if (curnum == 2) f += 100;
            PlayerPrefs.SetInt("WeaponList", f);

            FindObjectOfType<GUIShopSlots>().config[curnum] = 1;
            */
            

        }


    }


    public void Equip()
    {
        
        if(curTupe != "weapons") return;
        if(!weapons[curnum].GetComponent<GUIShopItm>().active) return;

        PlayerData.Equip(lastEquipSlot, curnum);
        

        slotScr.RewriteSlot(lastEquipSlot, curnum);
        
        
        //slots[lastEquipSlot].transform.GetChild(2).gameObject.GetComponent<Image>().overrideSprite =
          //  weapons[curnum].transform.GetChild(2).GetComponent<Image>().sprite;

        LastHighLight();
        
        curTupe = "slots";
        curnum = lastEquipSlot;
        
        
        lastEquipSlot++;

        if (lastEquipSlot >= PlayerPrefs.GetInt("W")) lastEquipSlot = 0;
        
       


    }



}
