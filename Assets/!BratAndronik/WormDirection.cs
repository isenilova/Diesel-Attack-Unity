using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormDirection : MonoBehaviour
{

    public GameObject myDirection;

    private bool changeDirection = false;
    private bool olddir = false;

    private GameObject PlayerObj;

    private SmoothFollow followScr;

    public float[] DirectionTimer;

    public float curTimer = 0f;
   public bool actDirection = false;
    private int changeDirNum = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerObj = GameObject.FindGameObjectWithTag("Player");
        followScr = gameObject.GetComponent<SmoothFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!actDirection) return;

        curTimer += Time.deltaTime;
        
        if(changeDirNum >= DirectionTimer.Length) return;

        if (curTimer >= DirectionTimer[changeDirNum])
        {

            changeDirection = !changeDirection;
            changeDirNum++;


        }

        if(olddir == changeDirection) return;


        if (changeDirection)
        {
            olddir = changeDirection;
            followScr.plr = myDirection;
            


        }

        else
        {
            olddir = changeDirection;
            followScr.plr = PlayerObj;
        }



    }
}
