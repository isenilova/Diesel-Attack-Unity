using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIShopItm : MonoBehaviour
{
    public bool active = false;
    public int cost = 100;


   
    
    
    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
        
        if(active) GetBuyed();

        transform.GetChild(4).gameObject.GetComponent<Text>().text = cost.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetBuyed()

    {
        active = true;
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(true);
        transform.GetChild(4).gameObject.SetActive(false);
        
        
        
        
        
        
    }

    public void GetHighLight()
    {

        if (transform.GetChild(1).gameObject.activeSelf)
        {
            return;
        }
        
        transform.GetChild(3).gameObject.SetActive(true);
        
    }
    
    public void OffHighLight()
    {
        
        transform.GetChild(3).gameObject.SetActive(false);
        
    }

}
