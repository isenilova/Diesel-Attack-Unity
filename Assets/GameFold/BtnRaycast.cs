using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnRaycast : MonoBehaviour {

    float maxY = 20;
    float minY = 4;
    float dy = 0.1f;

    float dist = 2f;

    public GameObject door;

    RaycastHit2D rh;

    public float vv = 0;

    private void Start()
    {

    }
    // Update is called once per frame
    void Update ()
    {


        rh = Physics2D.Raycast(transform.position, Vector2.up, dist, 1 << LayerMask.NameToLayer("Player"));

        if (rh.collider == null)
        {
            rh = Physics2D.Raycast(transform.position + new Vector3(0.2f, 0, 0), Vector2.up, dist, 1 << LayerMask.NameToLayer("Player"));
        }

        if (rh.collider == null)
        {
            rh = Physics2D.Raycast(transform.position - new Vector3(0.2f, 0, 0), Vector2.up, dist, 1 << LayerMask.NameToLayer("Player"));
        }

        if (rh.collider != null)
        {
            if (Mathf.Abs(SpeedTracker.instance.speedV) < vv) return;
            //Debug.Log(rh.collider.GetComponent<Rigidbody2D>().velocity);
            door.GetComponent<DoorBehav>().Open();
            //Debug.Log(rh.collider.gameObject);
            float curSc = transform.localScale.y;
            if (curSc > minY)
            {
                curSc -= dy;
            }

            transform.localScale = new Vector3(transform.localScale.x, curSc, transform.localScale.z);

        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, maxY, transform.localScale.z);
        }
        
	}

    /*
    public void OnCollisionEnter2D(Collision2D collision)
    {

        Debug.Log(collision.gameObject.GetComponent<Rigidbody2D>().velocity);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.GetComponent<Rigidbody2D>().velocity);
    }
    */
}
