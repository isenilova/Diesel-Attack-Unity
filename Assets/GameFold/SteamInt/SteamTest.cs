using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Steamworks;

public class SteamTest : MonoBehaviour {

	
	// Update is called once per frame
	void Update ()
    {
	
        if (Input.GetKeyDown("h"))
        {
           // SteamStatsAndAchievements.instance.ChangeSome();
        }

        if (Input.GetKeyDown("g"))
        {
           // SteamUserStats.ResetAllStats(true);
        }

    }
}
