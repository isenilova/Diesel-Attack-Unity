using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour {

    public float spd = 1;
    public float dir = 1;
    public float len = 1;

    RaycastHit2D rh;
    RaycastHit2D rh1;
    RaycastHit2D rh2;

    public bool avoidFall = false;

    public Vector2 uped = new Vector2(0, 0);

    public float nt = 1f;
    float t = 0;
    bool canC = true;
    public bool arachna = false;

    public bool canWalk = true;

    private void Start()
    {
        GetComponent<Animator>().SetBool("Idle", true);
        //GetComponent<Animator>().CrossFade("RetroBlobWalk", 0.2f);
    }

    void Update()
    {

        if (!canWalk) return;

        rh = Physics2D.Raycast(transform.position + new Vector3(uped.x, uped.y, 0), Vector2.right * dir, len, 1 << LayerMask.NameToLayer("Platforms"));
        t += Time.deltaTime;

        if (t > nt)
        {
            canC = true;
        }

        if (rh.collider != null && canC)
        {
            if (arachna)
            {
                Debug.Log(rh.collider);
            }


            if (rh.collider.tag == "mina") return;
            //Debug.Log("hitted");
            //Debug.Log(rh.collider);
            dir *= -1;

            if (arachna)
            {
                if (dir == 1) transform.Rotate(0, -100, 0);
                else transform.Rotate(0, 100, 0);
            }

            canC = false;
            t = 0;

            return;
        }

        if (avoidFall)
        {
            rh = Physics2D.Raycast(transform.position + new Vector3(dir, 0, 0), Vector2.down, 3f, 1 << LayerMask.NameToLayer("Platforms"));
            rh1 = Physics2D.Raycast(transform.position + new Vector3(dir - 0.2f, 0, 0), Vector2.down, 3f, 1 << LayerMask.NameToLayer("Platforms"));
            rh2 = Physics2D.Raycast(transform.position + new Vector3(dir + 0.2f, 0, 0), Vector2.down, 3f, 1 << LayerMask.NameToLayer("Platforms"));


            if (rh.collider == null && rh1.collider == null && rh2.collider == null)
            {
                dir *= -1;
                return;
            }
            else
            {
                //Debug.Log(rh.collider.gameObject);
            }
        }


        //Debug.Log("we are moving");
        transform.position += new Vector3(spd * Time.deltaTime * dir, 0, 0);

    }
}
