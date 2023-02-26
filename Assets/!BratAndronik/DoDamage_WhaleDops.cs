using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoDamage_WhaleDops : DoDamage
{
    
    
    public GameObject myDamagePartEffects;
    public float myDamageEffectTimer = 2f;
    private float myTimer = 0f;
   

    private Transform childEff;




    public override void Do(float val)
    {
        myTimer = 0f;
        
        for (int i = 0; i < myDamagePartEffects.transform.childCount; i++)
        {
            childEff = myDamagePartEffects.transform.GetChild(i);
            childEff.gameObject.SetActive(true);

        }

        StartCoroutine("EffectControl");

    }


    IEnumerator EffectControl()
    {
        while (myTimer < myDamageEffectTimer)
        {
            myTimer += Time.deltaTime;



            yield return null;
        }
            
        for (int i = 0; i < myDamagePartEffects.transform.childCount; i++)
        {
            childEff = myDamagePartEffects.transform.GetChild(i);
            childEff.gameObject.SetActive(false);

        }

    }
}
