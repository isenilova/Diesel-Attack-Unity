using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneDeath : MonoBehaviour
{
	public bool done = false;
	public string id = "";
	// Update is called once per frame
	void Update ()
	{

		var rt = GetComponent<OneHealth>();
		if (rt.curHealth <= 0 && !done)
		{
			done = true;
			GameController.instance.AcceptDeath(id, transform.position);
		}

	}

	private void OnDestroy()
	{
		if (!done)
		{
			done = true;
			GameController.instance.AcceptDeath(id, transform.position);
		}
	}
}
