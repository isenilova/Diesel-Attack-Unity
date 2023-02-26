using System.Collections;
using System.Collections.Generic;
//using Steamworks;
using UnityEngine;

public class ShootControl : MonoBehaviour
{

	public float dmg = 100;
    public float atkSpeed = 0.5f;
    float t = 0;
    public GameObject proj;

	public float atkDiv = 1.0f;
	public float dmgDiv = 1.0f;
	public float projSpeed = 20;
	public float projSpdDiv = 1.0f;

	public float maxAngl = 30;
	public int cnt = 5;
	public float diviation = 0.1f;

	public float projLength = 5;
	public float live = 5;

	public ShootType shoot = ShootType.common;

	public AudioClip shot;
	public float volume = 0.5f;

	public string rt = "1";
	
	public enum  ShootType
	{
		common,
		rockets,
		laser,
		angle
	}

	private string fireBtn = "Fire1";

	public GameObject shtParticle;

	public float sdvig = 0.5f;

	public bool autoAtk = false;

	public bool second;
	void Start()
	{
		if (GetComponentInParent<OneShip>() != null)
		{
			rt = GetComponentInParent<OneShip>().id;
			second = GetComponentInParent<OneShip>().second;
			if (rt == "2")
			{
				fireBtn = "Fire2";
			}
		}
	}
	// Update is called once per frame
	void Update ()
    {

        t -= Time.deltaTime;

        if ((Input.GetAxis(fireBtn) > 0 || autoAtk) && t < 0)
        {
	        if (shot != null)
	        {
		        AudioSource.PlayClipAtPoint(shot, Camera.main.transform.position, volume);
	        }

	        if (shtParticle != null)
	        {
		        var gp = (GameObject) Instantiate(shtParticle);
		        gp.transform.SetParent(transform);
		        gp.transform.localPosition = new Vector3(-2*sdvig, 0, 0);
		        gp.transform.rotation = transform.rotation;
		        gp.transform.Rotate(0,180,0);
	        }
	        
	        if (shoot == ShootType.common)
	        {
		        var go = (GameObject) Instantiate(proj);


		        go.GetComponentInChildren<Damage>().amnt = dmg;
		        go.GetComponentInChildren<Damage>().plNum = rt;
		        
		        
		        var sp = projSpeed * projSpdDiv;
		        var ang = transform.parent.localEulerAngles.z;

		        go.transform.position = transform.position - transform.right;

		        if (!second)
		        {
			        go.GetComponent<fall>().sx = sp * Mathf.Cos(ang * Mathf.PI / 180);
			        go.GetComponent<fall>().sy = -sp * Mathf.Sin(ang * Mathf.PI / 180);
		        }
		        else
		        {
			        go.GetComponent<fall>().sx = sp * -transform.right.x;
			        go.GetComponent<fall>().sy = sp * -transform.right.y;
		        }

		        //ebug.Log(go.GetComponent<fall>().sx);
		        //Debug.Log(go.GetComponent<fall>().sy);

		        t = atkSpeed * atkDiv;
	        }

	        if (shoot == ShootType.rockets)
	        {
		        var go = (GameObject) Instantiate(proj);
		        
		        go.GetComponentInChildren<Damage>().amnt = dmg;
		        go.GetComponentInChildren<Damage>().plNum = rt;
		        
		        go.transform.position = transform.position;
		        go.GetComponent<LineFollow>().dir = 1;
		        go.GetComponent<LineFollow>().pos = transform.position;
		        go.GetComponent<LineFollow>().vec = -transform.right;
		        go.GetComponent<LineFollow>().mul = projSpeed * projSpdDiv / 2;
		        
		        
		        var go1 = (GameObject) Instantiate(proj);
		        
		        go1.GetComponentInChildren<Damage>().amnt = dmg;
		        
		        go1.transform.position = transform.position;
		        go1.GetComponent<LineFollow>().dir = -1;
		        go1.GetComponent<LineFollow>().pos = transform.position;
		        go1.GetComponent<LineFollow>().vec = -transform.right;
		        go1.GetComponent<LineFollow>().mul = projSpeed * projSpdDiv / 2;
		        
		        t = atkSpeed * atkDiv;
	        }
	        
	        if (shoot == ShootType.angle)
	        {
		        for (int i = 0; i < cnt; i++)
		        {
			        var go = (GameObject) Instantiate(proj);

			        go.GetComponentInChildren<Damage>().amnt = dmg;
			        go.GetComponentInChildren<Damage>().plNum = rt;
			        
			        var sp = projSpeed * (1 + Random.Range(-diviation, diviation) * projSpdDiv);
			        var ang = transform.parent.localEulerAngles.z + Random.Range(-maxAngl, maxAngl);

			        go.transform.position = transform.position - transform.right;

			        go.GetComponent<fall>().sx = sp * Mathf.Cos(ang * Mathf.PI / 180);
			        go.GetComponent<fall>().sy = -sp * Mathf.Sin(ang * Mathf.PI / 180);

			        Debug.Log(go.GetComponent<fall>().sx);
			        Debug.Log(go.GetComponent<fall>().sy);

			        t = atkSpeed * atkDiv;
		        }
	        }

	        if (shoot == ShootType.laser)
	        {
		        var go = (GameObject) Instantiate(proj);
		        
		        go.GetComponentInChildren<Damage>().amnt = dmg;
		        go.GetComponentInChildren<Damage>().plNum = rt;
		        
		        go.transform.position = transform.position - transform.right;

		        go.transform.right = transform.right;

		        go.AddComponent<LaserMove>();
		        go.GetComponent<LaserMove>().vec = -transform.right;
		        go.GetComponent<LaserMove>().len = projLength;
		        go.transform.localScale = new Vector3(projLength, go.transform.localScale.y, go.transform.localScale.z);
		        go.GetComponent<LaserMove>().liveTime = live;
		        
		        t = atkSpeed * atkDiv;
	        }
	        
        }
        
	}
}
