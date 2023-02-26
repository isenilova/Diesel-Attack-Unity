using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{

	public OneHealth bossHealth;
	public OneHealth[] myHealth;
	
	private bool isDead = false;
	public GameObject view;

	public float appearTime = 5.0f;

	private float st = 0;
	
	public void PlayAgain()
	{
		Time.timeScale = 1.0f;
		Savero.instance.tStart = st;
		SceneManager.LoadScene(GameController.instance.lvl);
	}

	public void Exit()
	{
		Application.Quit();
	}

	public void BackSelect()
	{
		SceneManager.LoadScene("StartScene");
	}

	public IEnumerator SlowAppear()
	{
		yield return new WaitForSeconds(appearTime);
		
		view.SetActive(true);
	}

	public bool CheckHealthDead()
	{
		bool q = true;
		for (int i = 0; i < myHealth.Length; i++)
		{
			if (myHealth[i] != null && myHealth[i].gameObject.activeInHierarchy && myHealth[i].curHealth > 0) q = false;
		}

		return q;
	}

	private void Update()
	{
		if (isDead) return;

		if (CheckHealthDead() /*|| bossHealth == null || bossHealth.curHealth <= 0*/)
		{
			st = GameController.instance.GetClosestCheckpoint(TimeController.instance.tm);
			Savero.instance.tStart = st;
			isDead = true;
			StartCoroutine(SlowAppear());
		}
	}
}
