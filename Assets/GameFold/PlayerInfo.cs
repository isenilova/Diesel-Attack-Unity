using MoreMountains.CorgiEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour {

    public Character player;
    public static PlayerInfo instance;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    public void OnLevelWasLoaded(int level)
    {
        
    }
}
