using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownWheels : MonoBehaviour
{
    public float rotSpeed = 1f;

    public GameObject[] myWheels;

    private int i;

    public string direct = "y";
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

      if(direct == "y")  for (i = 0; i < myWheels.Length; i++)
            myWheels[i].transform.Rotate(0f, -rotSpeed, 0f);
        
        
        if(direct == "x")  for (i = 0; i < myWheels.Length; i++)
            myWheels[i].transform.Rotate(-rotSpeed, 0f, 0f);
        
        
    }
}
