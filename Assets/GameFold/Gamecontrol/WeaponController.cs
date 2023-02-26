using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

	public string id = "1";
	public static WeaponController instance;
	public static WeaponController instance1;
	
	public Transform[] slots;

	private int curSLot = 1;

	void Awake()
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
	
	public static WeaponController GetBy(string id)
	{
		if (id == "1") return instance;
		else return instance1;
	}

	public void PickWeapon(string what, string prefab)
	{
		bool doSave = false;
		Quaternion savedRot = Quaternion.identity;
		if (slots[curSLot].childCount > 0)
		{
			doSave = true;
			savedRot = slots[curSLot].GetChild(0).localRotation;
			Destroy(slots[curSLot].GetChild(0).gameObject);

		}
		
		var go = (GameObject) Instantiate(Resources.Load("Weapons/" + prefab));
		go.transform.SetParent(slots[curSLot]);
		go.transform.localPosition = Vector3.zero;

		if (doSave)
		{
			go.transform.localRotation = savedRot;
		}
		//go.transform.localRotation = Quaternion.identity;

		curSLot = (curSLot + 1) % 3;
	}
	
}
