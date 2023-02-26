using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpMessage : MonoBehaviour
{
    public string myMessage = "";
    public bool isActive = false;
    public bool isOff = false;

    public float myTimer = 10f;
    public float myLifeTime = 5f;
    
    bool isTwinkle = false;
    float step = 0.01f;
    float dir = 1;
    float f = 1;

    public void StartTwinkle()
    {
        isTwinkle = true;
        f = 1;
    }

    public void StopTwinkle()
    {
        isTwinkle = false;
        var cl = GetComponent<Text>().color;
        GetComponent<Text>().color = new Color(cl.r, cl.g, cl.b, 1);
    }

    private void OnGUI()
    {
        if (!isTwinkle) return;

        f -= step * 0.5f * dir;
        var cl = GetComponent<Text>().color;
        GetComponent<Text>().color = new Color(cl.r, cl.g, cl.b, f);

        if (f <= 0)
        {
            f = 0.01f;
            dir *= -1;
        }

        if (f >= 1)
        {
            f = 0.99f;
            dir *= -1;
        }
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        StartTwinkle();
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((TimeController.instance.tm > myTimer) && !isActive)
        {

            gameObject.GetComponent<Text>().text = myMessage;
            isActive = true;

        }


        if ((TimeController.instance.tm > (myTimer + myLifeTime)) && isActive && !isOff)
        {

            isOff = true;

            gameObject.GetComponent<Text>().text = "";

        }


    }
}
