using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontStart : MonoBehaviour
{
    private bool act1 = false;

    private bool act2 = false;
    
    public float myTimer1 = 36f;
    public float myTimer2 = 50f;

    public float myY = 1f;
    public float myZ = -0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!act1 && TimeController.instance.tm >= myTimer1)
        {


            var ch = gameObject.transform.GetChild(0);
            ch.gameObject.SetActive(true);
            ch.position = new Vector3(CamBound.instance.hix.transform.position.x, myY, myZ);

            act1 = true;

        }
        
        if (!act2 && TimeController.instance.tm >= myTimer2)
        {


            var ch2 = gameObject.transform.GetChild(1);
            ch2.gameObject.SetActive(true);
            ch2.position =  new Vector3(CamBound.instance.hix.transform.position.x, myY, myZ);
            act2 = true;

        }

    }
}
