using System.Collections;
using System.Collections.Generic;
//using Steamworks;
using UnityEngine;


public class Deado
{
	public int cnt = 0;
	public Vector3 pos;
}

[System.Serializable]
public class Rewards
{
	public string id = "";
	public int cnt = 0;
	public GameObject reward;
	public bool done = false;
	public bool useLoop = false;
}

public class GameController : MonoBehaviour
{

	public string lvl = "Level_1_1";
	public int[] checkpoints;
	public static GameController instance;
	
	
	Dictionary<string, Deado> deadDict = new Dictionary<string, Deado>();
	Dictionary<string, int> pickDict = new Dictionary<string, int>();

	public Rewards[] rewards;

	public OneHealth ship;
	public OneHealth ship1;

	public GameObject bs;
	// Use this for initialization
	void Awake()
	{
		instance = this;
	}

	public float GetClosestCheckpoint(float t)
	{
		float r = 0;
		for (int i = 0; i < checkpoints.Length; i++)
		{
			if (checkpoints[i] <= t) r = checkpoints[i];
		}

		return r;
	}

	public void AcceptDeath(string who, Vector3 pos)
	{
		Debug.Log("death is accepted");
		Debug.Log(who);
		Debug.Log(pos);
		
		if (deadDict.ContainsKey(who))
		{
			var fg = deadDict[who];
			deadDict[who].pos = pos;
			fg.cnt++;
		}
		else
		{
			var cf = new Deado();
			cf.pos = pos;
			deadDict.Add(who, cf);
		}
	}

	public void AcceptPick(string who, string what, string prefab, float duration, float speedModify, float attakSpeedModify, float health, float projSpeedModify)
	{
		Debug.Log("pick is accepted");
		Debug.Log(what);

		if (what.IndexOf("weapon") >= 0)
		{
			WeaponController.GetBy(who).PickWeapon(what, prefab);
			return;
		}

		if (what.IndexOf("speed") >= 0)
		{
			BuffController.GetBy(who).PickSpeed(speedModify, duration);
			return;
		}
		
		if (what.IndexOf("projspd") >= 0)
		{
			BuffController.GetBy(who).PickProjSpeed(projSpeedModify, duration);
			return;
		}
		
		if (what.IndexOf("atkspd") >= 0)
		{
			BuffController.GetBy(who).PickAtkSpeed(attakSpeedModify, duration);
			return;
		}
		
		
		if (what.IndexOf("health") >= 0)
		{
			if (who == "1")
				ship.AddHealth(health);
			else
				ship1.AddHealth(health);
			
			return;
		}
		
		if (what.IndexOf("shield") >= 0)
		{
			if (who == "1")
				ship.AddShield(health, duration);
			else
				ship1.AddShield(health, duration);
			
			return;
		}

		if (what.IndexOf("atkandproj") >= 0)
		{
			BuffController.GetBy(who).PickAtkAndProj(attakSpeedModify, projSpeedModify, duration);
		}

		
		if (pickDict.ContainsKey(what))
		{
			pickDict[what]++;
		}
		else
		{
			pickDict.Add(what, 1);
		}
	}
	// Update is called once per frame
	void Update () 
	{
		for (int i = 0; i < rewards.Length; i++)
		{
			if (rewards[i].done) continue;

			if (deadDict.ContainsKey(rewards[i].id))
			{
				var cf = deadDict[rewards[i].id].cnt;
				if (cf >= rewards[i].cnt)
				{
					rewards[i].done = true;
					var go = (GameObject) Instantiate(rewards[i].reward);
					go.transform.position = deadDict[rewards[i].id].pos;

					if (rewards[i].useLoop)
					{
						rewards[i].done = false;
						deadDict[rewards[i].id].cnt = 0;

					}
				}
			}
			
		}



		if (Input.GetKeyDown("n"))
		{
			var g = GameObject.FindGameObjectWithTag("Player");
			var her = g.GetComponent<Collider2D>().enabled;
			g.GetComponent<Collider2D>().enabled = !her;
		}

		if (Input.GetKeyDown("m"))
		{
			//bs.GetComponent<OneHealth>().curHealth = -100;
			//bs.tag = "Exploder";
			//ExplControl.instance.ExplodeObject(bs);
		}

	}
}
