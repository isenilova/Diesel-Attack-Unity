using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUISliderSound : MonoBehaviour
{
    public string myPrefs = "SoundM";
    
    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey(myPrefs)) PlayerPrefs.SetFloat(myPrefs, 1f);
        gameObject.GetComponent<Slider>().value = PlayerPrefs.GetFloat(myPrefs);

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetSoundM(float a)
    {
        
        
        PlayerPrefs.SetFloat(myPrefs, a);
        
        
    }

}
