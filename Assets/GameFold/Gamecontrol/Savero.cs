using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Savero : MonoBehaviour
{

	public static Savero instance;

	public float tStart = 0;
	private bool thisOne = false;
	
	private void Awake()
	{
		var obj1 = FindObjectsOfType<Savero>();
		if (obj1.Length > 1 && !thisOne) Destroy(gameObject);

		thisOne = true;
		
		instance = this;
	}

	// Use this for initialization
	void Start () {
		
		DontDestroyOnLoad(this);
	}
	
	void OnEnable() {
		SceneManager.sceneLoaded += OnSceneLoaded;
	}
 
	void OnDisable() {
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
 
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		//do stuff

		/*
		Debug.Log("LOADED");
		TimeController.instance.tm = tStart;
		
		//delete all objects that are no longer spawn
		var obj1 = FindObjectsOfType<BezSpawner>();
		for (int i = 0; i < obj1.Length; i++)
		{
			if (obj1[i].timeActivate < tStart)
			{
				obj1[i].StopAllCoroutines();
				Destroy(obj1[i].gameObject);
			}
		}

		var obj2 = FindObjectsOfType<ThrashSpawner>();
		for (int i = 0; i < obj2.Length; i++)
		{
			if (obj2[i].startTime < tStart)
			{
				obj2[i].StopAllCoroutines();
				Destroy(obj2[i].gameObject);
			}
		}
		
		var obj3 = FindObjectsOfType<MidPlatform>();
		for (int i = 0; i < obj3.Length; i++)
		{
			if (obj3[i].appearTime < tStart)
			{
				obj3[i].StopAllCoroutines();
				Destroy(obj3[i].gameObject);
			}
		}
	*/
	}
	
	
}
