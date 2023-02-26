using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIShopToutch : MonoBehaviour
{
    public bool active = false;
    public Sprite empty;
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Image>().overrideSprite = empty;
    }

    // Update is called once per frame
    void Update()
    {
        if (active) transform.position = Input.mousePosition;
    }

    public void SetActive(Sprite weap, Vector3 myPos)
    {
        gameObject.GetComponent<Image>().overrideSprite = weap;
        transform.position = myPos;
        active = true;




    }

    public void SetDisable()
    {
        gameObject.GetComponent<Image>().overrideSprite = empty;
        
        active = false;




    }

}
