using System.Collections;
using System.Collections.Generic;
using System.Text;
using MoreMountains.Tools;
using TextFx;
using UnityEngine;

public class WormBoss : MonoBehaviour
{
	public Transform head;

	public Transform[] parts;
	public List<Vector3> poses = new List<Vector3>();
	public List<Vector3> rots = new List<Vector3>();
	public List<Vector3> rights = new List<Vector3>();
	
	private int cnt = 0;
	private int cntMax = 10000;
	public int cntEvery = 30;

	public GameObject player;
	private bool state = false;
	private float dist = 8;

	public float tm = 10;
	public float dltAppear = 1.0f;
	public float explTime = 10.0f;
	
	private bool activated = false;
	public float timePursue = 20.0f;
	private bool pursuing = false;

	private bool dying = false;

	public float fallAccel = 5.0f;

	private bool particleActivated = false;
	public GameObject groundPart;


	public bool testDeath = false;


	private WormDirection dirScr;

	private float len;
	private float oneLen;
	public int curM;

	private void Start()
	{
		dirScr = gameObject.GetComponent<WormDirection>();
	}


	void DoStart()
	{

		float y = FPSCounter.fps;
		Debug.Log(y);
		int re = (int)((3000 - 15 * (170 - y)) / 100);
		cntEvery = re;
		
		Vector3 lastP = parts[parts.Length - 1].position;
		Vector3 dv = (lastP - transform.position) / cntMax;
		
		len = (lastP - transform.position).magnitude;
		oneLen = len / (parts.Length - 1);
		curM = cntMax;
		
		for (float i = 0; i < cntMax; i+= 1.0f)
		{
			poses.Add(lastP - i * dv);
			rots.Add(transform.eulerAngles);
			rights.Add(transform.right);
		}

		cnt = cntMax;
	}

	public List<GameObject> checks = new List<GameObject>();
	public void Checkery()
	{
		float rt = 0;
		for (int i = 1; i < poses.Count; i++)
		{
			rt += (poses[i] - poses[i-1]).magnitude;
			if (rt > len)
			{
				curM = i;
				break;
			}
		}
		
		if (checks.Count < cntMax)
		{
			for (int i = checks.Count; i < cntMax; i++)
			{
				var go = (GameObject) Instantiate(checks[0]);
				go.name += i.ToString();
				checks.Add(go);
			}
		}

		for (int i = 0; i < cntMax; i++)
		{
			checks[i].transform.position = new Vector3(poses[i].x, poses[i].y, 2);
		}
	}

	public void AssignPosesAndRots()
	{
		int cur = 0;
		int u = curM / parts.Length;
		
		/*
		for (int i = 0; i < parts.Length; i++)
		{
			
			Vector3 poso = Vector3.zero;
			Vector3 roto = Vector3.zero;
			for (int j = i * u; j < (i + 1) * u; j++)
			{
				poso += poses[j];
				roto += rots[j];
			}

			poso /= u;
			roto /= u;

			parts[i].position = poso;
			parts[i].localEulerAngles = roto;
			
			

		}
		*/

		float s = 0;
		int k1 = cntMax;
		int k2 = 0;
		int l = -1;
		
		for (int i = cntMax; i >= 1; i--)
		{
			s += (poses[i] - poses[i - 1]).magnitude;

			if (s > oneLen / 2 && l == -1)
			{
				k1 = i;
				s -= oneLen / 2;
				l = 0;
			}
			
			if (s > oneLen)
			{
				//Debug.Log(i);
				k2 = i;
				var tt = (poses[k2] + poses[k1]) / 2;
				parts[l].position = tt;
				parts[l].right = (rights[k2] + rights[k1]) / 2;
				parts[l].transform.RotateAround(parts[l].position, parts[l].transform.right, 180 );
				
				
				if (l == 0)
				{
					//var dlt = parts[l].GetChild(0).position - head.GetChild(0).position;
					//parts[l].transform.Translate(-dlt,Space.World);
				}
				else
				{
					//var dlt = parts[l].GetChild(0).position - parts[l - 1].GetChild(1).position;
					//parts[l].transform.Translate(-dlt,Space.World);
				}
				
				
				l++;
				k1 = i;
				s -= oneLen;

				if (l >= parts.Length) break;
			}
		}
		
	}

	public IEnumerator Purse()
	{
		float t = 0;
		while (t < timePursue)
		{
			t += Time.deltaTime;
			
			if (!pursuing)
			{
				break;
			}
			
			yield return null;
		}

		if (!dying)
		{
			player = GameObject.FindGameObjectWithTag("endworm");
			GetComponent<SmoothFollow>().plr = player;
		}
	}

	bool CheckAll()
	{
		bool q = true;
		for (int i = 0; i < parts.Length; i++)
		{
			bool q1 = false;
			if (parts[i].GetComponentInChildren<OneHealth>() == null ||
			    parts[i].GetComponentInChildren<OneHealth>().curHealth <= 0) q1 = true;

			q = q && q1;
			


		}


		return q;
	}


	public IEnumerator Dying()
	{
		player = GameObject.FindGameObjectWithTag("endworm");
		GetComponent<SmoothFollow>().plr = player;

		yield return null;
		/*
		Destroy(GetComponent<SmoothFollow>());

		float t = 0;
		float v = 0;
		while (t < 10)
		{
			v = t * fallAccel;
			transform.position -= new Vector3(0, v * Time.deltaTime, 0);
			t += Time.deltaTime;
			yield return null;
		}
		*/
	}
	
	void LateUpdate()
	{

		if (TimeController.instance.tm > tm && !activated)
		{
			
			FindObjectOfType<CameraShake>().ShakeCamera();
			
			activated = true;
			DoStart();
			GetComponent<SmoothFollow>().enabled = true;
			Destroy(GetComponent<MoveControl>());
			pursuing = true;
			StartCoroutine(Purse());

			dirScr.actDirection = true;

		}

		if (!activated)
		{
			return;
		}

		if (transform.position.y + dltAppear > CamBound.instance.loy.position.y && !particleActivated)
		{
			//Debug.Break();
			particleActivated = true;
			var go = Instantiate(groundPart);
			go.transform.position = transform.position;
			Destroy(go, explTime);
		}

		
		
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		if(!testDeath) testDeath = CheckAll();
		
		if (testDeath && !dying)
		{
			dying = true;
			pursuing = false;

			dirScr.enabled = false;
			StartCoroutine(Dying());
			
			
			GameObject.FindGameObjectWithTag("SaveController").GetComponent<AchivementController>().KillWorm = 1;
			PlayerPrefs.SetInt("KillWorm", 1);
		}

		//if (dying)
		//{
		//	return;
		//}
		
		poses.Add(transform.position);
		rots.Add(transform.eulerAngles);
		rights.Add(transform.right);

		if (cnt > cntMax)
		{
			poses.RemoveAt(0);
			rots.RemoveAt(0);
			rights.RemoveAt(0);
		}
		
		AssignPosesAndRots();
		//Checkery();
		SetAnim();
		cnt++;
	}

	public void SetAnim()
	{
		if (player == null)
		{
			player = GameObject.FindGameObjectWithTag("Player");
		}
		
		if (player == null) return;

		var dst = (player.transform.position - transform.position).magnitude;
		//Debug.Log(dst);
		if (dst < dist && !state)
		{
			state = true;
			GetComponent<Animator>().CrossFade("open", 0.2f);
		}

		if (dst > dist && state)
		{
			state = false;
			GetComponent<Animator>().CrossFade("close", 0.2f);
		}
	}
}
