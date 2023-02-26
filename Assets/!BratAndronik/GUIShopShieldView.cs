using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIShopShieldView : MonoBehaviour
{

    public int shieldVal;
    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("S")) PlayerPrefs.SetInt("S", 0);
        shieldVal = PlayerPrefs.GetInt("S");

        //PlayerPrefs.SetInt("S", 0);
        
        for (int i = 0; i < shieldVal; i++)
        {
            
            transform.GetChild(i).GetComponent<GUIShopItm>().GetBuyed();
            
        }
        
        
      // if(shieldVal >0) transform.GetChild(shieldVal-1).GetComponent<GUIShopItm>().GetHighLight();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
