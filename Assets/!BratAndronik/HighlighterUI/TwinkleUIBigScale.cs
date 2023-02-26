using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TwinkleUIBigScale : MonoBehaviour {

    bool isTwinkle = false;
    float step = 0.1f;
    float dir = 1;

    Vector2 fMin = Vector2.zero;
    Vector2 fMax = Vector2.zero;


    public Vector2 startSzMin = Vector2.zero;
    public Vector2 startSzMax = Vector2.zero;


    public Vector2 szBigMin = Vector2.zero;
    public Vector2 szBigMax = Vector2.zero;

    private void Start()
    {
        startSzMin = GetComponent<RectTransform>().anchorMin;
        startSzMax = GetComponent<RectTransform>().anchorMax;

    }

    public void StartTwinkle()
    {
        if (isTwinkle) return;

        if (startSzMin.x > 0) StopTwinkle();

        startSzMin = GetComponent<RectTransform>().anchorMin;
        startSzMax = GetComponent<RectTransform>().anchorMax;

        szBigMax = startSzMax * 1.05f;

        isTwinkle = true;
        fMin = startSzMin;
        fMax = startSzMax;
    }

    public void StopTwinkle()
    {
        isTwinkle = false;
        var cl = GetComponent<Image>().color;

        GetComponent<RectTransform>().anchorMin = startSzMin;
        GetComponent<RectTransform>().anchorMax = startSzMax;

        GetComponent<RectTransform>().offsetMin = Vector2.zero;
        GetComponent<RectTransform>().offsetMax = Vector2.zero;



    }

    private void Update()
    {
        if (!isTwinkle) return;

        fMax += new Vector2( step * dir * Time.deltaTime, step * dir*Time.deltaTime);
        fMin -= new Vector2(step * dir * Time.deltaTime, step * dir * Time.deltaTime);

        var cl = GetComponent<Image>().color;


        if (fMax.x > szBigMax.x)
        {
            fMax -= new Vector2(step * dir * Time.deltaTime, step * dir * Time.deltaTime);
            fMin += new Vector2(step * dir * Time.deltaTime, step * dir * Time.deltaTime);
            dir *= -1;
        }

        if (fMax.x < startSzMax.x)
        {
            fMax -= new Vector2(step * dir * Time.deltaTime, step * dir * Time.deltaTime);
            fMin += new Vector2(step * dir * Time.deltaTime, step * dir * Time.deltaTime);
            dir *= -1;
        }

        //
        GetComponent<RectTransform>().anchorMin = fMin;
        GetComponent<RectTransform>().anchorMax = fMax;

        GetComponent<RectTransform>().offsetMin = Vector2.zero;
        GetComponent<RectTransform>().offsetMax = Vector2.zero;

    }
}
