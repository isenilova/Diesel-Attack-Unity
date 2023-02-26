using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthTrack : MonoBehaviour {

    public string nm;

    private void Start()
    {
        BossHealth.instance.nm = nm;
        BossHealth.instance.who = gameObject;
    }
}
