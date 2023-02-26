using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rofler : MonoBehaviour {

    public string what = "stun";

    private void OnMouseDown()
    {
        GetComponent<Animator>().CrossFade(what, 0.2f);
    }
}
