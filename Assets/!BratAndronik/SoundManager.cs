using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public GameObject optmenu;
    public static float _SoundE;
    public static float SoundE

    
  
   
    {
        get { return _SoundE; }
        set
        {
            _SoundE = value;
            PlayerPrefs.SetFloat("SoundE", _SoundE);
        }
    }

    public static float _SoundM;
    public static float SoundM
    {
        get { return _SoundM; }
        set
        {
            _SoundM = value;
            PlayerPrefs.SetFloat("SoundM", _SoundM);
        }
    }
    public float SoundtestM = 1f;
    public float SoundtestE = 1f;
    public bool test = true;
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
      if(!PlayerPrefs.HasKey("SoundM")) PlayerPrefs.SetFloat("SoundM", 1f);
        if(!PlayerPrefs.HasKey("SoundE")) PlayerPrefs.SetFloat("SoundE", 1f);

        SoundM = PlayerPrefs.GetFloat("SoundM");
        SoundE = PlayerPrefs.GetFloat("SoundE");

        if (test)
        {
            SoundE = SoundtestE;
            SoundM = SoundtestM;
        }


    }

    // Update is called once per frame
    void Update()
    {
        if(optmenu == null) return;

        if (optmenu.activeSelf)
        {
            SoundM = PlayerPrefs.GetFloat("SoundM");
            SoundE = PlayerPrefs.GetFloat("SoundE");
            
            
        }


    }


}
