using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{

	public GameObject options;


	public GameObject newgame;

	public GameObject records;
	// Use this for initialization
	public void CloseAll()
	{
		options.SetActive(false);
		newgame.SetActive(false);
		records.SetActive(false);
		
	}

	public void OpenOptions()
	{
		options.SetActive(true);
	}
	
	public void OpenRecords()
	{
		records.SetActive(true);
	}


	public void OpenNewgame()
	{
		newgame.SetActive(true);
	}

	public void Go()
	{
		SceneManager.LoadScene(1);
	}
	
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
