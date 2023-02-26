using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class AbilGet : MonoBehaviour {

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            go.GetComponent<CharacterJetpack>().enabled = true;
            Destroy(gameObject);
        }
    }
}
