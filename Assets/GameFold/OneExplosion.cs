using MoreMountains.CorgiEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OneExplosion : MonoBehaviour {

    float tMax = 3.0f;
    float t = 0;
    int state = 0;
    float rad = 4;

    public GameObject explosion;

    public SuperTextMesh text;

	public void TriggerMe()
    {
        if (state != 0) return;

        state = 1;
        //waiting for trigger
    }

    public void MakeExplosion()
    {
        var cols = Physics2D.OverlapCircleAll(transform.position, rad);

        var g1 = (GameObject)Instantiate(explosion);
        g1.transform.position =  new Vector3(transform.position.x, transform.position.y, -6);

        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].tag == "Player")
            {
                LevelManager.Instance.KillPlayer(cols[i].GetComponent<Character>());
                continue;
            }

            //SpriteSlicer2DDemoManager.instance.ExplodeSprite(cols[i].gameObject, 2, 2);
        }

        Destroy(gameObject);
    }

	// Update is called once per frame
	void Update ()
    {
        if (state == 0) return;

        t += Time.deltaTime;

        BombTimer.instance.SetTimer(gameObject, Mathf.CeilToInt(tMax - t));
        //text.text = Mathf.CeilToInt(tMax - t).ToString(); 

        if (t > tMax)
        {
            t = 0;
            text.text = "";
            BombTimer.instance.SetTimer(gameObject, -1);
            MakeExplosion();
            state = 0;
        }



	}
}
