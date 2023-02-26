using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TwinkleUI : MonoBehaviour {

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
        var cl = GetComponent<Image>().color;
        GetComponent<Image>().color = new Color(cl.r, cl.g, cl.b, 1);
    }

    private void OnGUI()
    {
        if (!isTwinkle) return;

        f -= step * 0.1f * dir;
        var cl = GetComponent<Image>().color;
        GetComponent<Image>().color = new Color(cl.r, cl.g, cl.b, f);

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
}
