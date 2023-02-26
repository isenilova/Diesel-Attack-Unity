using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControl : MonoBehaviour
{

	public GameObject[] menuObjects;
	public string[] funcs;

	public GameObject view;

	private GameObject curSelect = null;

	private int curObj = -1;
	// Use this for initialization
	public GameObject[] hltors;

	public float tMax = 0.5f;
	public float t = 0;
	public void ClearHltors()
	{
		for (int i = 0; i < hltors.Length; i++)
			hltors[i].SetActive(false);
	}

	// Update is called once per frame
	void Update ()
	{
		//Debug.Log(Input.GetAxis("Horos"));
		t -= 0.03f;
			
		if (view.activeSelf && curObj < 0)
		{
			curObj = 0;
			curSelect = menuObjects[curObj];
			//do high lighting
		}

		if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Horos") < -0.5f) && t < 0 && view.activeSelf)
		{
			Debug.Log(Input.GetAxis("Horos"));
			curObj = (curObj + menuObjects.Length - 1) % menuObjects.Length;
			t = tMax;
		}
		
		if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Horos") > 0.5f) && t < 0 && view.activeSelf)
		{
			Debug.Log(Input.GetAxis("Horos"));
			curObj = (curObj + 1) % menuObjects.Length;
			t = tMax;
		}

		if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 1")) && view.activeSelf)
		{
			gameObject.SendMessage(funcs[curObj], SendMessageOptions.DontRequireReceiver);
		}

		if (view.activeSelf)
		{
			ClearHltors();
			hltors[curObj].SetActive(true);
		}
		else
		{
			ClearHltors();
		}
	}
}
