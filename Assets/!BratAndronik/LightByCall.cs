using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightByCall : MonoBehaviour
{
    public float myIntenc = 30f;

    public float myOff = 1f;
    private float myTimer = 0f;

    private bool isLight = false;
    private Light myLight;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        myLight = gameObject.GetComponent<Light>();

    }

    // Update is called once per frame
    void Update()
    {
        if (isLight)
        {
            myTimer += Time.deltaTime;

            if (myTimer >= myOff)
            {
                isLight = false;

                myTimer = 0f;

                myLight.intensity = 0f;


            }


        }



    }

    public void onMyLight()
    {
        isLight = true;

        myLight.intensity = myIntenc;




    }
}
