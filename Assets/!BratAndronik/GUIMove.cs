using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIMove : MonoBehaviour
{
    public int myMoveNumUpgr;

    public float[] myMoveCoef;

    public float durat = 100000f;
    
    // Start is called before the first frame update
    void Start()
    {
        myMoveNumUpgr = PlayerPrefs.GetInt("M");
        
        if((myMoveNumUpgr <0) || (myMoveNumUpgr > 3)) return;

        GameController.instance.AcceptPick("1", "speed", "xx", durat, myMoveCoef[myMoveNumUpgr], 0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
       
        
        
    }
}
