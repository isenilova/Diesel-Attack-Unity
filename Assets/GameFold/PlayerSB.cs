using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSB : MonoBehaviour {

    public OneHealth oh;

	public float oneHealthLoss = 100;

	public Transform container;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

        //transform.GetChild(0).GetComponent<Image>().fillAmount = oh.curHealth / oh.maxHealth;

	    int cnt = (int) (oh.GetCurShield()/ oneHealthLoss);

	    for (int i = cnt; i < container.childCount; i++)
	    {
		    container.GetChild(i).gameObject.SetActive(false);
	    }

	    for (int i = 0; i < cnt; i++)
	    {
		    container.GetChild(i).gameObject.SetActive(true);
	    }
	    
	    

    }
}
