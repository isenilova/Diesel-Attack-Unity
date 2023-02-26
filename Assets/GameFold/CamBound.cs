using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamBound : MonoBehaviour {

    // Use this for initialization
    public Transform lox;
    public Transform loy;
    public Transform hix;
    public Transform hiy;

    public static CamBound instance;

    private void Awake()
    {
        instance = this;
    }

    void Start ()
    {

        var rt1 = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10));
        var rt2 = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 10));

        lox.position = new Vector3(rt1.x, (rt1.y + rt2.y) / 2, lox.position.z);
        hix.position = new Vector3(rt2.x, (rt1.y + rt2.y) / 2, hix.position.z);
        loy.position = new Vector3((rt1.x + rt2.x)/2, rt1.y, loy.position.z);
        hiy.position = new Vector3((rt1.x + rt2.x)/2, rt2.y, hiy.position.z);



    }

}
