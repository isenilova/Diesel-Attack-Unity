using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoDamage : MonoBehaviour {

    public Light[] light;

    public float i1 = 1.0f;
    public float i2 = 3.0f;
    public float delay = 0.1f;
    public float fullTime = 2.0f;

    public bool useColor = false;

    public Color changedColor1;
    public Color changedColor2;

    float savedIntensity;
    Color savedColor;

    private void Start()
    {
        //i1 *= 2;
        //i2 *= 2;
        
        if (light != null)
        {

            for (int i = 0; i < light.Length; i++)
            {
                savedColor = light[i].color;
                savedIntensity = light[i].intensity;
            }
        }
    }

    public virtual void Do(float val)
    {

       // Debug.Log(light.name);
        //StopAllCoroutines();

        if (val <= 0)
        {
            for (int i = 0; i < light.Length; i++)
            light[i].intensity = 0;
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(Twinkle());
        }
    }


    public IEnumerator Twinkle()
    {
        float t = 0;
        int q = 0;

        for (int i = 0; i < light.Length; i++)
        {
            light[i].intensity = savedIntensity;
            light[i].color = savedColor;
        }

        while (t < fullTime)
        {
            if (q == 0)
            {
                for (int i = 0; i < light.Length; i++)
                light[i].intensity = i1;
                q = 1;

                if (useColor)
                {
                    for (int i = 0; i < light.Length; i++)
                    light[i].color = changedColor1;
                }
            }
            else
            {
                for (int i = 0; i < light.Length; i++)
                light[i].intensity = i2;
                q = 0;

                if (useColor)
                {
                    for (int i = 0; i < light.Length; i++)
                    light[i].color = changedColor2;
                }
            }

            yield return new WaitForSeconds(delay);

            t += delay;
        }

        for (int i = 0; i < light.Length; i++)
        {
            light[i].intensity = savedIntensity;
            light[i].color = savedColor;
        }
    }
}
