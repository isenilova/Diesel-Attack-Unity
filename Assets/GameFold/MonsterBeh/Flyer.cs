using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flyer : MonoBehaviour
{

    public GameObject player;
    bool isLoad = false;

    float spd = 1f;


    private void Update()
    {
        if (!isLoad)
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go == null) return;

            player = go;
            isLoad = true;
            return;
        }


        //
        var vec = player.transform.position - transform.position;
        vec.Normalize();
        transform.position += vec * Time.deltaTime * spd;

    }


}
