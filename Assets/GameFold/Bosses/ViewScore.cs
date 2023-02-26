using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewScore : MonoBehaviour
{
	private int score = 0;

	public Text scor;
	// Use this for initialization
	void Start () {
		EventManager.StartListening(EvtConsts.PLAYER_GET_SCORE, SomeFunction);
	}
	
	void SomeFunction(ParamsEvt e)
	{
		Debug.Log("Some Function was called!");
		score += e.score;
		scor.text = score.ToString();
	}
	
	
}
