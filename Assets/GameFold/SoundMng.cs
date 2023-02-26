using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundMng : MonoBehaviour {

    public static SoundMng instance;

    public GameObject sndOn;
    public GameObject sndOff;

    public GameObject view;

    bool isOpen = false;

    public GameObject track;
    public Slider sldr;

    private void Awake()
    {
        float f = 1;
        if (PlayerPrefs.HasKey("music"))
        {
            f = PlayerPrefs.GetFloat("music");
        }
        sldr.value = f;

        var t = FindObjectOfType<AudioSource>();

        if (t != null)
        {
            t.volume = f;
        }

    }

    public void Activate()
    {
        isOpen = !isOpen;
        view.SetActive(isOpen);
    }

    public void SliderChange(float f)
    {
        if (f == 0)
        {
            sndOn.SetActive(false);
            sndOff.SetActive(true);
        }
        else
        {
            sndOn.SetActive(true);
            sndOff.SetActive(false);
        }

        PlayerPrefs.SetFloat("music", f);
        var t = FindObjectOfType<AudioSource>();

        if (t != null)
        {
            t.volume = f;
        }
    }

    public void Close()
    {
        isOpen = false;
        view.SetActive(false);
    }

    private void Update()
    {
        if (track != null && !track.activeInHierarchy && isOpen)
        {
            isOpen = false;
            view.SetActive(false);
        }
    }

}
