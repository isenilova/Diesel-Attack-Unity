using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullLifeController : MonoBehaviour
{
    public GameObject myBattary;

    public float zSpaun = 0f;

    private int lastSpawn = 0;


    public float[] myAppearTimers;

  
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(lastSpawn >= myAppearTimers.Length) return;

        if ((TimeController.instance.tm > myAppearTimers[lastSpawn]) &&
            (TimeController.instance.tm < (myAppearTimers[lastSpawn] + 1f)))
        {
            lastSpawn++;

            Instantiate(myBattary, new Vector3(CamBound.instance.hix.position.x, 0f, zSpaun),
                Camera.main.transform.rotation);


        }



    }
}
