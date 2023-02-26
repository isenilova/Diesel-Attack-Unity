using MoreMountains.CorgiEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faller : MonoBehaviour {

    public GameObject player;
    bool thrown = false;

    public bool inFall = false;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (inFall)
        {
            thrown = true;
            GetComponent<CorgiController>().enabled = true;
            gameObject.AddComponent<Walker>();
            gameObject.GetComponent<Walker>().uped = new Vector2(0, 0.3f);
            gameObject.AddComponent<KillPlayerOnTouch>();
        }

    }

    private void Update()
    {

        if (thrown) return;

        if (player == null)
        {
            Start();
            return;
        }

        var dlt = player.transform.position - transform.position;

        if (Mathf.Abs(dlt.x) < 1.5f && -dlt.y < 10)
        {
            thrown = true;
            GetComponent<CorgiController>().enabled = true;
            gameObject.AddComponent<Walker>();
            gameObject.GetComponent<Walker>().uped = new Vector2(0, 0.3f);
            gameObject.AddComponent<KillPlayerOnTouch>();
        }

    }
}
