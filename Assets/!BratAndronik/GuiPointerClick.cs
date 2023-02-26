using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GuiPointerClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public GUIToutchControl mycontr;

    public int num = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnPointerDown(PointerEventData eventData)
    {
      //  Debug.Log("pointer");
        //transform.parent.gameObject.GetComponentInParent<GUIToutchControl>().onButtonWeaponClick(num);
        
        mycontr.onButtonWeaponClick(num);
       
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        mycontr.onPoinerUp();
    }
}
