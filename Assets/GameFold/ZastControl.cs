using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextFx;
using MoreMountains.CorgiEngine;

public class ZastControl : MonoBehaviour {


    public GameObject model;
    int mstate = 0;
    public Transform ep;
    float spd = 3;

    public AudioClip clip;


    public TextFxUGUI tt;
    public TextFxUGUI tt1;


    bool isLoad = false;


    // Use this for initialization
    void Start ()
    {

        FindObjectOfType<AudioSource>().clip = clip;
        FindObjectOfType<AudioSource>().Play();

        mstate = 1;

    }

    public void WeCome()
    {
        FindObjectOfType<AudioSource>().Stop();

        tt.gameObject.SetActive(true);
        tt1.gameObject.SetActive(true);

        model.GetComponent<Animator>().CrossFade("jumping", 0.2f);
        //tt.pl;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.anyKeyDown && !isLoad)
        {
            isLoad = true;
            LoadingSceneManager.LoadScene("level_start");
        }

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
            if (vec.magnitude > 1e-5f)
            {
                model.transform.position += vec.normalized * spd * Time.deltaTime;
                model.transform.forward = -vec;
            }
            else
            {
                model.transform.forward = new Vector3(0, 0, 1);
            }
           
        }
	}
}
