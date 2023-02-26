using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIToutchControl : MonoBehaviour
{
    
    
    public GUIShopToutch myToutch;

    public GUIShopBuyButton buyButScr;

    public GameObject[] mySlots;

    private bool findSlot = false;

    private float epsilon = 75f;

    private int k;

    public GUIShopSlots slotsScr;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (findSlot)
        {
            //Debug.Log("K="+k);
            
            k = CheckSlots();
            
            ContrHighLights(k);




        }




    }

    public void onButtonWeaponClick(int num)
    {
        Debug.Log(transform.GetChild(num).GetComponent<GUIShopItm>().active);
        if(!transform.GetChild(num).GetComponent<GUIShopItm>().active) return;
        myToutch.SetActive(transform.GetChild(num).GetChild(2).GetComponent<Image>().sprite, transform.GetChild(num).position);
        
        buyButScr.Select(num+3);

        findSlot = true;
    }


    public void onPoinerUp()
    {
        
        myToutch.SetDisable();
        findSlot = false;
        
        ContrHighLights(-1);

        k = CheckSlots();

        if (k != -1)
        {
            PlayerData.Equip(k, buyButScr.curnum);
            
            slotsScr.RewriteSlot(k, buyButScr.curnum);
            
            
            
            
        }

    }

    int CheckSlots()
    {
       int n = -1;

        float curdist = 1000;

        for (int i = 0; i < mySlots.Length; i++)
        {
            if ((mySlots[i].GetComponent<GUIShopItm>().active) &&
                ((myToutch.gameObject.transform.position - mySlots[i].transform.position).magnitude < Mathf.Min(epsilon, curdist)))
            {
                curdist = (myToutch.gameObject.transform.position - mySlots[i].transform.position).magnitude;

                n = i;

            }

            else
            {
                //Debug.Log((myToutch.gameObject.transform.position - mySlots[i].transform.position).magnitude);
            }


        }


        return n;
    }

    void ContrHighLights(int num)
    {
        for (int i = 0; i < mySlots.Length; i++)
        {
            if(i != num) mySlots[i].transform.GetChild(3).gameObject.SetActive(false);
            else
            {
                var tt = PlayerPrefs.GetInt("W");
                //Debug.Log(tt);
                //Debug.Log(num);
                //Debug.Log("-----------");
                if (num < tt)
                {
                    mySlots[i].transform.GetChild(3).gameObject.SetActive(true);
                }
                else
                {
                    mySlots[i].transform.GetChild(3).gameObject.SetActive(false);
                }
            }
            
            
            
        }

    }

}
