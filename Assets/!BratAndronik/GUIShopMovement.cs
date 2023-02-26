using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIShopMovement : MonoBehaviour
{
    public int moveVale;
    
    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("M")) PlayerPrefs.SetInt("M", 0);
        moveVale = PlayerPrefs.GetInt("M");

        for (int i = 0; i < moveVale; i++)
        {
            
            transform.GetChild(i).GetComponent<GUIShopItm>().GetBuyed();
            
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
