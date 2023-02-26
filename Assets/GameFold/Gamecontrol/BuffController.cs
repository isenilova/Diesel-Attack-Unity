using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController : MonoBehaviour
{

	public string id = "1";
	public static BuffController instance;
	public static BuffController instance1;
	
	private float startSpdX = 0;
	private float startSpdY = 0;
	private float startAtkSpd;
	
	public MoveControl ship;

	private float spdT = 0;
	private float atkSpdT = 0;
	private float projSpdT = 0;
	private float atkandprojT = 0;
	
	
	private float curSpdModify = 0;
	private float curAtkModify = 1;
	private float curProjSpdModify = 1;

	
	
	
	private void Awake()
	{
		if (id == "1")
		{
			instance = this;
		}
		else
		{
			instance1 = this;
		}
	}

	public static BuffController GetBy(string id)
	{
		if (id == "1") return instance;
		else return instance1;
	}

	private void Start()
	{
		startSpdX = ship.speedX;
		startSpdY = ship.speedY;
	}

	public void PickSpeed(float spdModify, float duration)
	{
		curSpdModify = spdModify;
		if (spdT < 0)
		{
			spdT = duration;
		}
		else
		{
			spdT += duration;
		}
	}
	
	public void PickAtkSpeed(float spdModify, float duration)
	{
		curAtkModify = spdModify;
		if (atkSpdT < 0)
		{
			atkSpdT = duration;
		}
		else
		{
			atkSpdT += duration;
		}
	}
	
	public void PickAtkAndProj(float spdModify, float projSpd, float duration)
	{
		curAtkModify = spdModify;
		curProjSpdModify = projSpd;
		if (atkandprojT < 0)
		{
			atkandprojT = duration;
		}
		else
		{
			atkandprojT += duration;
		}
	}
	
	public void PickProjSpeed(float spdModify, float duration)
	{
		curProjSpdModify = spdModify;
		if (projSpdT < 0)
		{
			projSpdT = duration;
		}
		else
		{
			projSpdT += duration;
		}
	}
	
	

	void Update()
	{
		spdT -= Time.deltaTime;
		atkSpdT -= Time.deltaTime;
		projSpdT -= Time.deltaTime;
		atkandprojT -= Time.deltaTime;
		
		if (atkSpdT < 0)
		{
			if (atkandprojT < 0)
			{
				var c1 = ship.GetComponentsInChildren<ShootControl>();
				for (int i = 0; i < c1.Length; i++)
				{
					c1[i].atkDiv = 1.0f;
				}
			}
		}
		else
		{

				var c1 = ship.GetComponentsInChildren<ShootControl>();
				for (int i = 0; i < c1.Length; i++)
				{
					c1[i].atkDiv = curAtkModify;
				}
		}
		
		if (projSpdT < 0)
		{
			if (atkandprojT < 0)
			{
				var c1 = ship.GetComponentsInChildren<ShootControl>();
				for (int i = 0; i < c1.Length; i++)
				{
					c1[i].projSpdDiv = 1.0f;
				}
			}
		}
		else
		{

				var c1 = ship.GetComponentsInChildren<ShootControl>();
				for (int i = 0; i < c1.Length; i++)
				{
					c1[i].projSpdDiv = curProjSpdModify;
				}
		}

		if (spdT < 0)
		{
			ship.speedX = startSpdX;
			ship.speedY = startSpdY;
		}
		else
		{
			ship.speedX = startSpdX + curSpdModify;
			ship.speedY = startSpdY + curSpdModify;
		}
		
		
		if (atkandprojT < 0)
		{
			if (atkSpdT < 0)
			{
				var c1 = ship.GetComponentsInChildren<ShootControl>();
				for (int i = 0; i < c1.Length; i++)
				{
					c1[i].atkDiv = 1.0f;
				}
			}
			
			if (projSpdT < 0)
			{
				var c1 = ship.GetComponentsInChildren<ShootControl>();
				for (int i = 0; i < c1.Length; i++)
				{
					c1[i].projSpdDiv = 1.0f;
				}
			}
			
		}
		else
		{

			var c1 = ship.GetComponentsInChildren<ShootControl>();
			for (int i = 0; i < c1.Length; i++)
			{
				c1[i].atkDiv = curAtkModify;
			}
			
			c1 = ship.GetComponentsInChildren<ShootControl>();
			for (int i = 0; i < c1.Length; i++)
			{
				c1[i].projSpdDiv = curProjSpdModify;
			}
			
		}
	}
}
