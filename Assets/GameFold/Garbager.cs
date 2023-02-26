using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garbager : MonoBehaviour {

    public string[] tags = new string[] {"projectile"};
	
	// Update is called once per frame
	void Update ()
    {
        for (int i = 0; i < tags.Length; i++)
        {
            var gos = GameObject.FindGameObjectsWithTag(tags[i]);

            for (int j = 0; j < gos.Length; j++)
            {
                if (gos[j].transform.position.x < CamBound.instance.lox.position.x || 
                    gos[j].transform.position.y < CamBound.instance.loy.position.y ||
                    gos[j].transform.position.x > CamBound.instance.hix.position.x ||
                    gos[j].transform.position.y > CamBound.instance.hiy.position.y


                    )
                {
                    var t = gos[j].GetComponentInChildren<OneDeath>();
                    if (t != null) t.done = true;
                    Destroy(gos[j]);
                }
            }
        }
	}
}
