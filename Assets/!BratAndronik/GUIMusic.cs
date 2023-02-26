using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIMusic : MonoBehaviour
{
    public float myVol;
    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("SoundM")) PlayerPrefs.SetFloat("SoundM", 1f);

        myVol = PlayerPrefs.GetFloat("SoundM");
        
        gameObject.GetComponent<AudioSource>().volume = myVol / 2.000f;
    }

    // Update is called once per frame
    void Update()
    {

        if (PlayerPrefs.GetFloat("SoundM") != myVol)
        {
            myVol = PlayerPrefs.GetFloat("SoundM");

            gameObject.GetComponent<AudioSource>().volume = myVol / 2.000f ;

            //PlayerPrefs.SetFloat("SoundM", myVol);
        }

    }
}
