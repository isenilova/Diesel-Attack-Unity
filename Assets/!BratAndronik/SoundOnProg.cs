using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnProg : MonoBehaviour
{
    public AudioClip onAppear;
    public AudioClip onDng;
    public AudioClip onDeath;

    public bool useOneHealth = false;
    private OneHealth hpScr;
    private float curHp;
    private bool playOnGeath = false;
    
    // Start is called before the first frame update
    void Start()
    {
        if (useOneHealth)
        {
            hpScr = gameObject.GetComponent<OneHealth>();
            curHp = hpScr.curHealth;
        }
        
        
        if(onAppear != null) AudioSource.PlayClipAtPoint(onAppear, Camera.main.transform.position, SoundManager.SoundE);
    }

    // Update is called once per frame
    void Update()
    {

        if(!useOneHealth) return;
        
        if (hpScr.curHealth <= 0f)
        {
            if ((onDeath != null) && (!playOnGeath))
            {
                playOnGeath = true;
                
                AudioSource.PlayClipAtPoint(onDeath, Camera.main.transform.position, SoundManager.SoundE);
            }

        }
        else
        {

            if ((hpScr.curHealth > curHp)&&(onDng != null))
            {
                curHp = hpScr.curHealth;
                
                
                AudioSource.PlayClipAtPoint(onDng, Camera.main.transform.position, SoundManager.SoundE);

            }

        }


    }
}
