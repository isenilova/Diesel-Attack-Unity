using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChooser : MonoBehaviour {

	public void Go1()
	{
		ClickLevel("Level_1_1");
	}

	public void Go2()
	{
		ClickLevel("Level_2_1");
	}
	
	public void ClickLevel(string lvl)
	{
		SceneManager.LoadScene(lvl);
	}
}
