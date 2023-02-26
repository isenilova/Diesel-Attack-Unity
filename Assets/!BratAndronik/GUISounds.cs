using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUISounds : MonoBehaviour
{
    public AudioClip[] myButSounds;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onButtonClick(int i= 0)
    {
        if((i < 0)||(i >= myButSounds.Length)) return;
        
        
        AudioSource.PlayClipAtPoint((myButSounds[i]), Camera.main.transform.position, SoundManager.SoundE / 2.000f);
        
        
    }

}
