using MoreMountains.CorgiEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickyJumper : MonoBehaviour
{

    public Transform hand;

    float savedRot;
    Vector3 savedPos;

    GameObject player;
    float minDstX = 2.5f;
    float minDstY = 1.5f;

    string state = "idle";

    float rotSpd = 1;
    float maxRot = 100;

    int flip = 0;
    float cnt = 0;


    float t = 0;
    float tMax = 3.0f;
    float spd = 1.0f;

    RaycastHit2D rh;

    public SkinnedMeshRenderer sm;

    private void Start()
    {
        Debug.Log(savedRot);

        player = GameObject.FindGameObjectWithTag("Player");

        sm.enabled = false;

        savedPos = transform.position;

    }


    // Update is called once per frame
    void Update()
    {

        t += Time.deltaTime;

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            return;
        }

        if (state == "idle")
        {
            var dlt = player.transform.position - transform.position;
            //Debug.Log(Mathf.Abs(dlt.y).ToString() + "    " + Mathf.Abs(dlt.x).ToString());
            if (Mathf.Abs(dlt.y) < minDstY && Mathf.Abs(dlt.x) < minDstX)
            {
                sm.enabled = true;
                state = "jumpy";
                t = 0;
                savedPos = transform.position;
            }

            return;

        }

        if (state == "jumpy")
        {

            rh = Physics2D.Raycast(transform.position, Vector2.down, 0.01f, 1 << LayerMask.NameToLayer("Platforms"));

            Vector3 newPos = savedPos + new Vector3(0, -2 * t * t + 4 * t, 0);

            transform.position = newPos;

            if (t > 2.2f || (t > 1f && rh.collider != null))
            {
                state = "walk";
                gameObject.AddComponent<Walker>();
                gameObject.GetComponent<Walker>().uped = new Vector2(0, 0.3f);
                gameObject.AddComponent<KillPlayerOnTouch>();
            }



        }



    }

}
