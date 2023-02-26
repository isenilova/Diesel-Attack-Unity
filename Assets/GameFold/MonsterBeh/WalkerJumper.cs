using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerJumper : MonoBehaviour {

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


    //we jump in the same area

    float j0 = 3.0f;
    public float t0 = 0;
    bool doo = false;


    public IEnumerator StartJumping()
    {
        Vector3 ep = Vector3.zero;

        var savedPos = transform.position;
        int cnt = (int)(FPSCounter.fps * Mathf.Abs(spd) * Random.Range(0.5f, 1.5f));
        float k = 0.001f;
        int tr = 40;
        ep = transform.position + new Vector3(spd * dir, 0, 0) * 3;

        for (int i = 0; i < 1; i++)
        {
            var dlt = (ep - transform.position) / cnt;

            float a;
            float b;
            float c;

            float x1;
            float y1;
            float x2;
            float y2;
            float x3;
            float y3;

            x1 = transform.position.x;
            y1 = transform.position.y;

            x3 = ep.x;
            y3 = ep.y;
            //y3 -= 0.5f;

            x2 = x1 + (x3 - x1) * 0.5f;

            y2 = y1 + 3.0f * Mathf.Abs(spd);
            /*
            if (y3 > y1)
                y2 = y1 + (y3 - y1) * 1.3f;
            else
                y2 = y1 + (y3 - y1) * 0.75f;
                */


            a = (y3 - (x3 * (y2 - y1) + x2 * y1 - x1 * y2) / (x2 - x1)) / (x3 * (x3 - x1 - x2) + x1 * x2);
            b = (y2 - y1) / (x2 - x1) - a * (x1 + x2);
            c = (x2 * y1 - x1 * y2) / (x2 - x1) + a * x1 * x2;

            var dx = 0.1f;
            if (x3 < x1)
            {
                dx *= -1;
                //transform rotate
            }

            //yield return new WaitForSeconds(3.0f);

            for (int j = 0; j < cnt; j++)
            {
                float x0 = x1 + j * (x3 - x1) / cnt;
                float y0 = a * x0 * x0 + b * x0 + c;
                transform.position = new Vector3(x0, y0, transform.position.z);

                yield return null;

            }

            transform.position = new Vector3(transform.position.x, ep.y, transform.position.z);


        }

        yield return null;

        canWalk = true;
    }

    private void Start()
    {
        GetComponent<Animator>().SetBool("Idle", true);
        //GetComponent<Animator>().CrossFade("RetroBlobWalk", 0.2f);
        t0 = Random.Range(0, j0);
    }

    void Update()
    {

       // if (Input.GetKeyDown("b")) doo = true;

        if (!doo) return;

        t0 += Time.deltaTime;
        if (t0 > j0 && canWalk)
        {
            canWalk = false;
            t0 = 0;
            StartCoroutine(StartJumping());
        }

        if (!canWalk) return;

        rh = Physics2D.Raycast(transform.position + new Vector3(uped.x, uped.y, 0), Vector2.right * dir, len, 1 << LayerMask.NameToLayer("Platforms"));
        t += Time.deltaTime;

        if (t > nt)
        {
            canC = true;
        }

        if (rh.collider != null && canC)
        {
            Debug.Log(rh.collider);
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
