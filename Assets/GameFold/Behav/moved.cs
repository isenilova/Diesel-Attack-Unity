using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moved : MonoBehaviour {

    float spd = 2;
    public Vector2 dir;


    public void SetDir(Vector2 v2)
    {
        dir = v2;
    }


    private void Update()
    {
        transform.position += new Vector3(dir.x, dir.y, 0) * spd * Time.deltaTime;
    }

}
