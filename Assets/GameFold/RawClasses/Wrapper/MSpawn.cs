using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSpawn : MonoBehaviour
{
    public string id;
    private bool isLoaded = false;


    void Update()
    {
        if (!isLoaded && DatabaseAll.instance.isLoaded)
        {
            isLoaded = true;
            var mon = (Spawn)DatabaseAll.instance.data["Spawn"][id];

            if (mon.typo == "common")
            {
                GetComponent<BezSpawner>().timeActivate = mon.tm;
                GetComponent<BezSpawner>().delay = mon.delay;
                GetComponent<BezSpawner>().amount = mon.amount;
            }
            else if (mon.typo == "boss1")
            {
                GetComponent<BossBehav1>().tm = mon.tm;
            }
            else if (mon.typo == "boss2")
            {
                GetComponent<BossBehav_Whale>().tm = mon.tm;
            }
            else if (mon.typo == "worm")
            {
                GetComponent<WormBoss>().tm = mon.tm;
            }
        }
    }

}
