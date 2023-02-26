using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrgArea : MonoBehaviour {

    bool isTriggered = false;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;

        if (isTriggered) return;

        isTriggered = true;

        GetComponentInParent<BezSpawner>().Triggered();
    }
}
