using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Noselect : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
		if (!PlayerPrefs.HasKey("player"))
        {
            GetComponent<Image>().color = Color.grey;
            GetComponentInChildren<Text>().color = Color.grey;
            GetComponent<Button>().onClick.RemoveAllListeners();
        }
	}
	

}
