using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Cooper : MonoBehaviour
{

	public static Cooper instance;

	public static float healthMult = 1.0f;
	public static int mode = 1;

	public GameObject secondShip;
	public GameObject secondHealth;
	
	void Awake()
	{
		instance = this;
		
		var sw = new StreamReader("config.txt");
		var c = sw.ReadLine();
		healthMult = float.Parse(c);
		c = sw.ReadLine();
		mode = int.Parse(c);

		if (mode == 2)
		{
			secondShip.SetActive(true);
			secondHealth.SetActive(true);
		}
	}
	
}
