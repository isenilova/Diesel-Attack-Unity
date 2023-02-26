using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonExplosion : MonoBehaviour {

    public List<OneExplosion> explosions = new List<OneExplosion>();

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag != "Player") return;
        //Debug.Log("enetred");
        foreach (var ep in explosions)
        {
            ep.TriggerMe();
        }        
    }

}
