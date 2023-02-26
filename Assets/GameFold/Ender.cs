using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class Ender : MonoBehaviour {

    public GameObject model;
    int mstate = 0;
    public Transform ep;
    float spd = 3;
    // Use this for initialization
    public GameObject toActivate;

    public static Ender instance;


    public GameObject hat;
    public Transform endObj;
    public GameObject credits;


    int mode = 0;


    private void Awake()
    {
        instance = this;
    }

    void Start () {

        mstate = 1;
        model.GetComponentInChildren<Animator>().CrossFade("spine-space-corgi-walk", 0.2f);
	}

    public void WeCome()
    {
        toActivate.SetActive(true);
    }

    public void DoneReading()
    {
        mstate = 3;
        Debug.Log("We are here");
        StartCoroutine(ShowTitles());
        //we rotate hat and show credits
    }

    public IEnumerator ShowTitles()
    {
        var go = (GameObject)Instantiate(hat);
        go.transform.position = hat.transform.position;
        go.transform.localScale = new Vector3(1,1,1);
        go.transform.rotation = Quaternion.identity;

        float spd = 10f;
        Vector3 vec = endObj.position - go.transform.position;

        float dl = 0.2f;
        float rs = 10;

        while ((go.transform.position - endObj.position).magnitude > dl )
        {
            go.transform.Rotate(0, spd, 0);
            go.transform.position += vec.normalized * spd * Time.deltaTime;
            go.transform.localScale += (new Vector3(4f, 4, 4)) * Time.deltaTime;

            yield return null;
        }


        go.transform.eulerAngles = new Vector3(0, -90, 0);

        credits.SetActive(true);

        mode = 4;
    }
	
	// Update is called once per frame
	void Update ()
    {

        if (mstate == 0) return;

        var vec = ep.position - model.transform.position;
        if (vec.magnitude < Time.deltaTime * spd && mstate == 1)
        {

            model.transform.position = ep.position;

            mstate = 2;
            WeCome();

        }
        else
        {
            model.transform.position += vec.normalized * spd * Time.deltaTime;
            //model.transform.forward = -vec;
        }


        if (mode == 4 && Input.anyKeyDown)
        {
            LoadingSceneManager.LoadScene("level_start");
        }

    }
}
