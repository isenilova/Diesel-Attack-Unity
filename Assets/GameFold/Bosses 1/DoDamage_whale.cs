using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoDamage_whale : DoDamage
{
    private bool dod = false;
    public float spd = 10.0f;
    public float tm = 3.0f;

    public Transform who;



 
    
    public override void Do(float val)
    {

        if (val >= 0)
        {
            dod = false;
            StopAllCoroutines();
            return;
        }

        if (dod)
        {
            return;
        }

        StartCoroutine(DoFloat());

    }

    public IEnumerator DoFloat()
    {
        dod = true;
        float t = 0;
        while (t < tm)
        {
            t += Time.deltaTime;
            who.transform.Translate(0, -spd * Time.deltaTime, 0);
            yield return null;
        }
        
        t = 0;
        while (t < tm)
        {
            t += Time.deltaTime;
            who.transform.Translate(0, spd * Time.deltaTime, 0);
            yield return null;
        }

        dod = false;
        

    }


   
}
