using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : MonoBehaviour {

    float range = 5;
    float dir = 1;
    public float spd = 1;
    // Use this for initialization
    public bool useBomb = true;
    public GameObject bomb;
    Vector3 savedPos;

    float dropTime = 2.0f;
    float dt = 0;
    public float projRot = 180;

    private void Start()
    {
        savedPos = transform.position;
    }

    // Update is called once per frame
    void Update ()
    {
        dt += Time.deltaTime;
		if (transform.position.x + spd * Time.deltaTime * dir > savedPos.x + range)
        {
            dir *= -1;
            transform.forward = -transform.forward;
            return;
        }

        if (transform.position.x + spd * Time.deltaTime * dir < savedPos.x - range)
        {
            dir *= -1;
            transform.forward = -transform.forward;
            return;
        }

        transform.position += new Vector3(spd * Time.deltaTime * dir, 0, 0);

        if (dt > dropTime && useBomb)
        {
            dt -= dropTime;
            var go = (GameObject)Instantiate(bomb, transform.position, Quaternion.identity);
            go.transform.Rotate(0, 0, projRot);
        }
    }
}
