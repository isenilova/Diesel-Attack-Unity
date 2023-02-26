using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pauser : MonoBehaviour
{

	public GameObject view;

	public static Pauser instance;
	private void Awake()
	{
		instance = this;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 7") || (Input.GetKeyDown("p")))
		{
			if (view.activeSelf || (GUIOptions.instance != null && GUIOptions.instance.gameObject.activeSelf))
			{
				Time.timeScale = 1;
				JoystMaker.instance.done = false;
				HighlighterUI.instance.UnhighlightAll();
				view.SetActive(false);
				
				if (GUIOptions.instance != null) GUIOptions.instance.gameObject.SetActive(false);
			}
			else
			{
				Time.timeScale = 0;
				view.SetActive(true);
			}
		}
	}

	
	private void OnApplicationFocus(bool hasFocus)
	{
		Debug.Log("focusing " + hasFocus);
		if (!hasFocus)
		{
			Time.timeScale = 0;
			view.SetActive(true);
		}
	}
	
	private void OnApplicationPause(bool hasFocus)
	{
		Debug.Log("pausing " + hasFocus);
		if (hasFocus)
		{
			Time.timeScale = 0;
			view.SetActive(true);
		}
	}


	private bool pressedLock;
	private bool pressedShield;

	private GameObject goLock;
	private GameObject goShield;
	private void LateUpdate()
	{

		if (SceneManager.GetActiveScene().name.IndexOf("1_1") >= 0)
		{
			if (!pressedShield && goShield == null)
			{
				goShield  = (GameObject)Instantiate(Resources.Load("UI/Tuta"), transform);
				goShield.GetComponentInChildren<Text>().text = "Press Q to activate shield";
			}

			if ((Input.GetKeyDown("q") ||  Input.GetKeyDown("joystick button 0")) && !pressedShield)
			{
				pressedShield = true;
				StartCoroutine(Fadi(goShield));
			}
		}
		
		if (SceneManager.GetActiveScene().name.IndexOf("2_1") >= 0)
		{
			if (!pressedLock  && goLock == null)
			{
				goLock  = (GameObject)Instantiate(Resources.Load("UI/Tuta"), transform);
				goLock.GetComponentInChildren<Text>().text = "Press E to lock/unlock weapons";
			}

			if ((Input.GetKeyDown("e") ||  Input.GetKeyDown("joystick button 3")) && !pressedLock)
			{
				pressedLock = true;
				StartCoroutine(Fadi(goLock));
			}
		}
		
		
		
		//testing
		
		/*
		if (Input.GetKeyDown("5"))
		{
			Unlock(2, -1);
		}

		if (Input.GetKeyDown("6"))
		{
			Unlock(-1, 4);
		}
		*/

	}

	public void Unlock(int slot, int weap, int ship = -1)
	{

		if (ship > 0)
		{
			if (PlayerData.player.maxShip >= ship) return;
			var go1 = (GameObject)Instantiate(Resources.Load("UI/Unlock"), transform);
			PlayerData.player.maxShip = ship;
			PlayerData.Save();
			var spr = Resources.Load<Sprite>("ships/" + "ship002");
			go1.transform.GetChild(1).GetComponent<Image>().sprite = spr;
			go1.transform.GetChild(1).GetComponent<Image>().color = Color.white;
			go1.GetComponentInChildren<Text>().text = "You have unlocked new ship";
			
			StartCoroutine(Going(go1));
			return;
		}
		
		if (slot <0 && weap < 0) return;
		
		if (PlayerPrefs.HasKey("W"))
		{
			int u = PlayerPrefs.GetInt("W");
			if (slot < u) slot = -1;
		}
		
		if (PlayerPrefs.HasKey("Weap"))
		{
			int u = PlayerPrefs.GetInt("Weap");
			if (weap < u) weap = -1;
		}
		
		if (slot <0 && weap < 0) return;
		//check for same
		
		var go = (GameObject)Instantiate(Resources.Load("UI/Unlock"), transform);

		if (slot >= 0 && weap >= 0)
		{
			go.GetComponentInChildren<Text>().text = "You have unlocked weapon and slot";
			var spr = Resources.Load<Sprite>("guns/" + "gun" + weap.ToString());
			go.transform.GetChild(1).gameObject.SetActive(false);
			go.transform.GetChild(2).gameObject.SetActive(true);
			go.transform.GetChild(3).gameObject.SetActive(true);
			go.transform.GetChild(3).GetComponent<Image>().sprite = spr;
			go.transform.GetChild(3).GetComponent<Image>().color = Color.white;
			
			PlayerPrefs.SetInt("W", slot + 1);
			PlayerData.BuySlot(slot);
			PlayerPrefs.SetInt("Weap", weap + 1);
		}
		else
		{
			if (slot >= 0)
			{
				go.GetComponentInChildren<Text>().text = "You have unlocked slot " + slot.ToString();
				PlayerPrefs.SetInt("W", slot + 1);
				PlayerData.BuySlot(slot);
			}
			else if (weap >= 0)
			{
				PlayerPrefs.SetInt("Weap", weap + 1);
				var spr = Resources.Load<Sprite>("guns/" + "gun" + weap.ToString());
				go.transform.GetChild(1).GetComponent<Image>().sprite = spr;
				go.transform.GetChild(1).GetComponent<Image>().color = Color.white;
				go.GetComponentInChildren<Text>().text = "You have unlocked weapon " + (weap).ToString();
			}
		}

		StartCoroutine(Going(go));

	}

	public IEnumerator Going(GameObject go)
	{
		float dy = 0;
		float spd = 500;

		while (dy < 500)
		{
			go.transform.position += new Vector3(0,Time.deltaTime * spd,0);
			dy += Time.deltaTime * spd;
			yield return null;
		}
		
		yield return new WaitForSeconds(0.8f);

		var scl = go.GetComponent<CanvasGroup>();
		while (scl.alpha > 0)
		{
			scl.alpha -= Time.deltaTime * 2;
			yield return null;
		}
		
		Destroy(go);
		
		yield return null;
	}

	public IEnumerator Fadi(GameObject go)
	{
		var scl = go.GetComponent<CanvasGroup>();
		while (scl.alpha > 0)
		{
			scl.alpha -= Time.deltaTime * 2;
			yield return null;
		}
		
		Destroy(go);
		
		yield return null;
	}
	
}
