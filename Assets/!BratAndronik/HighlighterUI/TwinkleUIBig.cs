using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TwinkleUIBig : MonoBehaviour {

    bool isTwinkle = false;
    float step = 0.1f;
    float dir = 1;
    Vector2 f = new Vector2(0, 0);

    Vector2 startSz = new Vector2(0, 0);
    Vector2 szBig = new Vector2(0, 0);

//    private Meta rm;

    public void Start()
    {
        startSz = GetComponent<RectTransform>().sizeDelta;
    }

    public void StartTwinkle()
    {
        if (isTwinkle) return;

        if (startSz.x > 0) StopTwinkle();

        startSz = GetComponent<RectTransform>().sizeDelta;
       // rm = (Meta) DatabaseAll.instance.data["Meta"]["meta0"];
        //szBig = startSz * rm.twinkleSize;
        isTwinkle = true;
        f = startSz;
    }

    public void StopTwinkle()
    {
        isTwinkle = false;
        var cl = GetComponent<Image>().color;
        GetComponent<RectTransform>().sizeDelta = startSz;
    }

    private void Update()
    {
        if (!isTwinkle) return;

       // if (rm != null)
        //{
          //  step = rm.twinkleSpeed;
        //}

        f += new Vector2( step * dir, step * dir);
        var cl = GetComponent<Image>().color;
        GetComponent<RectTransform>().sizeDelta = f;

        if (f.x > szBig.x)
        {
            f = new Vector2 (szBig.x - 0.01f, szBig.y - 0.01f);
            dir *= -1;
        }

        if (f.x < startSz.x)
        {
            f = new Vector2(startSz.x + 0.01f, startSz.y + 0.01f);
            dir *= -1;
        }
    }
}
