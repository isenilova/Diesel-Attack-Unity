using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIShopScore : MonoBehaviour
{
    private int k;
    
    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("Score")) PlayerPrefs.SetInt("Score", 0);
        
        //PlayerPrefs.SetInt("Score", 10000);

        gameObject.GetComponent<Text>().text = PlayerPrefs.GetInt("Score").ToString()+ " points";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuySmt(int cost)
    {

        k = PlayerPrefs.GetInt("Score");
         if(k < cost) return;

        k -= cost;
        
        PlayerPrefs.SetInt("Score", k);

        gameObject.GetComponent<Text>().text = PlayerPrefs.GetInt("Score").ToString()+ " points";

    }

}
